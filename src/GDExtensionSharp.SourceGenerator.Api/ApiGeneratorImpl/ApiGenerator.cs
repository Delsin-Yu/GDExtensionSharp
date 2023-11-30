using System.Text;
using System.Text.Json;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    public static IEnumerable<(string sourceName, string sourceContent)> Generate(string jsonContent)
    {
        var stringBuilder = new StringBuilder();
        var gdExtensionApi = JsonSerializer.Deserialize<GDExtensionApi>(jsonContent, Converter.Settings);

        yield return GenerateVersionHeader(gdExtensionApi.Header);

        yield return GenerateGlobalUsing();

        yield return GenerateGlobalEnum(stringBuilder, gdExtensionApi);

        foreach (var generatedEngineClass in GenerateEngineClasses(stringBuilder, gdExtensionApi.Classes))
        {
            yield return generatedEngineClass;
        }

        stringBuilder.Clear();
    }
}
