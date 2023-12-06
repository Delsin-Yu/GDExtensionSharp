using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static void GenerateClassEntry(string className, StringBuilder stringBuilder, string engineClassInherits, IEnumerable<string> entries)
    {
        stringBuilder
            .AppendIndentLine(engineClassInherits != null ? $"public new class {className} : {engineClassInherits}.{className}" : $"public class {className}")
            .AppendIndentLine("{");

        using (stringBuilder.AddIndent())
        {
            if (entries != null)
            {
                foreach (string entry in entries)
                {
                    stringBuilder
                        .AppendIndentLine($"""public static readonly StringName {entry.SnakeCaseToPascalCase()} = "{entry}";""");
                }
            }
        }

        stringBuilder
            .AppendIndentLine("}")
            .AppendIndentLine();
    }
}
