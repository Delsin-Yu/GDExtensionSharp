using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string Indents = "    ";

    private static readonly HashSet<string> _contextualKeywordList =
        new()
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while"
        };

    private static bool IsBuiltinType(this string name) => name switch
    {
        "Nil" => true,
        "void" => true,
        "bool" => true,
        "real_t" => true,
        "float" => true,
        "double" => true,
        "int" => true,
        "int8_t" => true,
        "uint8_t" => true,
        "int16_t" => true,
        "uint16_t" => true,
        "int32_t" => true,
        "int64_t" => true,
        "uint32_t" => true,
        "uint64_t" => true,
        _ => false
    };

    private static string OperatorIdToName(this string operatorId) => operatorId switch
    {
        "==" => "equal",
        "!=" => "not_equal",
        "<" => "less",
        "<=" => "less_equal",
        ">" => "greater",
        ">=" => "greater_equal",
        "+" => "add",
        "-" => "subtract",
        "*" => "multiply",
        "/" => "divide",
        "unary-" => "negate",
        "unary+" => "positive",
        "%" => "module",
        "<<" => "shift_left",
        ">>" => "shift_right",
        "&" => "bit_and",
        "|" => "bit_or",
        "^" => "bit_xor",
        "~" => "bit_negate",
        "and" => "and",
        "or" => "or",
        "xor" => "xor",
        "not" => "not",
        "in" => "in",
        _ => throw new ArgumentOutOfRangeException(nameof(operatorId))
    };

    private static string EscapeContextualKeyWord(this string name) => _contextualKeywordList.Contains(name) ? $"@{name}" : name;

    private static readonly Regex _pascalToSnakeRegex = new("(?<=[a-z0-9])([A-Z])|([A-Z][a-z])", RegexOptions.Compiled);

    private static readonly Regex _snakeToPascalRegex = new("(^|_)([a-z])", RegexOptions.Compiled);

    private static string SnakeCaseToPascalCase(this string input) => string.IsNullOrEmpty(input)
        ? input
        : _snakeToPascalRegex
            .Replace(input, m => m.Groups[2].Value.ToUpperInvariant());

    private static string PascalCaseToSnakeCase(this string input) => string.IsNullOrEmpty(input)
        ? input
        : _pascalToSnakeRegex
            .Replace(input, "_$0")
            .ToLower();

    private static readonly StringBuilder _stringBuilder = new();

    private static string InsertIndentation(this string source, int indentations = 1)
    {
        _stringBuilder.Append(source);
        for (var i = 0; i < indentations; i++)
        {
            _stringBuilder.Insert(0, Indents);
            _stringBuilder.Replace("\n", $"\n{Indents}");
        }

        return _stringBuilder.ToStringAndClear();
    }


    private static string ToStringAndClear(this StringBuilder stringBuilder)
    {
        if (stringBuilder.Length == 0) return string.Empty;

        while (stringBuilder[^1] == '\n' || stringBuilder[^1] == '\r')
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        var result = stringBuilder.ToString();
        stringBuilder.Clear();
        return result;
    }

    private static string MakeFunctionParameters(Span<Argument> parameters, bool includeDefault = false, bool forBuiltin = false)
    {
        if (parameters == null) return string.Empty;

        for (var index = 0; index < parameters.Length; index++)
        {
            var param = parameters[index];
            var type_name = param.Type;
            _stringBuilder.Append(GetTypeFromParameter(type_name, param.Meta));
            var parameterName = param.Name.EscapeContextualKeyWord();
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                parameterName = $"arg_{(index + 1).ToString(CultureInfo.InvariantCulture)}";
            }

            _stringBuilder.Append(parameterName);

            if (includeDefault && param.DefaultValue != null && (!forBuiltin || type_name != "Variant"))
            {
                _stringBuilder.Append(" = ");
                if (type_name.IsEnum())
                {
                    var paramType = CorrectType(type_name);
                    if (paramType == "void") paramType = "Variant";
                    _stringBuilder.AppendFormat("({0})", paramType);
                }

                _stringBuilder.Append(CorrectDefaultValue(param.DefaultValue, param.Type));
            }

            if (index != parameters.Length - 1)
            {
                _stringBuilder.Append(", ");
            }
        }

        return _stringBuilder.ToStringAndClear();
    }

    private static (string encode, string argName) GetEncodedArgs(string argName, string typeName, string typeMeta)
    {
        var name = argName.EscapeContextualKeyWord();
        var arg_type = CorrectType(typeName);
        if (arg_type.IsBuiltinType())
        {
            _stringBuilder.AppendLine($"{GetGDExtensionType(arg_type)} {name}_encoded = ({CorrectType(typeName)}){name};");
            name = $"&{name}_encoded";
        }
        else if (typeName.IsEngineClass())
        {
            // `{name}` is a C++ wrapper, it contains a field which is the object's pointer Godot expects.
            // We have to check `nullptr` because when the caller sends `nullptr`, the wrapper itself will be null.
            name = $"({name} != nullptr ? &{name}->_owner : nullptr)";
        }
        else
        {
            name = $"&{name}";
        }

        return (_stringBuilder.ToStringAndClear(), name);
    }

    private static string GetGDExtensionType(string typeName) => typeName switch
    {
        "bool" => "int8_t",
        "uint8_t" => "int64_t",
        "int8_t" => "int64_t",
        "uint16_t" => "int64_t",
        "int16_t" => "int64_t",
        "uint32_t" => "int64_t",
        "int32_t" => "int64_t",
        "int" => "int64_t",
        "float" => "double",
        _ => typeName
    };

    private static readonly Dictionary<string, string> _defaultValueCorrection =
        new()
        {
            {"null", "nullptr"},
            {"\"\"", "String()"},
            {"&\"\"", "StringName()"},
            {"[]", "Array()"},
            {"{}", "Dictionary()"},
            {"Transform2D(1, 0, 0, 1, 0, 0)", "Transform2D()"},
            {"Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0)", "Transform3D()"}
        };

    public static string CorrectDefaultValue(string value, string type_name)
    {
        if (_defaultValueCorrection.TryGetValue(value, out var corrected)) return corrected;
        if (value == "") return $"{type_name}()";
        if (value.StartsWith("Array[")) return "{}";
        return value;
    }

    private static string GetTypeFromParameter(string type_name, string meta = null)
    {
        if (type_name == "void") return "Variant ";

        if (type_name.IsBuiltinType() && type_name != "Nil " || type_name.IsEnum()) return $"{CorrectType(type_name, meta)} ";

        if (type_name.IsVariant() || type_name.IsRefCounted()) return $"in {CorrectType(type_name)} ";

        return $"{CorrectType(type_name)} ";
    }

    private static readonly Dictionary<string, string> _typeConversion =
        new() {{"float", "double"}, {"int", "int64_t"}, {"Nil", "Variant"}};

    private static string CorrectType(string typeName, string meta = null)
    {
        if (meta != null)
        {
            if (meta.Contains("int"))
            {
                return $"{meta}_t";
            }

            if (_typeConversion.TryGetValue(meta, out var value))
            {
                return value;
            }

            return meta;
        }

        if (_typeConversion.TryGetValue(typeName, out var type))
        {
            return type;
        }

        if (typeName.StartsWith("typedarray::"))
        {
            return typeName.Replace("typedarray::", "TypedArray<") + ">";
        }

        if (typeName.IsEnum())
        {
            string baseClass;
            if (typeName.IsBitField())
            {
                baseClass = GetEnumClass(typeName);
                if (baseClass == "GlobalConstants")
                {
                    return $"BitField<{GetEnumName(typeName)}>";
                }

                return $"BitField<{baseClass}::{GetEnumName(typeName)}>";
            }

            baseClass = GetEnumClass(typeName);
            if (baseClass == "GlobalConstants")
            {
                return $"{GetEnumName(typeName)}";
            }

            return $"{baseClass}::{GetEnumName(typeName)}";
        }

        if (typeName.IsRefCounted())
        {
            return $"Ref<{typeName}>";
        }

        if (typeName == "Object" || typeName.IsEngineClass())
        {
            return $"{typeName} *";
        }

        if (typeName.EndsWith("*"))
        {
            return $"{typeName[..^1]} *";
        }

        return typeName;
    }

    private static bool IsBitField(this string typeName) => typeName.StartsWith("bitfield::");

    private static bool IsVariant(this string typeName) => typeName == "Variant" ||
                                                           _builtinClass.Contains(typeName) ||
                                                           typeName == "Nil" ||
                                                           typeName.StartsWith("typedarray::");

    private static bool IsEngineClass(this string typeName) => typeName == "Object" || _engineClass.ContainsKey(typeName);

    private static bool IsRefCounted(this string typeName) => _engineClass.TryGetValue(typeName, out var isRefCounted) && isRefCounted;

    private static bool IsEnum(this string typeName) => typeName.StartsWith("enum::") || typeName.IsBitField();


    private static readonly char[] _splitChars = {'.'};

#warning Populate
    private static readonly Dictionary<string, bool> _engineClass = new();
    private static readonly HashSet<string> _builtinClass = [];

    private static string GetEnumClass(string enumName)
    {
        if (!enumName.Contains(".")) return "GlobalConstants";

        if (enumName.IsBitField()) return enumName.Replace("bitfield::", string.Empty).Split(_splitChars)[0];

        return enumName.Replace("enum::", string.Empty).Split(_splitChars)[0];
    }

    private static string GetEnumName(string enumName)
    {
        if (enumName.IsBitField()) return enumName.Replace("bitfield::", string.Empty).Split(_splitChars)[^1];

        return enumName.Replace("enum::", string.Empty).Split(_splitChars)[^1];
    }

    private static string VarArgReplaceKey = "<VAR_ARG>";

    private static string MakeSignature(
        string className,
        BuiltinClassMethod functionData,
        bool forHeader = false,
        bool useTemplateGetNode = true,
        bool forBuiltin = false,
        bool @static = false)
    {
        bool isVararg = functionData.IsVararg;

        _stringBuilder.Append("public ");

        if (functionData.IsVirtual)
        {
            _stringBuilder.Append("virtual ");
        }

        if (functionData.IsVirtual)
        {
            _stringBuilder.Append("virtual ");
        }

        if (@static)
        {
            _stringBuilder.Append("static ");
        }

        _stringBuilder.Append("unsafe ");

        string returnType = "void";
        string returnMeta = null;
        if (!string.IsNullOrWhiteSpace(functionData.ReturnType))
        {
            returnType = CorrectType(functionData.ReturnType);
        }
        else if (functionData.ReturnValue != null)
        {
            returnType = functionData.ReturnValue.Type;
            returnMeta = string.IsNullOrWhiteSpace(functionData.ReturnValue.Meta) ? null : functionData.ReturnValue.Meta;
        }

        _stringBuilder.Append(CorrectType(returnType, returnMeta));
        if (_stringBuilder[^1] != '*')
        {
            _stringBuilder.Append(' ');
        }

        _stringBuilder.Append(functionData.Name.EscapeContextualKeyWord().SnakeCaseToPascalCase());
        if (!forBuiltin && useTemplateGetNode && className == "Node" && functionData.Name == "get_node")
        {
            _stringBuilder.Append("_Internal");
        }

        _stringBuilder.Append("(");
        string functionParameters = MakeFunctionParameters(functionData.Arguments, forHeader, forBuiltin);
        if (!isVararg)
        {
            _stringBuilder.Append(functionParameters);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(functionParameters))
            {
                _stringBuilder
                    .Append(functionParameters);
            }

            _stringBuilder
                .Append(VarArgReplaceKey);
        }

        _stringBuilder.Append(')');
        if (functionData.IsStatic && forHeader)
        {
            _stringBuilder.Insert(0, "static ");
        }
        else if (functionData.IsConst)
        {
            // _stringBuilder.Append(" const");
        }

        return _stringBuilder.ToStringAndClear();
    }
}
