using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string ReturnVariableName = "ret";

    private class ReturnValueHandler(string returnArg, string interopType, string returnType)
    {
        public string ReturnArg { get; } = returnArg;
        public string InteropType { get; } = interopType;
        public string ReturnType { get; } = returnType;
    }

    private static readonly Dictionary<string, ReturnValueHandler> _handler_returnValue = new()
    {
        { "StringName", new("StringName.CreateTakingOwnershipOfDisposableValue({0})", "godot_string_name", "StringName") },
        { "PackedStringArray", new("Marshaling.ConvertNativePackedStringArrayToSystemArray({0})", "using godot_packed_string_array", "string[]") },
        { "String", new("Marshaling.ConvertStringToManaged({0})", "using godot_string", "string") },
        { "Dictionary", new("Godot.Collections.Dictionary.CreateTakingOwnershipOfDisposableValue({0})", "godot_dictionary", "Godot.Collections.Dictionary") },
        { "Array", new("Godot.Collections.Array.CreateTakingOwnershipOfDisposableValue({0})", "godot_array", "Godot.Collections.Array") },
    };

    private class ParameterValueHandler(string nativeArg, string parameterType)
    {
        public string NativeArg { get; } = nativeArg;
        public string ParameterType { get; } = parameterType;
    }

    private static readonly Dictionary<string, ParameterValueHandler> _handler_parameterValue = new()
    {
        { "StringName", new("godot_string_name {0} = (godot_string_name)({1}?.NativeValue ?? default);", "StringName") },
        { "String", new("using godot_string {0} = Marshaling.ConvertStringToNative({1});", "string") },
        { "PackedStringArray", new("using godot_packed_string_array {0} = Marshaling.ConvertSystemArrayToNativePackedStringArray({1});", "string[]") },
        { "PackedFloat32Array", new("using godot_packed_float32_array {0} = Marshaling.ConvertSystemArrayToNativePackedFloat32Array({1});", "float[]") },
        { "Variant", new("godot_variant {0} = (godot_variant){1}.NativeVar;", "Variant") },
        { "Array", new("godot_array {0} = (godot_array)({1} ?? new()).NativeValue;", "Godot.Collections.Array") },
    };

    private static void BuildMethodReturnCall(StringBuilder stringBuilder, string returnType, bool isGodotType)
    {
        if (returnType == "void") return;

        string returnArg;

        if (isGodotType)
        {
            returnArg = $"({returnType})InteropUtils.UnmanagedGetManaged({{0}})";
        }
        else if (_handler_returnValue.TryGetValue(returnType, out var handler))
        {
            returnArg = handler.ReturnArg;
        }
        else
        {
            returnArg = "{0}";
        }

        stringBuilder
            .AppendIndent("return ")
            .AppendFormat(returnArg, ReturnVariableName)
            .AppendLine(";");
    }

    private static void BuildMethodBodyReturnInit(StringBuilder stringBuilder, string returnType, bool isGodotType)
    {
        if (returnType == "void") return;


        if (isGodotType)
        {
            stringBuilder.AppendIndent("nint");
        }
        else if (_handler_returnValue.TryGetValue(returnType, out var handler))
        {
            stringBuilder.AppendIndent(handler.InteropType);
        }
        else
        {
            stringBuilder.AppendIndent(returnType);
        }

        stringBuilder.AppendLine($" {ReturnVariableName} = default;");
    }

    private static void BuildMethodHeaderReturn(StringBuilder stringBuilder, ReturnValue returnValue, out string returnTypeName)
    {
        if (returnValue == null)
        {
            stringBuilder.Append("void");
            returnTypeName = "void";
            return;
        }

        returnTypeName = CorrectType(returnValue.Type);

        if (_handler_returnValue.TryGetValue(returnTypeName, out var handler))
        {
            stringBuilder.Append(handler.ReturnType);
        }
        else
        {
            stringBuilder.Append(returnTypeName);
        }
    }

    private static void BuildMethodBodyArguments(StringBuilder stringBuilder, string methodBindName, string methodBindDebugName, string returnType, IReadOnlyList<Argument> arguments, ICollection<string> godotTypes)
    {
        stringBuilder.AppendIndentLine($"if ({methodBindName} == IntPtr.Zero) throw new ArgumentNullException(nameof({methodBindName}), \"Godot Method \\\"{methodBindDebugName}\\\" is uninitialized!\");");

        if (arguments == null)
        {
            stringBuilder.AppendIndent($"{MethodTableAccess}.object_method_bind_ptrcall((void*){methodBindName}, (void*)GodotObject.GetPtr(this), null, ");
        }
        else
        {
            var argumentList = new (string argumentInit, string argumentName, string argumentUsage)[arguments.Count];

            for (int i = 0; i < arguments.Count; i++)
            {
                Argument classMethodArgument = arguments[i];

                string initArg;
                string argumentUsage;
                string argName = classMethodArgument.Name.EscapeContextualKeyWord();
                string originalArgName = argName;

                string classMethodArgumentType = CorrectType(classMethodArgument.Type);

                if (_handler_parameterValue.TryGetValue(classMethodArgumentType, out var handler))
                {
                    argName = $"native_{classMethodArgument.Name}";
                    initArg = string.Format(handler.NativeArg, argName, originalArgName);
                    argumentUsage = $"&{argName}";
                }
                else if (godotTypes.Contains(classMethodArgumentType))
                {
                    argName = $"native_{classMethodArgument.Name}";
                    initArg = $"nint {argName} = GodotObject.GetPtr({originalArgName});";
                    argumentUsage = $"(void*){argName}";
                }
                else
                {
                    initArg = null;
                    argumentUsage = $"&{argName}";
                }

                argumentList[i] = (initArg, argName, argumentUsage);
            }

            foreach (var (argumentInit, _, _) in argumentList)
            {
                if (argumentInit == null) continue;
                stringBuilder.AppendIndentLine(argumentInit);
            }


            stringBuilder
                .AppendIndent($"void** argList = stackalloc void*[{argumentList.Length.ToString(CultureInfo.InvariantCulture)}]{{ ");

            foreach (var (_, _, argumentUsage) in argumentList)
            {
                stringBuilder
                    .Append(argumentUsage)
                    .Append(", ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);


            stringBuilder
                .AppendLine(" };")
                .AppendIndent($"{MethodTableAccess}.object_method_bind_ptrcall((void*){methodBindName}, (void*)GodotObject.GetPtr(this), argList, ");
        }

        stringBuilder
            .Append(returnType != "void" ? $"&{ReturnVariableName}" : "null")
            .AppendLine(");");
    }

    private static void BuildMethodHeaderArguments(StringBuilder stringBuilder, IReadOnlyList<Argument> arguments)
    {
        if (arguments == null) return;

        foreach (Argument argument in arguments)
        {
            string correctType = CorrectType(argument.Type);

            if (_handler_parameterValue.TryGetValue(correctType, out var handler))
            {
                correctType = handler.ParameterType;
            }

            stringBuilder
                .Append($"{correctType} {argument.Name.EscapeContextualKeyWord()}, ");
        }

        stringBuilder.Remove(stringBuilder.Length - 2, 2);
    }

    private static void GenerateClassMethod(StringBuilder stringBuilder, IReadOnlyList<ClassMethod> engineClassMethods, ICollection<string> godotTypes)
    {
        if (engineClassMethods != null)
        {
            for (int index = 0; index < engineClassMethods.Count; index++)
            {
                ClassMethod classMethod = engineClassMethods[index];

                string methodName = classMethod.Name;
                string methodPascalName = methodName.SnakeCaseToPascalCase();

                bool isGodotType = godotTypes.Contains(methodName);

                string methodBindName = $"MethodBind{index.ToString(CultureInfo.InvariantCulture)}";

                stringBuilder
                    .AppendIndentLine($"""
                                       [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                                       private static readonly IntPtr {methodBindName} = ClassDB_get_method_with_compatibility(NativeName, MethodName.{methodPascalName}, {classMethod.Hash?.ToString(CultureInfo.InvariantCulture)}ul);
                                       """)
                    .AppendLine()
                    .AppendIndent("public unsafe ");

                BuildMethodHeaderReturn(stringBuilder, classMethod.ReturnValue, out string returnTypeName);

                stringBuilder
                    .Append(' ')
                    .Append(methodPascalName)
                    .Append('(');

                BuildMethodHeaderArguments(stringBuilder, classMethod.Arguments);

                stringBuilder
                    .AppendLine(")")
                    .AppendIndentLine("{");

                {
                    using var handle = stringBuilder.AddIndent();

                    BuildMethodBodyReturnInit(stringBuilder, returnTypeName, isGodotType);

                    BuildMethodBodyArguments(stringBuilder, methodBindName, methodName, returnTypeName, classMethod.Arguments, godotTypes);

                    BuildMethodReturnCall(stringBuilder, returnTypeName, isGodotType);
                }


                stringBuilder
                    .AppendIndentLine("}")
                    .AppendLine();
            }
        }
    }
}
