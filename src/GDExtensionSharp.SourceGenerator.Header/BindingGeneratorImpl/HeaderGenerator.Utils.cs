using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Header.RegexParser;

partial class HeaderGenerator
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

    private static readonly Regex _enumGlobalRegex = new(@"typedef enum {\n?(?<EnumBody>.+?)\n?} (?<EnumName>.+?);", RegexOptions.Singleline | RegexOptions.Compiled);
    private static Regex GetEnumGlobalRegex() => _enumGlobalRegex;

    private static readonly Regex _enumBodyRegex = new(@"(?:(?:\/\*)? ?(?<TitleComment>[\w ]*) ?(?:\*\/))?\n?\t?(?<EnumName>[a-zA-Z_0-9]+)(?: = (?<EnumValue>[0-9a-zA-Z_]+))?,?(?: \/\/ ?(?<EnumComment>[\w .]*))?", RegexOptions.Singleline | RegexOptions.Compiled);
    private static Regex GetEnumBodyRegex() => _enumBodyRegex;

    private static readonly Regex _delegateGlobalRegex = new(@"(?<DelegateComment>\/\*\*?\n.+?\*\/)?\n?\t?(?:typedef (?:const)? ?(?<ReturnType>[a-zA-z0-9\\_]*?) (?<PointerInfo>\*)?\(?\*(?<DelegateName>[a-zA-Z0-9]+)\)?\(?(?<DelegateParamters>[a-zA-Z *0-9_,]*)\)?;)(?<DelegateTrailingComment> ?\/\/ ?[\w .]+)?", RegexOptions.Singleline | RegexOptions.Compiled);
    private static Regex GetDelegateGlobalRegex() => _delegateGlobalRegex;

    private static readonly Regex _delegateNameDocumentationCommentRegex = new(@"\@name (?<MethodName>.*)", RegexOptions.Compiled);
    private static Regex GetDelegateNameDocumentationCommentRegex() => _delegateNameDocumentationCommentRegex;

    private static readonly Regex _delegateSinceDocumentationCommentRegex = new(@"\@since (?<SinceComment>.*)", RegexOptions.Compiled);
    private static Regex GetDelegateSinceDocumentationCommentRegex() => _delegateSinceDocumentationCommentRegex;

    private static readonly Regex _delegateParamDocumentationCommentRegex = new(@"\@param (?<ParamName>.*?) (?<ParamComment>.*)", RegexOptions.Compiled);
    private static Regex GetDelegateParamDocumentationCommentRegex() => _delegateParamDocumentationCommentRegex;

    private static readonly Regex _delegateReturnDocumentationCommentRegex = new(@"\@return (?<ReturnComment>.*)", RegexOptions.Compiled);
    private static Regex GetDelegateReturnDocumentationCommentRegex() => _delegateReturnDocumentationCommentRegex;

    private static readonly Regex _delegateSeeDocumentationCommentRegex = new(@"\@see (?<SeeComment>.*)", RegexOptions.Compiled);
    private static Regex GetDelegateSeeDocumentationCommentRegex() => _delegateSeeDocumentationCommentRegex;

    private static readonly Regex _delegateMethodCommentDocumentationCommentRegex = new(@"^(?!@)(?<MethodComment>.*)", RegexOptions.Compiled);
    private static Regex GetDelegateMethodCommentDocumentationCommentRegex() => _delegateMethodCommentDocumentationCommentRegex;

    private static readonly Regex _delegateBodyRegex = new(@"(?:const )?(?<ParameterType>[\w]+) ?(?<PointerInfo>\*)?(?<ParameterName>[\w]+)?", RegexOptions.Singleline | RegexOptions.Compiled);
    private static Regex GetDelegateBodyRegex() => _delegateBodyRegex;

    private static readonly Regex _structGlobalRegex = new(@"typedef struct {\n?(?<StructBody>.+?)\n?} (?<StructName>.+?);", RegexOptions.Singleline | RegexOptions.Compiled);
    private static Regex GetStructGlobalRegex() => _structGlobalRegex;

    private static readonly Regex _structFieldRegex = new(@"(?:\/\* ?(?<FieldHeaderComment>.*?) ?\*\/\n?\t?)?(?:const )?(?<FieldType>[a-zA-Z_0-9]+) ?(?<PointerInfo>\*?)(?<FieldName>[a-zA-Z_0-9]+); ?\/?\/? ?(?<FieldComment>[\w `.();,-]+)?", RegexOptions.Singleline | RegexOptions.Compiled);
    private static Regex GetStructFieldRegex() => _structFieldRegex;

    private static readonly Regex _structDelegateRegex = new(@"(?:\/\*\*?\n? ?(?<DelegateHeaderComment>[a-zA-Z .]+?) ?\*\/)?\n?\t?(?<ReturnType>[a-zA-z0-9\\_]*?) ?(?<PointerInfo>\*)?\(?\*?(?<DelegateName>[a-zA-Z0-9_]+?)\) ?\((?<DelegateBody>.+?)\)", RegexOptions.Singleline | RegexOptions.Compiled);
    private static Regex GetStructDelegateRegex() => _structDelegateRegex;

    private static readonly Regex _pascalToSnakeRegex = new("(?<=[a-z0-9])([A-Z])|([A-Z][a-z])", RegexOptions.Compiled);
    private static Regex GetPascalToSnakeRegex() => _pascalToSnakeRegex;

    private static readonly Regex _standardizeLineEndingsRegex = new(@"\r\n|\n\r|\n|\r", RegexOptions.Compiled);
    private static Regex GetStandardizeLineEndingsRegex() => _standardizeLineEndingsRegex;
    
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

        foreach (var delegateInfo in delegateBodyDictionary)
        {
            tempStringBuilder.Replace(delegateInfo.Key, delegateInfo.Value);
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
