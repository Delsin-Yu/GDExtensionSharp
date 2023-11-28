using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
      private delegate string EnumFormatHandler(string enumName, string enumValueName);

    private static readonly Dictionary<string, EnumFormatHandler> _enumFormatHandlers =
        new()
        {
            { "Type", VariantTypeFormatHandler },
            { "Operator", VariantOpFormatHandler },
            { "Key", KeyEnumFormatHandler },
            { "KeyModifierMask", KeyModifierMaskEnumFormatHandler },
            { "PositionalShadowAtlasQuadrantSubdiv", PositionalShadowAtlasQuadrantSubdivEnumFormatHandler },
            { "Scaling3DMode", Scaling3DModeEnumFormatHandler },
            { "MSAA", MsaaEnumFormatHandler },
            { "SDFOversize", SDFOversizeEnumFormatHandler },
            { "SDFScale", SdfScaleEnumFormatHandler },
            { "VRSMode", VRSModeEnumFormatHandler },
        };

    private static readonly char[] _trimChars = ['_'];

    private static void GenerateEnum(StringBuilder stringBuilder, IReadOnlyList<GlobalEnumElement> enumElements)
    {
        if (enumElements == null) return;


        foreach (GlobalEnumElement enumElement in enumElements)
        {
            if (enumElement.IsBitfield)
            {
                stringBuilder.AppendIndentLine("[global::System.Flags]");
            }

            string enumElementName = enumElement.Name;

            IndentDisposable? outerScope = null;

            if (enumElementName.StartsWith("Variant."))
            {
                stringBuilder
                    .AppendIndentLine("public partial struct Variant")
                    .AppendIndentLine("{");

                enumElementName = enumElementName.Replace("Variant.", string.Empty);

                outerScope = stringBuilder.AddIndent();
            }

            stringBuilder
                .AppendIndentLine($"public enum {enumElementName}")
                .AppendIndentLine("{");

            {
                using var handle = stringBuilder.AddIndent();

                foreach (ValueElement elementValue in enumElement.Values)
                {
                    string elementValueName = elementValue.Name;

                    if (_enumFormatHandlers.TryGetValue(enumElementName, out var handler))
                    {
                        elementValueName = handler(enumElementName, elementValueName);
                    }
                    else
                    {
                        elementValueName = DefaultEnumFormatHandler(enumElementName, elementValueName);
                    }

                    stringBuilder.AppendIndentLine($"{elementValueName} = {elementValue.Value.ToString(CultureInfo.InvariantCulture)},");
                }
            }

            if (outerScope != null)
            {
                stringBuilder
                    .AppendIndentLine("}");
                outerScope.Value.Dispose();
            }

            stringBuilder
                .AppendIndentLine("}")
                .AppendIndentLine();
        }
    }



    private static string DefaultEnumFormatHandler(string enumName, string enumValueName)
    {
        enumName =
            enumName
                .PascalCaseToSnakeCase()
                .ToUpperInvariant()
                .TrimStart(_trimChars);
        return enumValueName
            .Replace(enumName, string.Empty)
            .TrimStart(_trimChars)
            .ToLowerInvariant()
            .SnakeCaseToPascalCase();
    }

    private static string VRSModeEnumFormatHandler(string enumName, string enumValueName)
    {
        string name = DefaultEnumFormatHandler("VRS_", enumValueName);
        if (name == "Xr") return "XR";
        return name;
    }

    private static string SDFOversizeEnumFormatHandler(string enumName, string enumValueName)
    {
        return "OverSize" + DefaultEnumFormatHandler("SDF_OVERSIZE_", enumValueName);
    }

    private static string SdfScaleEnumFormatHandler(string enumName, string enumValueName)
    {
        string name = DefaultEnumFormatHandler("SDF_SCALE_", enumValueName);

        return name switch
        {
            "Max" => name,
            _ => "Scale" + name
        };
    }

    private static string MsaaEnumFormatHandler(string enumName, string enumValueName)
    {
        string name = DefaultEnumFormatHandler("MSAA_", enumValueName);

        switch (name)
        {
            case "2x":
            case "4x":
            case "8x":
                return "Msaa" + name;
        }

        return name;
    }

    private static string Scaling3DModeEnumFormatHandler(string enumName, string enumValueName)
    {
        return DefaultEnumFormatHandler("SCALING_3D_MODE_", enumValueName["SCALING_3D_MODE_".Length..]);
    }

    private static string PositionalShadowAtlasQuadrantSubdivEnumFormatHandler(string enumName, string enumValueName)
    {
        string name = DefaultEnumFormatHandler("SHADOW_ATLAS_QUADRANT_SUBDIV_", enumValueName);

        switch (name)
        {
            case "1":
            case "4":
            case "16":
            case "64":
            case "256":
            case "1024":
                return "Subdiv" + name;
        }

        return name;
    }

    private static string KeyModifierMaskEnumFormatHandler(string enumName, string enumValueName)
    {
        return DefaultEnumFormatHandler("KEY", enumValueName);
    }

    private static string KeyEnumFormatHandler(string enumName, string enumValueName)
    {
        string name = DefaultEnumFormatHandler(enumName, enumValueName);

        switch (name)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                return "Key" + name;
        }

        return name;
    }

    private static string VariantTypeFormatHandler(string enumName, string enumValueName)
    {
        string generated = enumValueName
            .Replace("TYPE_", string.Empty)
            .ToLowerInvariant()
            .SnakeCaseToPascalCase();

        if (generated.EndsWith("i"))
        {
            return generated[..^1] + "I";
        }

        if (generated.EndsWith("d") && generated != "Rid")
        {
            return generated[..^1] + "D";
        }

        return generated;
    }

    private static string VariantOpFormatHandler(string enumName, string enumValueName)
    {
        return enumValueName
            .Replace("OP_", string.Empty)
            .ToLowerInvariant()
            .SnakeCaseToPascalCase();
    }

}
