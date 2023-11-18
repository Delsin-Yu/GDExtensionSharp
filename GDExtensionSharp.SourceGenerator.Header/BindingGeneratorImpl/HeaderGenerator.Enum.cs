using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Header.BindingGeneratorImpl;

partial class HeaderGenerator
{
    private static void ProcessEnum(Match match, StringBuilder stringBuilder, out string enumName)
    {
        string? enumBody = match.Groups["EnumBody"].Value;
        enumName = match.Groups["EnumName"].Value;


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
            string? titleComment = bodyMatch.Groups["TitleComment"].Value;
            string? enumValueName = bodyMatch.Groups["EnumName"].Value;
            string? enumValue = bodyMatch.Groups["EnumValue"].Value;
            string? enumComment = bodyMatch.Groups["EnumComment"].Value;

            if (!string.IsNullOrWhiteSpace(titleComment))
                stringBuilder
                   .AppendLine()
                   .AppendIndentation()
                   .AppendLine($"/* {titleComment.TrimStart().TrimEnd()} */");

            if (!string.IsNullOrWhiteSpace(enumComment))
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

            stringBuilder
               .AppendIndentation()
               .Append(enumValueName.EscapeContextualKeyWord());

            if (!string.IsNullOrWhiteSpace(enumValue))
                stringBuilder
                   .Append(" = ")
                   .Append(enumValue);

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
}
