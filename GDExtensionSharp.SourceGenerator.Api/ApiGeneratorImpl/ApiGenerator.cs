using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace GDExtensionSharp.SourceGenerator.Api;

internal static partial class ApiGenerator
{
    public static IEnumerable<(string sourceName, string sourceContent)> Generate(string jsonContent)
    {
        var stringBuilder = new StringBuilder();
        var gdExtensionApi = JsonSerializer.Deserialize<GDExtensionApi>(jsonContent, Converter.Settings);

        yield return GenerateGlobalConstants(stringBuilder, gdExtensionApi.GlobalConstants);

        yield return GenerateVersionHeader(gdExtensionApi.Header);
        
        yield return GenerateMethodHelper();

        foreach (var generatedGlobalEnum in GenerateGlobalEnums(stringBuilder, gdExtensionApi.GlobalEnums))
        {
            yield return generatedGlobalEnum;
        }

        foreach (var generatedBuiltinClass in GenerateBuiltinClasses(stringBuilder, gdExtensionApi.BuiltinClasses, gdExtensionApi.BuiltinClassSizes))
        {
            yield return generatedBuiltinClass;
        }

        stringBuilder.Clear();
    }
}
