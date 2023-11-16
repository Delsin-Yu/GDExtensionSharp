using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionCSharp.BindingGenerator;

public static partial class BindingGenerator
{
    private static int _indentationLevel;

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

    private static readonly char[] _trimChars =
    {
        ' ',
        '*',
        '\n',
        '\r'
    };

    [GeneratedRegex(@"typedef enum {\n?(?<EnumBody>.+?)\n?} (?<EnumName>.+?);", RegexOptions.Singleline)]
    private static partial Regex GetEnumGlobalRegex();

    [GeneratedRegex(@"(?:(?:\/\*)? ?(?<TitleComment>[\w ]*) ?(?:\*\/))?\n?\t?(?<EnumName>[a-zA-Z_0-9]+)(?: = (?<EnumValue>[0-9a-zA-Z_]+))?,?(?: \/\/(?<EnumComment>[\w .]*)\n)?", RegexOptions.Singleline)]
    private static partial Regex GetEnumBodyRegex();

    [GeneratedRegex(@"(?<DelegateComment>\/\*\*?\n.+?\*\/)?\n?\t?(?:typedef (?:const)? ?(?<ReturnType>[a-zA-z0-9\\_]*?) (?<PointerInfo>\*)?\(?\*(?<DelegateName>[a-zA-Z0-9]+)\)?\(?(?<DelegateParamters>[a-zA-Z *0-9_,]*)\)?;)(?<DelegateTrailingComment> ?\/\/ ?[\w .]+)?", RegexOptions.Singleline)]
    private static partial Regex GetDelegateGlobalRegex();

    [GeneratedRegex(@"\@name (?<MethodName>.*)")]
    private static partial Regex GetDelegateNameDocumentationCommentRegex();

    [GeneratedRegex(@"\@since (?<SinceComment>.*)")]
    private static partial Regex GetDelegateSinceDocumentationCommentRegex();

    [GeneratedRegex(@"\@param (?<ParamName>.*?) (?<ParamComment>.*)")]
    private static partial Regex GetDelegateParamDocumentationCommentRegex();

    [GeneratedRegex(@"\@return (?<ReturnComment>.*)")]
    private static partial Regex GetDelegateReturnDocumentationCommentRegex();

    [GeneratedRegex(@"\@see (?<SeeComment>.*)")]
    private static partial Regex GetDelegateSeeDocumentationCommentRegex();

    [GeneratedRegex(@"^(?!@)(?<MethodComment>.*)")]
    private static partial Regex GetDelegateMethodCommentDocumentationCommentRegex();

    [GeneratedRegex(@"(?:const )?(?<ParameterType>[\w]+) ?(?<PointerInfo>\*)?(?<ParameterName>[\w]+)?", RegexOptions.Singleline)]
    private static partial Regex GetDelegateBodyRegex();

    [GeneratedRegex(@"typedef struct {\n?(?<StructBody>.+?)\n?} (?<StructName>.+?);", RegexOptions.Singleline)]
    private static partial Regex GetStructGlobalRegex();

    [GeneratedRegex(@"(?:\/\* ?(?<FieldHeaderComment>.*?) ?\*\/\n?\t?)?(?:const )?(?<FieldType>[a-zA-Z_0-9]+) ?(?<PointerInfo>\*?)(?<FieldName>[a-zA-Z_0-9]+); ?\/?\/? ?(?<FieldComment>[\w `.();,-]+)?", RegexOptions.Singleline)]
    private static partial Regex GetStructFieldRegex();

    [GeneratedRegex(@"(?:\/\*\*?\n? ?(?<DelegateHeaderComment>[a-zA-Z .]+?) ?\*\/)?\n?\t?(?<ReturnType>[a-zA-z0-9\\_]*?) ?\(?(?<PointerInfo>\*)?(?<DelegateName>[a-zA-Z0-9_]+?)\) ?\((?<DelegateBody>.+?)\)", RegexOptions.Singleline)]
    private static partial Regex GetStructDelegateRegex();

    [GeneratedRegex("(?<=[a-z0-9])([A-Z])|([A-Z][a-z])")]
    private static partial Regex GetPascalToSnakeRegex();

    private static string PascalCaseToSnakeCase(this string input) =>
        string.IsNullOrEmpty(input) ?
            input :
            GetPascalToSnakeRegex()
               .Replace(input, "_$0")
               .ToLower();

    private static string EscapeContextualKeyWord(this string name) =>
        _contextualKeywordList.Contains(name) ? $"@{name}" : name;

    private static void ReplaceSubstitutedType(StringBuilder tempStringBuilder, IDictionary<string, string> delegateBodyDictionary)
    {
        var matchKey = tempStringBuilder.ToString();
        if (delegateBodyDictionary.TryGetValue(matchKey, out var matchedSubstitute))
        {
            tempStringBuilder.Replace(matchKey, matchedSubstitute);
            return;
        }

        foreach (var (substituteParameterName, substituteParameterBody) in delegateBodyDictionary)
        {
            tempStringBuilder.Replace(substituteParameterName, substituteParameterBody);
        }
    }

    private static StringBuilder AppendIndentation(this StringBuilder stringBuilder)
    {
        for (var i = 0; i < _indentationLevel; i++)
        {
            stringBuilder.Append(' ', 4);
        }

        return stringBuilder;
    }

    private static string EscapeType(this string type) => 
        type == "char" ? "byte" : type;
}
