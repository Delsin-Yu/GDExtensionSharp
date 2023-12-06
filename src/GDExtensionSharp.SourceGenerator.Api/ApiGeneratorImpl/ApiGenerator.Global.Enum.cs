using System.Collections.Immutable;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static (string sourceName, string sourceContent) GenerateGlobalEnum(StringBuilder stringBuilder, GDExtensionApi gdExtensionApi)
    {
        stringBuilder
            .AppendIndentLine(NamespaceHeader)
            .AppendIndentLine();

        GenerateEnum(stringBuilder, ImmutableHashSet<string>.Empty, gdExtensionApi.GlobalEnums);

        return ("GlobalEnum", stringBuilder.ToStringAndClear());
    }
}
