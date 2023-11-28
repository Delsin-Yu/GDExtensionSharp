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
        switch (typeName)
        {
            case "float":
                return "double";
            case "int":
                return "int64_t";
            case "Nil":
                return "Variant";
        }

        if (typeName.StartsWith("typedarray::"))
        {
            return typeName["typedarray::".Length..];
        }

        if (typeName.StartsWith("bitfield::"))
        {
            return typeName["bitfield::".Length..];
        }

        if (typeName.StartsWith("enum::"))
        {
            return typeName["enum::".Length..];
        }

        if (typeName == "RID") return "Rid";

        if ((typeName.Contains("Vector") || typeName.Contains("Rect")) && typeName.Contains("i"))
        {
            return typeName[..^1] + "I";
        }

        if (typeName.EndsWith("*"))
        {
            return typeName[..^1];
        }

        return typeName;
    }
}
