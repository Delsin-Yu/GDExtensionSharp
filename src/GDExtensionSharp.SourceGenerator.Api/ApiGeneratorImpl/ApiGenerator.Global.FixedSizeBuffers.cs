using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static (string sourceName, string sourceContent) GenerateFixedSizeBuffer(StringBuilder stringBuilder, IReadOnlyDictionary<string, long> builtinClassSizes)
    {
        var allClassSizes = new HashSet<long>(builtinClassSizes.Values);

        stringBuilder
            .AppendLine(NamespaceHeader)
            .AppendLine();

        foreach (var size in allClassSizes.OrderBy(x => x))
        {
            if (size == 0) continue;

            var sizeString = size.ToString(CultureInfo.InvariantCulture);
            stringBuilder.AppendLine($$"""
                                       [System.Runtime.CompilerServices.InlineArray({{sizeString}})]
                                       public struct {{OpaqueFixedSizeArrayName}}{{sizeString}}
                                       {
                                           private {{OpaqueDataSize}} _element0;
                                       """);

            GenerateCopyMethods(stringBuilder, size, sizeString);

            stringBuilder.AppendLine("}");
        }

        return ("OpaqueDataBuffers", stringBuilder.ToStringAndClear());
    }

    private static void GenerateCopyMethods(StringBuilder stringBuilder, long size, string sizeString)
    {
        const string dataName = "data";

        stringBuilder
            .Append(Indents)
            .Append($"internal static unsafe void {OpaqueCopyToMethodName}(ref readonly {OpaqueFixedSizeArrayName}")
            .Append(sizeString)
            .AppendLine($" {dataName}, {OpaqueDataSize}* {OpaqueDataPointerName})")
            .Append(Indents)
            .AppendLine("{");

        for (int i = 0; i < size; i++)
        {
            stringBuilder
                .Append(Indents)
                .Append(Indents)
                .Append($"{OpaqueDataPointerName}[")
                .Append(i)
                .Append($"] = {dataName}[")
                .Append(i)
                .AppendLine("];");
        }

        stringBuilder
            .Append(Indents)
            .AppendLine("}")
            .AppendLine()
            .Append(Indents)
            .Append($"internal static unsafe void {OpaqueCopyToMethodName}({OpaqueDataSize}* {OpaqueDataPointerName}, ref {OpaqueFixedSizeArrayName}")
            .Append(sizeString)
            .AppendLine($" {dataName})")
            .Append(Indents)
            .AppendLine("{");

        for (int i = 0; i < size; i++)
        {
            stringBuilder
                .Append(Indents)
                .Append(Indents)
                .Append($"{dataName}[")
                .Append(i)
                .Append($"] = {OpaqueDataPointerName}[")
                .Append(i)
                .AppendLine("];");
        }

        stringBuilder
            .Append(Indents)
            .AppendLine("}");

    }

}
