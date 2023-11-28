using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static (string sourceName, string sourceContent) GenerateGlobalEnum(StringBuilder stringBuilder, GDExtensionApi gdExtensionApi)
    {
        stringBuilder
            .AppendIndentLine(NamespaceHeader)
            .AppendIndentLine();

        GenerateEnum(stringBuilder, gdExtensionApi.GlobalEnums);

        return ("GlobalEnum", stringBuilder.ToStringAndClear());
    }
}
