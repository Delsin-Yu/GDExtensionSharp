using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace GDExtensionSharp.SourceGenerator.Header;

[Generator(LanguageNames.CSharp)]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files =
            context
               .AdditionalTextsProvider
               .Where(
                    provider => provider.Path.EndsWith(
                        "gdextension_interface.h",
                        StringComparison.InvariantCultureIgnoreCase
                    )
                )
               .Select((text, token) => text.GetText(token)!.ToString())
               .Collect();

        context.RegisterSourceOutput(files, GenerateCSharpBindingForHeaderFile);
    }

    private static void GenerateCSharpBindingForHeaderFile(SourceProductionContext context, ImmutableArray<string> headerContentArray)
    {
        foreach (var headerContent in headerContentArray)
        {
            foreach (var (sourceName, sourceContent) in BindingGenerator.Generate(headerContent))
            {
                context.AddSource($"{sourceName}.g.cs", sourceContent);
            }
            break;
        }
    }
}
