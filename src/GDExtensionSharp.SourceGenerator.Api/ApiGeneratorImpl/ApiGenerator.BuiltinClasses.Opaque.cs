using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string OpaqueFixedSizeArrayName = "OpaqueData";
    private const string OpaqueCopyToMethodName = "CopyTo";
    private const string OpaqueFieldName = "_opaque";
    private const string OpaqueDataSize = "uint8_t";
    private const string OpaqueDataPointerName = "opaqueDataPointer";

    private static string GenerateBuiltinClassOpaqueData(long classSize)
    {
        var classSizeString = classSize.ToString(CultureInfo.InvariantCulture);
        return $"private {OpaqueFixedSizeArrayName}{classSizeString} {OpaqueFieldName} = new();";
    }

    private static readonly StringBuilder _opaqueDataCommandStringBuilder = new();
    private static string GenerateBufferToPointerCommand(long classSize, string selfName = null)
    {
        if (!string.IsNullOrWhiteSpace(selfName))
        {
            selfName = $"{selfName}.";
        }
        else
        {
            selfName = string.Empty;
        }

        _opaqueDataCommandStringBuilder
            .Append($"var {OpaqueDataPointerName} = stackalloc {OpaqueDataSize}[")
            .Append(classSize)
            .AppendLine("];")
            .Append(OpaqueFixedSizeArrayName)
            .Append(classSize)
            .Append($".{OpaqueCopyToMethodName}(in {selfName}{OpaqueFieldName}, {OpaqueDataPointerName});");

        return _opaqueDataCommandStringBuilder.ToStringAndClear();
    }

    private static string GeneratePointerToBufferCommand(long classSize)
    {
        _opaqueDataCommandStringBuilder
            .Append(OpaqueFixedSizeArrayName)
            .Append(classSize)
            .Append($".{OpaqueCopyToMethodName}({OpaqueDataPointerName}, ref {OpaqueFieldName});");

        return _opaqueDataCommandStringBuilder.ToStringAndClear();
    }

}
