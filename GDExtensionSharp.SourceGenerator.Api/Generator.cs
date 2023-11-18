using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace GDExtensionSharp.SourceGenerator.Api;

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
                        "extension_api.json",
                        StringComparison.InvariantCultureIgnoreCase
                    )
                )
               .Select((text, token) => text.GetText(token)!.ToString())
               .Collect();

        context.RegisterSourceOutput(files, GenerateCSharpApiFromJson);
    }

    private static void GenerateCSharpApiFromJson(SourceProductionContext context, ImmutableArray<string> jsonContentArray)
    {
        foreach (var jsonContent in jsonContentArray)
        {
            foreach (var (sourceName, sourceContent) in ApiGenerator.Generate(jsonContent))
            {
                context.AddSource($"{sourceName}.g.cs", sourceContent);
            }
            break;
        }
    }
}
