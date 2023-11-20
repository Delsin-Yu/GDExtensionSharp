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

        foreach (var generatedGlobalEnum in GenerateGlobalEnums(stringBuilder, gdExtensionApi.GlobalEnums))
        {
            yield return generatedGlobalEnum;
        }

        var size =
            gdExtensionApi
               .BuiltinClassSizes
               .First(size => size.BuildConfiguration == "double_64")
               .Sizes
               .ToDictionary(x => x.Name, x => x.SizeSize);

        foreach (var generatedBuiltinClass in GenerateBuiltinClasses(stringBuilder, gdExtensionApi.BuiltinClasses, size))
        {
            yield return generatedBuiltinClass;
        }

        stringBuilder.Clear();
    }

    private static (string sourceName, string sourceContent) GenerateGlobalUsing()
    {
        const string content =
            """
            global using int8_t = System.SByte;
            global using int16_t = System.Int16;
            global using int32_t = System.Int32;
            global using int64_t = System.Int64;
            global using uint8_t = System.Byte;
            global using uint16_t = System.UInt16;
            global using uint32_t = System.UInt32;
            global using uint64_t = System.UInt64;

            global using size_t = System.UInt64;

            global using wchar_t = System.UInt16;
            """;

        return ("GlobalUsing", content);
    }
}
