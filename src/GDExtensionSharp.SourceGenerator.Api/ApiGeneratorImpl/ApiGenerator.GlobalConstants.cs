using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static (string sourceName, string sourceContent) GenerateGlobalConstants(StringBuilder stringBuilder, IReadOnlyList<GlobalConstantElement> globalConstantElements)
    {
        stringBuilder.AppendLine(
            """
            namespace Godot;

            public static class GlobalConstants
            {
            """
        );

        foreach (var globalConstantElement in globalConstantElements)
        {
            stringBuilder.AppendLine($"    public const int {globalConstantElement.Name.EscapeContextualKeyWord()} = {globalConstantElement.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        stringBuilder.AppendLine("}");

        var result = stringBuilder.ToString();
        stringBuilder.Clear();
        
        return ("GlobalConstants", result);
    }
}
