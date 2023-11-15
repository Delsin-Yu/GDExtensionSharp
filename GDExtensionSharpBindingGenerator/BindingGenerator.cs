using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharpBindingGenerator;

public static partial class BindingGenerator
{
    [GeneratedRegex(@"typedef enum {\n?(?<EnumBody>.+?)\n?} (?<EnumName>.+?);", RegexOptions.Singleline)]
    private static partial Regex GetEnumGlobalRegex();

    [GeneratedRegex(@"(?:(?:\/\*)? ?(?<TitleComment>[\w ]*) ?(?:\*\/))?\n?\t?(?<EnumName>[a-zA-Z_0-9]+)(?: = (?<EnumValue>[0-9a-zA-Z_]+))?,?(?: \/\/(?<EnumComment>[\w .]*)\n)?", RegexOptions.Singleline)]
    private static partial Regex GetEnumBodyRegex();

    [GeneratedRegex(@"(?<DelegateComment>\/\*\*?\n.+?\*\/)?\n?\t?(?:typedef (?:const)? ?(?<ReturnType>[a-zA-z0-9\\_]*?) (?<PointerInfo>\*)?\(?\*(?<DelegateName>[a-zA-Z0-9]+)\)?\(?(?<DelegateParamters>[a-zA-Z *0-9_,]*)\)?;)(?<DelegateTrailingComment> ?\/\/ ?[\w .]+)?", RegexOptions.Singleline)]
    private static partial Regex GetDelegateGlobalRegex();

    [GeneratedRegex(@"\@name (?<MethodName>.*)")] private static partial Regex GetDelegateNameDocumentationCommentRegex();

    [GeneratedRegex(@"\@since (?<SinceComment>.*)")] private static partial Regex GetDelegateSinceDocumentationCommentRegex();

    [GeneratedRegex(@"\@param (?<ParamName>.*?) (?<ParamComment>.*)")] private static partial Regex GetDelegateParamDocumentationCommentRegex();

    [GeneratedRegex(@"\@return (?<ReturnComment>.*)")] private static partial Regex GetDelegateReturnDocumentationCommentRegex();

    [GeneratedRegex(@"\@see (?<SeeComment>.*)")] private static partial Regex GetDelegateSeeDocumentationCommentRegex();

    [GeneratedRegex(@"^(?!@)(?<MethodComment>.*)")] private static partial Regex GetDelegateMethodCommentDocumentationCommentRegex();

    [GeneratedRegex(@"(?:const )?(?<ParameterType>[\w]+) ?(?<PointerInfo>\*)?(?<ParameterName>[\w]+)?", RegexOptions.Singleline)]
    private static partial Regex GetDelegateBodyRegex();

    [GeneratedRegex(@"typedef struct {\n?(?<StructBody>.+?)\n?} (?<StructName>.+?);", RegexOptions.Singleline)]
    private static partial Regex GetStructGlobalRegex();

    [GeneratedRegex(@"(?:\/\* ?(?<FieldHeaderComment>.*?) ?\*\/\n?\t?)?(?:const )?(?<FieldType>[a-zA-Z_0-9]+) ?(?<PointerInfo>\*?)(?<FieldName>[a-zA-Z_0-9]+); ?\/?\/? ?(?<FieldComment>[\w `.();,-]+)?", RegexOptions.Singleline)]
    private static partial Regex GetStructFieldRegex();

    [GeneratedRegex(@"(?:\/\*\*?\n? ?(?<DelegateHeaderComment>[a-zA-Z .]+?) ?\*\/)?\n?\t?(?<ReturnType>[a-zA-z0-9\\_]*?) ?\(?(?<PointerInfo>\*)?(?<DelegateName>[a-zA-Z0-9_]+?)\) ?\((?<DelegateBody>.+?)\)", RegexOptions.Singleline)]
    private static partial Regex GetStructDelegateRegex();

    [GeneratedRegex("(?<=[a-z0-9])([A-Z])|([A-Z][a-z])")] private static partial Regex GetPascalToSnakeRegex();

    private static string PascalCaseToSnakeCase(this string input) =>
        string.IsNullOrEmpty(input) ?
            input :
            GetPascalToSnakeRegex()
               .Replace(input, "_$0")
               .ToLower();

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
            "while",
        };

    public static string Generate(string source)
    {
        var builder = new StringBuilder();

        builder.AppendLine(
            """
            // ReSharper disable RedundantUsingDirective.Global
            // ReSharper disable InconsistentNaming
            // ReSharper disable IdentifierTypo
            // ReSharper disable CommentTypo
            // ReSharper disable InvalidXmlDocComment
            // ReSharper disable RedundantUnsafeContext

            #pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            #pragma warning disable CS0169 // Field is never used

            global using int8_t = System.SByte;
            global using int16_t = System.Int16;
            global using int32_t = System.Int32;
            global using int64_t = System.Int64;
            global using uint8_t = System.Byte;
            global using uint16_t = System.UInt16;
            global using uint32_t = System.UInt32;
            global using uint64_t = System.UInt64;

            global using size_t = System.UInt64;

            global using wchar_t = System.UInt16;
            global using char16_t = System.UInt16;
            global using char32_t = System.UInt32;

            global using GDExtensionInt = System.Int64;
            global using GDExtensionBool = System.Byte;
            global using GDObjectInstanceID = System.UInt64;

            using System.Runtime.InteropServices;


            """
        );

        _indentationLevel = 0;

        foreach (Match match in GetEnumGlobalRegex().Matches(source))
        {
            ProcessEnum(match, builder);
        }

        var delegateBodyDictionary = new Dictionary<string, string>();
        var tempStringBuilder = new StringBuilder();
        {
            var delegateDefinitions = new List<(string delegateName, string delegateBody, string delegateComment, string delegatePseudoCode)>();

            var delegateBodyBuilder = new StringBuilder();
            var delegatePseudoCodeStringBuilder = new StringBuilder();
            var delegateCommentStringBuilder = new StringBuilder();

            foreach (Match match in GetDelegateGlobalRegex().Matches(source))
            {
                PreProcessDelegate(
                    match,
                    delegateBodyBuilder,
                    delegatePseudoCodeStringBuilder,
                    delegateCommentStringBuilder,
                    out var delegateName
                );

                var delegateBody = delegateBodyBuilder.ToString();
                var delegatePseudoCode = delegatePseudoCodeStringBuilder.ToString();
                var delegateComment = delegateCommentStringBuilder.ToString().TrimStart().TrimStart('\n').TrimEnd().TrimEnd('\n');

                delegateDefinitions.Add((delegateName, delegateBody, delegateComment, delegatePseudoCode));
                delegateBodyDictionary.Add(delegateName, delegateBody);

                delegateBodyBuilder.Clear();
                delegatePseudoCodeStringBuilder.Clear();
                delegateCommentStringBuilder.Clear();

                delegateBodyBuilder.Clear();
            }

            for (var i = 0; i < 20; i++)
            {
                foreach (var delegateName in delegateBodyDictionary.Keys)
                {
                    tempStringBuilder.Append(delegateBodyDictionary[delegateName]);
                    foreach (var (substituteParameterName, substituteParameterBody) in delegateBodyDictionary)
                    {
                        tempStringBuilder.Replace(substituteParameterName, substituteParameterBody);
                    }

                    delegateBodyDictionary[delegateName] = tempStringBuilder.ToString();
                    tempStringBuilder.Clear();
                }
            }

            var godotInterfaceDelegates = new List<(string delegateName, string delegateGodotName, string generatedDocumentationComment, string delegateBody, string delegatePseudoCode)>();

            foreach (var (delegateName, delegateBody, delegateComment, delegatePseudoCode) in delegateDefinitions)
            {
                var godotMethodName = GetDelegateNameDocumentationCommentRegex().Match(delegateComment).Groups["MethodName"].Value;
                if (string.IsNullOrWhiteSpace(godotMethodName)) continue;

                var sinceDocumentationComment = GetDelegateSinceDocumentationCommentRegex().Match(delegateComment).Groups["SinceComment"].Value;
                var paramDocumentationComment = GetDelegateParamDocumentationCommentRegex().Matches(delegateComment);
                var returnDocumentationComment = GetDelegateReturnDocumentationCommentRegex().Match(delegateComment).Groups["ReturnComment"].Value;
                var seeDocumentationComment = GetDelegateSeeDocumentationCommentRegex().Match(delegateComment).Groups["SeeComment"].Value;
                var methodCommentDocumentationComment = GetDelegateMethodCommentDocumentationCommentRegex().Match(delegateComment).Groups["MethodComment"].Value;

                tempStringBuilder
                   .AppendLine("/// <summary>");

                if (!string.IsNullOrWhiteSpace(methodCommentDocumentationComment))
                {
                    tempStringBuilder
                       .Append("/// ")
                       .AppendLine(methodCommentDocumentationComment);
                }

                if (!string.IsNullOrWhiteSpace(sinceDocumentationComment))
                {
                    tempStringBuilder
                       .Append("/// Since: ")
                       .AppendLine(sinceDocumentationComment);
                }

                if (!string.IsNullOrWhiteSpace(seeDocumentationComment))
                {
                    tempStringBuilder
                       .Append("/// See: ")
                       .AppendLine(seeDocumentationComment);
                }

                foreach (Match paramMatch in paramDocumentationComment)
                {
                    var paramName = paramMatch.Groups["ParamName"].Value;
                    var paramComment = paramMatch.Groups["ParamComment"].Value;

                    tempStringBuilder
                       .Append("/// <param name=\"")
                       .Append(paramName)
                       .Append('>')
                       .Append(paramComment)
                       .AppendLine("</param>");
                }

                if (!string.IsNullOrWhiteSpace(returnDocumentationComment))
                {
                    tempStringBuilder
                       .Append("/// <returns>")
                       .AppendLine(returnDocumentationComment)
                       .AppendLine("</returns>");
                }

                godotInterfaceDelegates.Add((delegateName, godotMethodName, tempStringBuilder.ToString(), delegateBody, delegatePseudoCode));

                tempStringBuilder.Clear();
            }

            const string methodTableName = "MethodTable";
            
            builder
               .AppendLine(
                    $$"""
                    internal unsafe struct {{methodTableName}}
                    {

                    """
                );

            _indentationLevel++;

            const string getDelegateCallbackName = "p_get_proc_address";
            const string utilityFunctionName = "GetMethod";
            const string utilityFunction =
                $$"""
                  private static void* {{utilityFunctionName}}(string methodName, delegate* unmanaged <char*, delegate* unmanaged <void>> {{getDelegateCallbackName}})
                  {
                      var bstrString = Marshal.StringToBSTR(methodName);
                      var methodPointer = p_get_proc_address((char*)bstrString);
                      Marshal.FreeBSTR(bstrString);
                      return methodPointer;
                  }
                  """;

            foreach (var utilityFunctionLine in utilityFunction.Split('\n'))
            {
                builder
                   .AppendIndentation()
                   .Append(utilityFunctionLine);
            }

            builder
               .AppendLine()
               .AppendLine()
               .AppendIndentation()
               .AppendLine($"internal {methodTableName}(delegate* unmanaged <char*, delegate* unmanaged <void>> {getDelegateCallbackName})")
               .AppendIndentation()
               .AppendLine("{");


            _indentationLevel++;
            foreach (var (delegateName, delegateGodotName, generatedDocumentationComment, delegateBody, delegatePseudoCode) in godotInterfaceDelegates)
            {
                ProcessDelegateInitialization(builder, utilityFunctionName, getDelegateCallbackName, delegateGodotName, delegateName, delegateBody);
            }
            _indentationLevel--;

            builder
               .AppendIndentation()
               .AppendLine("}")
               .AppendLine();

            foreach (var (delegateName, delegateGodotName, generatedDocumentationComment, delegateBody, delegatePseudoCode) in godotInterfaceDelegates)
            {
                ProcessDelegateDeclaration(
                    builder,
                    tempStringBuilder,
                    delegateName,
                    delegateBody,
                    delegateBodyDictionary
                );
                tempStringBuilder.Clear();
            }

            _indentationLevel--;

            builder
               .AppendIndentation()
               .AppendLine("}")
               .AppendLine();
        }


        {
            var tempStringBuilder2 = new StringBuilder();

            foreach (Match match in GetStructGlobalRegex().Matches(source))
            {
                ProcessStruct(match, builder, tempStringBuilder, tempStringBuilder2, delegateBodyDictionary);
                tempStringBuilder.Clear();
            }
        }

        return builder.ToString();
    }

    private static void ProcessEnum(Match match, StringBuilder stringBuilder)
    {
        var enumBody = match.Groups["EnumBody"].Value;
        var enumName = match.Groups["EnumName"].Value;


        stringBuilder.AppendIndentation();
        stringBuilder.AppendLine(
            $$"""
              internal enum {{enumName}}
              {
              """
        );

        _indentationLevel++;

        foreach (Match bodyMatch in GetEnumBodyRegex().Matches(enumBody))
        {
            var titleComment = bodyMatch.Groups["TitleComment"].Value;
            var enumValueName = bodyMatch.Groups["EnumName"].Value;
            var enumValue = bodyMatch.Groups["EnumValue"].Value;
            var enumComment = bodyMatch.Groups["EnumComment"].Value;

            if (!string.IsNullOrWhiteSpace(titleComment))
            {
                stringBuilder
                   .AppendLine()
                   .AppendIndentation()
                   .AppendLine($"/* {titleComment.TrimStart().TrimEnd()} */");
            }

            if (!string.IsNullOrWhiteSpace(enumComment))
            {
                stringBuilder
                   .AppendLine()
                   .AppendIndentation()
                   .AppendLine("/// <summary>")
                   .AppendIndentation()
                   .Append("/// ")
                   .Append(enumComment)
                   .AppendLine()
                   .AppendIndentation()
                   .AppendLine("/// </summary>");
            }

            stringBuilder
               .AppendIndentation()
               .Append(enumValueName.EscapeContextualKeyWord());

            if (!string.IsNullOrWhiteSpace(enumValue))
            {
                stringBuilder
                   .Append(" = ")
                   .Append(enumValue);
            }

            stringBuilder
               .Append(',')
               .AppendLine();
        }

        _indentationLevel--;

        stringBuilder
           .AppendIndentation()
           .AppendLine("}")
           .AppendLine();
    }

    private static void PreProcessDelegate
        (
            Match match,
            StringBuilder delegateBodyBuilder,
            StringBuilder delegatePseudoCodeStringBuilder,
            StringBuilder delegateCommentStringBuilder,
            out string delegateName
        )
    {
        var delegateComment = match.Groups["DelegateComment"].Value;
        if (!string.IsNullOrWhiteSpace(delegateComment))
        {
            delegateCommentStringBuilder.AppendLine(delegateComment);
        }

        delegateCommentStringBuilder.Append(match.Groups["DelegateTrailingComment"].Value);
        delegateCommentStringBuilder.Replace("*", "");
        delegateCommentStringBuilder.Replace("/", "");

        delegateName = match.Groups["DelegateName"].Value;

        var returnType = match.Groups["ReturnType"].Value;
        var delegateParameters = match.Groups["DelegateParamters"].Value;
        var delegatePointerInfo = match.Groups["PointerInfo"].Value;

        BuildDelegate(delegateBodyBuilder, delegatePseudoCodeStringBuilder, delegateName, returnType, delegatePointerInfo, delegateParameters);
    }

    private static void BuildDelegate(StringBuilder delegateBodyBuilder, StringBuilder delegatePseudoCodeStringBuilder, string delegateName, string returnType, string delegatePointerInfo, string delegateParameters)
    {
        delegatePseudoCodeStringBuilder
           .Append("delegate ")
           .Append(returnType)
           .Append(delegatePointerInfo)
           .Append(' ')
           .Append(delegateName.EscapeContextualKeyWord())
           .Append('(');


        delegateBodyBuilder.Append("delegate* unmanaged[Cdecl]<");

        var matchCollection = GetDelegateBodyRegex().Matches(delegateParameters);
        foreach (Match bodyMatch in matchCollection)
        {
            var parameterType = bodyMatch.Groups["ParameterType"].Value;
            var parameterPointerInfo = bodyMatch.Groups["PointerInfo"].Value;
            var parameterName = bodyMatch.Groups["ParameterName"].Value;

            delegateBodyBuilder
               .Append(parameterType)
               .Append(parameterPointerInfo)
               .Append(", ");

            delegatePseudoCodeStringBuilder
               .Append(parameterType)
               .Append(parameterPointerInfo)
               .Append(' ')
               .Append(string.IsNullOrWhiteSpace(parameterName) ? parameterType.PascalCaseToSnakeCase() : parameterName)
               .Append(", ");
        }

        delegateBodyBuilder
           .Append(returnType)
           .Append(delegatePointerInfo);

        delegateBodyBuilder.Append('>');

        delegatePseudoCodeStringBuilder
           .Append(')')
           .Replace(", )", ")");
    }

    private static readonly char[] _trimChars =
    {
        ' ',
        '*'
    };

    private static void ProcessDelegateInitialization(StringBuilder stringBuilder, string helperMethodName, string getDelegateCallbackName, string godotMethodName, string delegateName, string delegateBody)
    {
        stringBuilder
           .AppendIndentation()
           .Append(delegateName)
           .Append(" = (")
           .Append(delegateBody)
           .Append(')')
           .Append(helperMethodName)
           .Append("(\"")
           .Append(godotMethodName)
           .Append("\", ")
           .Append(getDelegateCallbackName)
           .AppendLine(");");
    }

    private static void ProcessDelegateDeclaration
        (
            StringBuilder stringBuilder,
            StringBuilder tempStringBuilder,
            string delegateName,
            string delegateBody,
            IDictionary<string, string> delegateBodyDictionary
        )
    {
        tempStringBuilder.Append(delegateBody);

        ReplaceSubstitutedType(tempStringBuilder, delegateBodyDictionary);

        stringBuilder
           .AppendIndentation()
           .Append("internal ")
           .Append(tempStringBuilder)
           .Append(' ')
           .Append(delegateName.EscapeContextualKeyWord())
           .AppendLine(";")
           .AppendLine();
    }

    private static void ProcessStruct
        (
            Match match,
            StringBuilder stringBuilder,
            StringBuilder tempStringBuilder,
            StringBuilder tempStringBuilder2,
            IDictionary<string, string> delegateBodyDictionary
        )
    {
        var structBody = match.Groups["StructBody"].Value;
        var structName = match.Groups["StructName"].Value;

        stringBuilder
           .AppendIndentation()
           .AppendLine(
                $$"""
                  [StructLayout(LayoutKind.Sequential)]
                  internal unsafe struct {{structName}}
                  {
                  """
            );

        _indentationLevel++;

        foreach (Match bodyMatch in GetStructFieldRegex().Matches(structBody))
        {
            var fieldHeaderComment = bodyMatch.Groups["FieldHeaderComment"].Value;
            var fieldType = bodyMatch.Groups["FieldType"].Value;
            var pointerInfo = bodyMatch.Groups["PointerInfo"].Value;
            var fieldName = bodyMatch.Groups["FieldName"].Value;
            var fieldComment = bodyMatch.Groups["FieldComment"].Value;

            PrintHeaderContent(fieldHeaderComment);

            tempStringBuilder.Append(fieldType);

            ReplaceSubstitutedType(tempStringBuilder, delegateBodyDictionary);

            stringBuilder
               .AppendIndentation()
               .Append("internal ")
               .Append(tempStringBuilder)
               .Append(pointerInfo)
               .Append(' ')
               .Append(fieldName.EscapeContextualKeyWord())
               .Append(';');

            tempStringBuilder.Clear();

            if (!string.IsNullOrWhiteSpace(fieldComment))
            {
                stringBuilder
                   .Append(" // ")
                   .Append(fieldComment);
            }

            stringBuilder
               .AppendLine()
               .AppendLine();
        }


        foreach (Match bodyMatch in GetStructDelegateRegex().Matches(structBody))
        {
            var delegateHeaderComment = bodyMatch.Groups["DelegateHeaderComment"].Value;
            var returnType = bodyMatch.Groups["ReturnType"].Value;
            var pointerInfo = bodyMatch.Groups["PointerInfo"].Value;
            var delegateName = bodyMatch.Groups["DelegateName"].Value;
            var delegateBody = bodyMatch.Groups["DelegateBody"].Value;

            BuildDelegate(tempStringBuilder, tempStringBuilder2, delegateName, returnType, pointerInfo, delegateBody);

            PrintHeaderContent(delegateHeaderComment, tempStringBuilder2.ToString());

            stringBuilder
               .AppendIndentation()
               .Append("internal ")
               .Append(tempStringBuilder)
               .Append(' ')
               .Append(delegateName.EscapeContextualKeyWord())
               .Append(';')
               .AppendLine()
               .AppendLine();

            tempStringBuilder.Clear();
            tempStringBuilder2.Clear();
        }

        _indentationLevel--;

        stringBuilder
           .AppendIndentation()
           .AppendLine("}")
           .AppendLine();

        void PrintHeaderContent(string fieldHeaderComment, string additionalContent = null)
        {
            if (string.IsNullOrWhiteSpace(fieldHeaderComment) && string.IsNullOrWhiteSpace(additionalContent)) return;

            stringBuilder
               .AppendIndentation()
               .AppendLine("/// <summary>");

            if (!string.IsNullOrWhiteSpace(fieldHeaderComment))
            {
                foreach (var commentSegment in fieldHeaderComment.TrimEnd('\n').TrimEnd().Split('\n'))
                {
                    stringBuilder
                       .AppendIndentation()
                       .Append("/// ")
                       .Append(commentSegment.TrimStart(_trimChars))
                       .AppendLine("<br/>");
                }
            }

            if (!string.IsNullOrWhiteSpace(additionalContent))
            {
                if (!string.IsNullOrWhiteSpace(fieldHeaderComment))
                {
                    stringBuilder
                       .AppendIndentation()
                       .Append("/// ")
                       .AppendLine("<br/>");
                }

                stringBuilder
                   .AppendIndentation()
                   .Append("/// ")
                   .AppendLine(additionalContent.TrimStart(_trimChars));
            }

            stringBuilder
               .AppendIndentation()
               .AppendLine("/// </summary>");
        }
    }

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
}
