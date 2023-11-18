using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Header.BindingGeneratorImpl;

partial class HeaderGenerator
{
    private static void ProcessStruct
        (
            Match match,
            StringBuilder stringBuilder,
            StringBuilder tempStringBuilder,
            StringBuilder tempStringBuilder2,
            IDictionary<string, string> delegateBodyDictionary,
            out string structName
        )
    {
        string? structBody = match.Groups["StructBody"].Value;
        structName = match.Groups["StructName"].Value;

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
            string? fieldHeaderComment = bodyMatch.Groups["FieldHeaderComment"].Value;
            string? fieldType = bodyMatch.Groups["FieldType"].Value;
            string? pointerInfo = bodyMatch.Groups["PointerInfo"].Value;
            string? fieldName = bodyMatch.Groups["FieldName"].Value;
            string? fieldComment = bodyMatch.Groups["FieldComment"].Value;

            PrintHeaderContent(fieldHeaderComment);

            tempStringBuilder.Append(fieldType.EscapeType());

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
                stringBuilder
                   .Append(" // ")
                   .Append(fieldComment);

            stringBuilder
               .AppendLine()
               .AppendLine();
        }


        foreach (Match bodyMatch in GetStructDelegateRegex().Matches(structBody))
        {
            string? delegateHeaderComment = bodyMatch.Groups["DelegateHeaderComment"].Value;
            string? returnType = bodyMatch.Groups["ReturnType"].Value;
            string? pointerInfo = bodyMatch.Groups["PointerInfo"].Value;
            string? delegateName = bodyMatch.Groups["DelegateName"].Value;
            string? delegateBody = bodyMatch.Groups["DelegateBody"].Value;

            BuildDelegate(tempStringBuilder, tempStringBuilder2, null, returnType, pointerInfo, delegateBody);

            PrintHeaderContent(delegateHeaderComment);

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
                foreach (string commentSegment in fieldHeaderComment.TrimEnd('\n').TrimEnd().Split('\n'))
                {
                    stringBuilder
                       .AppendIndentation()
                       .Append("/// ")
                       .Append(commentSegment.TrimStart(_trimChars))
                       .AppendLine("<br/>");
                }

            if (!string.IsNullOrWhiteSpace(additionalContent))
            {
                if (!string.IsNullOrWhiteSpace(fieldHeaderComment))
                    stringBuilder
                       .AppendIndentation()
                       .Append("/// ")
                       .AppendLine("<br/>");

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
}
