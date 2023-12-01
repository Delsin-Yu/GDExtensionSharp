namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
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

    private static string EscapeContextualKeyWord(this string name) => _contextualKeywordList.Contains(name) ? $"@{name}" : name;

    private static string CorrectType(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName)) return typeName;

        string rawType = typeName;

        bool isTypedArray = typeName.StartsWith("typedarray::");
        bool isBitField = typeName.StartsWith("bitfield::");
        bool isEnum = typeName.StartsWith("enum::");
        bool isPointer = typeName.EndsWith("*");

        if (isTypedArray) rawType = typeName["typedarray::".Length..];
        if (isBitField) rawType = typeName["bitfield::".Length..];
        if (isEnum) rawType = typeName["enum::".Length..];
        if (isPointer) rawType = typeName[..^1];

        rawType = rawType switch
        {
            "float" => "double",
            "int" => "int64_t",
            "Nil" => "Variant",
            "RID" => "Rid",
            "AABB" => "Aabb",
            "Object" => "GodotObject",
            "Vector4i" => "Vector4I",
            "Vector3i" => "Vector3I",
            "Vector2i" => "Vector2I",
            "Rect2i" => "Rect2I",
            _ => rawType
        };

        if (isTypedArray)
        {
            rawType = $"Godot.Collections.Array<{rawType}>";
        }

        return rawType;
    }
}
