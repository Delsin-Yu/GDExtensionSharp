using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Header;

internal static partial class BindingGenerator
{
    internal static IEnumerable<(string sourceName, string sourceContent)> Generate(string source)
    {
        yield return ("GlobalUsing", GlobalUsing);
        
        var builder = new StringBuilder();

        _indentationLevel = 0;

        foreach (Match match in GetEnumGlobalRegex().Matches(source))
        {
            builder.AppendLine(FileHeader);
            ProcessEnum(match, builder, out var enumName);
            yield return ($"Enum.{enumName}", builder.ToString());
            builder.Clear();
        }

        var delegateBodyDictionary = new Dictionary<string, string>();
        var tempStringBuilder = new StringBuilder();
        var tempStringBuilder2 = new StringBuilder();
        
        {
            builder.AppendLine(FileHeader);

            var delegateDefinitions = new List<(string delegateName, string delegateBody, string delegateComment, string delegateMethod, string delegateCallBody)>();

            var delegateBodyBuilder = new StringBuilder();
            var delegateMethodStringBuilder = new StringBuilder();
            var delegateCommentStringBuilder = new StringBuilder();

            foreach (Match match in GetDelegateGlobalRegex().Matches(source))
            {
                PreProcessDelegate(
                    match,
                    delegateBodyBuilder,
                    delegateMethodStringBuilder,
                    delegateCommentStringBuilder,
                    tempStringBuilder,
                    out var delegateName
                );

                var delegateBody = delegateBodyBuilder.ToString();
                var delegateMethod = delegateMethodStringBuilder.ToString();
                var delegateComment = delegateCommentStringBuilder.ToString().TrimStart().TrimStart('\n').TrimEnd().TrimEnd('\n');
                var delegateCallBody = tempStringBuilder.ToString();

                delegateDefinitions.Add((delegateName, delegateBody, delegateComment, delegateMethod, delegateCallBody));
                delegateBodyDictionary.Add(delegateName, delegateBody);

                delegateBodyBuilder.Clear();
                delegateMethodStringBuilder.Clear();
                delegateCommentStringBuilder.Clear();

                delegateBodyBuilder.Clear();

                tempStringBuilder.Clear();
            }

            for (var i = 0; i < 20; i++)
            {
                foreach (var delegateName in delegateBodyDictionary.Keys)
                {
                    tempStringBuilder.Append(delegateBodyDictionary[delegateName]);
                    foreach (var delegateInfo in delegateBodyDictionary)
                    {
                        tempStringBuilder.Replace(delegateInfo.Key, delegateInfo.Value);
                    }

                    delegateBodyDictionary[delegateName] = tempStringBuilder.ToString();
                    tempStringBuilder.Clear();
                }
            }

            var godotInterfaceDelegates = new List<(string delegateName, string delegateGodotName, string generatedDocumentationComment, string delegateBody, string delegateCallBody, string delegatePseudoCode)>();

            foreach (var (delegateName, delegateBody, delegateComment, delegatePseudoCode, delegateCallBody) in delegateDefinitions)
            {
                if (ProcessGodotInterfaceDelegate(
                        delegateComment,
                        tempStringBuilder,
                        out var godotMethodName
                    )) continue;

                godotInterfaceDelegates.Add(
                    (
                        delegateName,
                        godotMethodName,
                        tempStringBuilder.ToString(),
                        delegateBody,
                        delegateCallBody,
                        delegatePseudoCode
                    )
                );

                tempStringBuilder.Clear();
            }

            builder.AppendLine(MethodTableStructHeader);

            _indentationLevel++;

            foreach (var utilityFunctionLine in UtilityFunctionBody.Split('\n'))
            {
                builder
                   .AppendIndentation()
                   .AppendLine(utilityFunctionLine);
            }

            builder
               .AppendLine()
               .AppendIndentation()
               .AppendFormat(MethodTableStructConstructorHeader, MethodTableStructName, GetDelegateCallbackName)
               .AppendLine()
               .AppendIndentation()
               .AppendLine("{");


            _indentationLevel++;

            builder
               .AppendIndentation()
               .AppendLine("// ReSharper disable StringLiteralTypo");

            foreach (var (delegateName, delegateGodotName, generatedDocumentationComment, delegateBody, delegateCallBody, delegateMethod) in godotInterfaceDelegates)
            {
                ProcessDelegateInitialization(
                    builder,
                    tempStringBuilder,
                    UtilityFunctionName,
                    GetDelegateCallbackName,
                    delegateGodotName,
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

            builder
               .AppendLine("#region Delegate Methods")
               .AppendLine();

            foreach (var (delegateName, delegateGodotName, generatedDocumentationComment, delegateBody, delegateCallBody, delegateMethod) in godotInterfaceDelegates)
            {
                ProcessDelegateMethodDeclaration(
                    builder,
                    tempStringBuilder,
                    delegateGodotName,
                    delegateName,
                    delegateMethod,
                    delegateCallBody,
                    generatedDocumentationComment,
                    delegateBodyDictionary
                );
                tempStringBuilder.Clear();
            }

            builder
               .AppendLine("#endregion")
               .AppendLine();

            builder
               .AppendLine("#region Delegates")
               .AppendLine();

            foreach (var (delegateName, delegateGodotName, generatedDocumentationComment, delegateBody, delegateCallBody, delegateMethod) in godotInterfaceDelegates)
            {
                ProcessDelegateDeclaration(
                    builder,
                    tempStringBuilder,
                    delegateGodotName,
                    delegateBody,
                    delegateBodyDictionary
                );
                tempStringBuilder.Clear();
            }

            builder.AppendLine("#endregion");

            _indentationLevel--;

            builder
               .AppendIndentation()
               .AppendLine("}")
               .AppendLine();

            yield return ("MethodTable", builder.ToString());
            builder.Clear();
        }

        foreach (Match match in GetStructGlobalRegex().Matches(source))
        {
            builder.AppendLine(FileHeader);
            ProcessStruct(match, builder, tempStringBuilder, tempStringBuilder2, delegateBodyDictionary, out var structName);
            tempStringBuilder.Clear();
            yield return ($"Struct.{structName}", builder.ToString());
            builder.Clear();
        }
    }
}
