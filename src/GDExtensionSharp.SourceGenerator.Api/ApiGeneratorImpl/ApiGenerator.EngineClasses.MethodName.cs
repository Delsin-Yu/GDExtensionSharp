using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static void GenerateClassMethodName(StringBuilder stringBuilder, bool inherits, string engineClassInherits, Class engineClass)
    {
        stringBuilder
            .AppendIndentLine(inherits ? $"public new class MethodName : {engineClassInherits}.MethodName" : "public class MethodName")
            .AppendIndentLine("{");

        {
            using var handle = stringBuilder.AddIndent();

            if (engineClass.Methods != null)
            {
                foreach (var engineClassMethod in engineClass.Methods)
                {
                    stringBuilder
                        .AppendIndent("public static readonly StringName ")
                        .Append(engineClassMethod.Name.SnakeCaseToPascalCase())
                        .AppendLine($" = \"{engineClassMethod.Name}\";");
                }
            }
        }

        stringBuilder
            .AppendIndentLine("}");
    }
}
