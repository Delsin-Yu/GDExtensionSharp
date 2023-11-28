using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static IEnumerable<(string sourceName, string sourceContent)> GenerateEngineClasses(StringBuilder stringBuilder, IReadOnlyList<Class> classes)
    {
        var godotTypes = new HashSet<string>(classes.Select(x => x.Name));

        foreach (Class engineClass in classes)
        {
            string engineClassName = engineClass.Name;

            string engineClassInherits = engineClass.Inherits;

            if (engineClassName == "Object") engineClassName = "GodotObject";
            if (engineClassInherits == "Object") engineClassInherits = "GodotObject";

            stringBuilder
                .AppendIndentLine("using System.Diagnostics;")
                .AppendIndentLine("using Godot.NativeInterop;")
                .AppendIndentLine()
                .AppendIndentLine(NamespaceHeader)
                .AppendIndentLine();

            bool inherits = !string.IsNullOrWhiteSpace(engineClassInherits);

            stringBuilder
                .AppendIndentLine(inherits ? $"public partial class {engineClassName} : {engineClassInherits}" : $"public partial class {engineClassName}")
                .AppendIndentLine("{");

            {
                using var handle = stringBuilder.AddIndent();

                GenerateEnum(stringBuilder, engineClass.Enums);

                stringBuilder
                    .AppendIndentLine($"private static readonly StringName NativeName = \"{engineClassName}\";")
                    .AppendIndentLine();

                GenerateClassHeader(stringBuilder, inherits, engineClassName, engineClass);

                GenerateClassMethod(stringBuilder, engineClass.Methods, godotTypes);

                GenerateClassMethodName(stringBuilder, inherits, engineClassInherits, engineClass);
            }


            stringBuilder
                .AppendLine()
                .AppendLine("}");

            yield return ($"EngineClass.{engineClassName}", stringBuilder.ToStringAndClear());
        }
    }
}
