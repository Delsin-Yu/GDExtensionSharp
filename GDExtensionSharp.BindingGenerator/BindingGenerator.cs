using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionCSharp.BindingGenerator;

public static partial class BindingGenerator
{
    public static string Generate(string source)
    {
        var builder = new StringBuilder();

        builder.AppendLine(FileHeader);

        _indentationLevel = 0;

        foreach (Match match in GetEnumGlobalRegex().Matches(source)) ProcessEnum(match, builder);

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
                    tempStringBuilder
                       .Append("/// ")
                       .AppendLine(methodCommentDocumentationComment);

                if (!string.IsNullOrWhiteSpace(sinceDocumentationComment))
                    tempStringBuilder
                       .Append("/// Since: ")
                       .AppendLine(sinceDocumentationComment);

                if (!string.IsNullOrWhiteSpace(seeDocumentationComment))
                    tempStringBuilder
                       .Append("/// See: ")
                       .AppendLine(seeDocumentationComment);

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

                if (!string.IsNullOrWhiteSpace(returnDocumentationComment)) tempStringBuilder.AppendFormat(DocumentationCommentReturn, returnDocumentationComment);

                godotInterfaceDelegates.Add((delegateName, godotMethodName, tempStringBuilder.ToString(), delegateBody, delegatePseudoCode));

                tempStringBuilder.Clear();
            }

            builder.AppendLine(MethodTableStructHeader);

            _indentationLevel++;

            foreach (var utilityFunctionLine in UtilityFunctionBody.Split('\n'))
            {
                builder
                   .AppendIndentation()
                   .Append(utilityFunctionLine);
            }

            builder
               .AppendLine()
               .AppendLine()
               .AppendIndentation()
               .AppendFormat(MethodTableStructConstructorHeader, MethodTableStructName, GetDelegateCallbackName)
               .AppendIndentation()
               .AppendLine("{");


            _indentationLevel++;

            builder
               .AppendIndentation()
               .AppendLine("// ReSharper disable StringLiteralTypo");

            foreach (var (delegateName, delegateGodotName, generatedDocumentationComment, delegateBody, delegatePseudoCode) in godotInterfaceDelegates)
            {
                ProcessDelegateInitialization(
                    builder,
                    tempStringBuilder,
                    UtilityFunctionName,
                    GetDelegateCallbackName,
                    delegateGodotName,
                    delegateName,
                    delegateBody,
                    delegateBodyDictionary
                );
                tempStringBuilder.Clear();
            }

            builder
               .AppendIndentation()
               .AppendLine("// ReSharper restore StringLiteralTypo");

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
}
