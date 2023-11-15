using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionCSharp.BindingGenerator;

public static partial class BindingGenerator
{
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
        if (!string.IsNullOrWhiteSpace(delegateComment)) delegateCommentStringBuilder.AppendLine(delegateComment);

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

    private static void ProcessDelegateInitialization
        (
            StringBuilder stringBuilder,
            StringBuilder tempStringBuilder,
            string helperMethodName,
            string getDelegateCallbackName,
            string godotMethodName,
            string delegateName,
            string delegateBody,
            Dictionary<string, string> delegateBodyDictionary
        )
    {
        tempStringBuilder.Append(delegateBody);
        ReplaceSubstitutedType(tempStringBuilder, delegateBodyDictionary);

        stringBuilder
           .AppendIndentation()
           .Append(delegateName)
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
}
