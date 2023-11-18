using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.BindingGenerator;

partial class BindingGenerator
{
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
            var delegateHeaderComment = bodyMatch.Groups["DelegateHeaderComment"].Value;
            var returnType = bodyMatch.Groups["ReturnType"].Value;
            var pointerInfo = bodyMatch.Groups["PointerInfo"].Value;
            var delegateName = bodyMatch.Groups["DelegateName"].Value;
            var delegateBody = bodyMatch.Groups["DelegateBody"].Value;

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
                foreach (var commentSegment in fieldHeaderComment.TrimEnd('\n').TrimEnd().Split('\n'))
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
