using System.Globalization;
using System.Text;
using Humanizer;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static IEnumerable<(string sourceName, string sourceContent)> GenerateGlobalEnums(StringBuilder stringBuilder, IEnumerable<GlobalEnumElement> globalEnumArray)
    {
        foreach (var globalEnumElement in globalEnumArray)
        {
            string name = globalEnumElement.Name;

            stringBuilder
                .AppendLine(NamespaceHeader)
                .AppendLine();

            if (globalEnumElement.IsBitfield)
            {
                stringBuilder.AppendLine("[global::System.Flags]");
            }

            var valueMapper = MapEnum(name, out string mappedName, out string container);

            if (container != null)
            {
                stringBuilder.AppendLine($$"""
                                           public partial {{container}}
                                           {
                                           """);
            }

            stringBuilder.AppendLine($$"""
                                       public enum {{mappedName}}
                                       {
                                       """);

            foreach (var valueElement in globalEnumElement.Values)
            {
                stringBuilder.AppendLine($"    {valueMapper(valueElement.Name)} = {valueElement.Value.ToString(CultureInfo.InvariantCulture)},");
            }

            stringBuilder.AppendLine("}");

            if (container != null)
            {
                stringBuilder.AppendLine("}");
            }

            yield return ($"Enum.{mappedName}", stringBuilder.ToString());
            stringBuilder.Clear();
        }
    }

    private static Func<string, string> MapEnum(string enumName, out string mappedName, out string container)
    {
        const string variant = "Variant.";
        if (enumName.StartsWith(variant))
        {
            enumName = enumName[variant.Length..];
        }

        mappedName = enumName;
        container = null;

        switch (enumName)
        {
            default:
                container = null;
                return x => x;

            case "Error":
                return x => ValueMapper_TrimHeader("ERR", x);

            case "Type":
                mappedName = "Type";
                container = "struct Variant";
                return x =>
                {
                    string mapped = ValueMapper_TrimHeader("TYPE", x);

                    if ((mapped.Contains("Vector") || mapped.Contains("Rect")) && mapped.EndsWith("i"))
                    {
                        return mapped[..^1] + "I";
                    }

                    if (mapped.Contains("Transform") && mapped.EndsWith("d"))
                    {
                        return mapped[..^1] + "D";
                    }

                    return mapped;
                };

            case "Side":
                return x => ValueMapper_TrimHeader("SIDE", x);

            case "EulerOrder":
                return x => ValueMapper_TrimHeader("EULER_ORDER", x);
        }
    }

    private static string ValueMapper_TrimHeader(string header, string src)
    {
        if (src.StartsWith(header))
        {
            src = src[header.Length..];
        }

        return src.ToLowerInvariant().SnakeCaseToPascalCase();
    }
}
