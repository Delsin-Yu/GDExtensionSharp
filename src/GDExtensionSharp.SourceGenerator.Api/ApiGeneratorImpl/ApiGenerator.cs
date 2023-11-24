using System.Globalization;
using System.Text;
using System.Text.Json;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    public static IEnumerable<(string sourceName, string sourceContent)> Generate(string jsonContent)
    {
        var stringBuilder = new StringBuilder();
        var gdExtensionApi = JsonSerializer.Deserialize<GDExtensionApi>(jsonContent, Converter.Settings);

        yield return GenerateGlobalConstants(stringBuilder, gdExtensionApi.GlobalConstants);

        yield return GenerateVersionHeader(gdExtensionApi.Header);

        yield return GenerateMethodHelper();

        yield return GenerateGlobalUsing();

        var size =
            gdExtensionApi
                .BuiltinClassSizes
                .First(size => size.BuildConfiguration == "double_64")
                .Sizes
                .ToDictionary(x => x.Name, x => x.SizeSize);

        yield return GenerateFixedSizeBuffer(stringBuilder, size);

        foreach (var generatedGlobalEnum in GenerateGlobalEnums(stringBuilder, gdExtensionApi.GlobalEnums))
        {
            yield return generatedGlobalEnum;
        }

        // foreach (var generatedBuiltinClass in GenerateBuiltinClasses(stringBuilder, gdExtensionApi.BuiltinClasses, size))
        // {
        //     yield return generatedBuiltinClass;
        // }

        stringBuilder.Clear();
    }
}
