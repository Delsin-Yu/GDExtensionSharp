using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Header.BindingGeneratorImpl;

partial class HeaderGenerator
{
    private static void PreProcessDelegate
        (
            Match match,
            StringBuilder delegateBodyBuilder,
            StringBuilder delegateMethodStringBuilder,
            StringBuilder delegateCommentStringBuilder,
            StringBuilder godotDelegateCallBodyBuilder,
            out string delegateName
        )
    {
        string? delegateComment = match.Groups["DelegateComment"].Value;
        if (!string.IsNullOrWhiteSpace(delegateComment)) delegateCommentStringBuilder.AppendLine(delegateComment);

        delegateCommentStringBuilder.Append(match.Groups["DelegateTrailingComment"].Value);
        delegateCommentStringBuilder.Replace("*", "");
        delegateCommentStringBuilder.Replace("/", "");

        delegateName = match.Groups["DelegateName"].Value;

        string? returnType = match.Groups["ReturnType"].Value;
        string? delegateParameters = match.Groups["DelegateParamters"].Value;
        string? delegatePointerInfo = match.Groups["PointerInfo"].Value;

        BuildDelegate(delegateBodyBuilder, delegateMethodStringBuilder, godotDelegateCallBodyBuilder, returnType, delegatePointerInfo, delegateParameters);
    }

    private static void BuildDelegate
        (
            StringBuilder delegateBodyBuilder,
            StringBuilder delegateMethodStringBuilder,
            StringBuilder delegateCallBodyBuilder,
            string returnType,
            string delegatePointerInfo,
            string delegateParameters
        )
    {
        delegateMethodStringBuilder
           .Append("internal ")
           .Append(returnType.EscapeType())
           .Append(delegatePointerInfo)
           .Append(" <DELEGATE_METHOD_NAME>(");


        delegateBodyBuilder.Append("delegate* unmanaged<");

        var matchCollection = GetDelegateBodyRegex().Matches(delegateParameters);
        foreach (Match bodyMatch in matchCollection)
        {
            string? parameterType = bodyMatch.Groups["ParameterType"].Value;
            string? parameterPointerInfo = bodyMatch.Groups["PointerInfo"].Value;
            string? parameterName = bodyMatch.Groups["ParameterName"].Value;

            delegateBodyBuilder
               .Append(parameterType.EscapeType())
               .Append(parameterPointerInfo)
               .Append(", ");

            delegateMethodStringBuilder
               .Append(parameterType.EscapeType())
               .Append(parameterPointerInfo)
               .Append(' ')
               .Append(string.IsNullOrWhiteSpace(parameterName) ? parameterType.PascalCaseToSnakeCase() : parameterName)
               .Append(", ");

            delegateCallBodyBuilder?
               .Append(parameterName)
               .Append(", ");
        }

        if (delegateCallBodyBuilder?.Length > 0)
        {
            delegateCallBodyBuilder.Remove(delegateCallBodyBuilder.Length - 2, 2);
        }

        delegateBodyBuilder
           .Append(returnType)
           .Append(delegatePointerInfo);

        delegateBodyBuilder.Append('>');

        delegateMethodStringBuilder
           .Append(')')
           .Replace(", )", ")");
    }

    private static void ProcessDelegateInitialization
        (
            StringBuilder stringBuilder,
            StringBuilder tempStringBuilder,
            string helperMethodName,
            string getDelegateCallbackName,
            string godotMethodName,
            string delegateBody,
            Dictionary<string, string> delegateBodyDictionary
        )
    {
        tempStringBuilder.Append(delegateBody);
        ReplaceSubstitutedType(tempStringBuilder, delegateBodyDictionary);

        stringBuilder
           .AppendIndentation()
           .Append('_')
           .Append(godotMethodName)
           .Append(" = (")
           .Append(tempStringBuilder)
           .Append(')')
           .Append(helperMethodName)
           .Append("(\"")
           .Append(godotMethodName)
           .Append("\", ")
           .Append(getDelegateCallbackName)
           .AppendLine(");");
    }

    private static void ProcessDelegateMethodDeclaration
        (
            StringBuilder stringBuilder,
            StringBuilder tempStringBuilder,
            string delegateGodotName,
            string delegateMethodName,
            string delegateMethodBody,
            string delegateDelegateCallBody,
            string delegateMethodGeneratedDocumentationComment,
            IDictionary<string, string> delegateBodyDictionary
        )
    {
        // if (!string.IsNullOrWhiteSpace(delegateMethodGeneratedDocumentationComment))
        // {
        //     foreach (var delegateCommentLine in delegateMethodGeneratedDocumentationComment.TrimEnd('\n').TrimEnd('\r').Split('\n'))
        //     {
        //         stringBuilder
        //            .AppendIndentation()
        //            .AppendLine(delegateCommentLine);
        //     }
        // }
        //

        tempStringBuilder.Append(delegateMethodBody);

        ReplaceSubstitutedType(tempStringBuilder, delegateBodyDictionary);

        tempStringBuilder.Replace("<DELEGATE_METHOD_NAME>", delegateGodotName);

        stringBuilder
           .AppendIndentation()
           .Append(tempStringBuilder)
           .Append(" => _")
           .Append(delegateGodotName)
           .Append('(')
           .Append(delegateDelegateCallBody)
           .AppendLine(");");
    }


    private static void ProcessDelegateDeclaration
        (
            StringBuilder stringBuilder,
            StringBuilder tempStringBuilder,
            string delegateGodotName,
            string delegateBody,
            IDictionary<string, string> delegateBodyDictionary
        )
    {
        tempStringBuilder.Append(delegateBody);

        ReplaceSubstitutedType(tempStringBuilder, delegateBodyDictionary);

        stringBuilder
           .AppendIndentation()
           .Append("private readonly ")
           .Append(tempStringBuilder)
           .Append(" _")
           .Append(delegateGodotName.EscapeContextualKeyWord())
           .AppendLine(";");
    }

    private static bool ProcessGodotInterfaceDelegate
        (
            string delegateComment,
            StringBuilder godotDocumentationCommentBuilder,
            out string godotMethodName
        )
    {
        godotMethodName = GetDelegateNameDocumentationCommentRegex().Match(delegateComment).Groups["MethodName"].Value;
        if (string.IsNullOrWhiteSpace(godotMethodName)) return true;

        string? sinceDocumentationComment = GetDelegateSinceDocumentationCommentRegex().Match(delegateComment).Groups["SinceComment"].Value;
        var paramDocumentationComment = GetDelegateParamDocumentationCommentRegex().Matches(delegateComment);
        string? returnDocumentationComment = GetDelegateReturnDocumentationCommentRegex().Match(delegateComment).Groups["ReturnComment"].Value;
        string? seeDocumentationComment = GetDelegateSeeDocumentationCommentRegex().Match(delegateComment).Groups["SeeComment"].Value;
        string? methodCommentDocumentationComment = GetDelegateMethodCommentDocumentationCommentRegex().Match(delegateComment).Groups["MethodComment"].Value;

        godotDocumentationCommentBuilder
           .AppendLine("/// <summary>");

        if (!string.IsNullOrWhiteSpace(methodCommentDocumentationComment))
            godotDocumentationCommentBuilder
               .Append("/// ")
               .AppendLine(methodCommentDocumentationComment);

        if (!string.IsNullOrWhiteSpace(sinceDocumentationComment))
            godotDocumentationCommentBuilder
               .Append("/// Since: ")
               .AppendLine(sinceDocumentationComment);

        if (!string.IsNullOrWhiteSpace(seeDocumentationComment))
            godotDocumentationCommentBuilder
               .Append("/// See: ")
               .AppendLine(seeDocumentationComment);

        godotDocumentationCommentBuilder
           .AppendLine("/// </summary>");

        foreach (Match paramMatch in paramDocumentationComment)
        {
            string? paramName = paramMatch.Groups["ParamName"].Value;
            string? paramComment = paramMatch.Groups["ParamComment"].Value;

            godotDocumentationCommentBuilder
               .Append("/// <param name=\"")
               .Append(paramName)
               .Append("\">")
               .Append(paramComment)
               .AppendLine("</param>");
        }

        if (!string.IsNullOrWhiteSpace(returnDocumentationComment)) godotDocumentationCommentBuilder.AppendFormat(DocumentationCommentReturn, returnDocumentationComment);
        return false;
    }
}
