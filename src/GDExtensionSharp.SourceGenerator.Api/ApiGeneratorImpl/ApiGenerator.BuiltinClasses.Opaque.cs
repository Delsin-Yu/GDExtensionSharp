using System.Globalization;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string OpaqueFieldName = "_opaque";
    private static string GenerateBuiltinClassOpaqueData(long classSize) => $"private readonly uint8_t[] {OpaqueFieldName} = new uint8_t[{classSize.ToString(CultureInfo.InvariantCulture)}];";
}
