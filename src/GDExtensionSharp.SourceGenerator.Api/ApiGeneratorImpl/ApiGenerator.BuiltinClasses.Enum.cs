using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static string GenerateBuiltinClassEnums(StringBuilder stringBuilder, IReadOnlyList<BuiltinClassEnum> builtinClassEnums)
    {
        if (builtinClassEnums == null) return string.Empty;

        foreach (var builtinClassEnum in builtinClassEnums)
        {
            stringBuilder
                .AppendLine($$"""
                              public enum {{builtinClassEnum.Name}}
                              {
                              """);

            foreach (var valueElement in builtinClassEnum.Values)
            {
                stringBuilder.AppendLine($"    {valueElement.Name} = {valueElement.Value.ToString(CultureInfo.InvariantCulture)},");
            }

            stringBuilder
                .AppendLine("}");
        }

        return stringBuilder.ToStringAndClear();
    }
}
