using System.Collections;
using System.Text.Json;

namespace GDExtensionSharp.SourceGenerator.Api;

internal static class ApiGenerator
{
    public static IEnumerable<(string sourceName, string sourceContent)> Generate(string jsonContent)
    {
        var gdExtensionApi = JsonSerializer.Deserialize<GDExtensionApi>(jsonContent, Converter.Settings);
        yield break;
    }
}
