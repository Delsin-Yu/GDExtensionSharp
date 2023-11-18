using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api;

internal static partial class ApiGenerator
{
    private static IEnumerable<(string sourceName, string sourceContent)> GenerateGlobalEnums(StringBuilder stringBuilder, IEnumerable<GlobalEnumElement> globalEnumArray)
    {
        foreach (var globalEnumElement in globalEnumArray)
        {
            if(globalEnumElement.Name.StartsWith("Variant.")) continue;
            
            stringBuilder
               .AppendLine("namespace Godot;")
               .AppendLine();

            if (globalEnumElement.IsBitfield)
            {
                stringBuilder.AppendLine("[global::System.Flags]");
            }

            stringBuilder.AppendLine(
                $$"""
                  public enum {{globalEnumElement.Name}}
                  {
                  """
            );

            foreach (var valueElement in globalEnumElement.Values)
            {
                stringBuilder.AppendLine($"    {valueElement.Name} = {valueElement.Value.ToString(CultureInfo.InvariantCulture)},");
            }

            stringBuilder.AppendLine("}");

            yield return ($"Enum.{globalEnumElement.Name}", stringBuilder.ToString());
            stringBuilder.Clear();
        }
    }
}
