using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Header;

partial class BindingGenerator
{
    private static void ProcessEnum(Match match, StringBuilder stringBuilder, out string enumName)
    {
        var enumBody = match.Groups["EnumBody"].Value;
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
            var titleComment = bodyMatch.Groups["TitleComment"].Value;
            var enumValueName = bodyMatch.Groups["EnumName"].Value;
            var enumValue = bodyMatch.Groups["EnumValue"].Value;
            var enumComment = bodyMatch.Groups["EnumComment"].Value;

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
