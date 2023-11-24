using System.Collections.Immutable;
using GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GDExtensionSharp.SourceGenerator.Api;

[Generator(LanguageNames.CSharp)]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext initializationContext)
    {
        var source =
            initializationContext
               .AdditionalTextsProvider
               .Combine(initializationContext.AnalyzerConfigOptionsProvider)
               .Where(MatchExtensionApiJson)
               .Select((t, _) => t.Left)
               .Collect();

        initializationContext.RegisterSourceOutput(
            source,
            GenerateCSharpApi
        );
    }

    private static bool MatchExtensionApiJson((AdditionalText Left, AnalyzerConfigOptionsProvider Right) provider)
    {
        var analyzerConfigOptions = provider.Right.GetOptions(provider.Left);
        if (analyzerConfigOptions.TryGetValue("build_metadata.AdditionalFiles.SourceItemGroup", out var sourceItemGroup))
        {
            return sourceItemGroup == "ExtensionApi" && provider.Left.Path.EndsWith(".json");
        }
        return false;
    }

    private static void GenerateCSharpApi(SourceProductionContext sourceProductionContext, ImmutableArray<AdditionalText> jsonContentArray)
    {
        foreach (var jsonContent in jsonContentArray)
        {
            var generated = ApiGenerator.Generate(jsonContent.GetText(sourceProductionContext.CancellationToken)!.ToString());
            foreach (var (sourceName, sourceContent) in generated)
            {
                sourceProductionContext.AddSource($"{sourceName}.g.cs", sourceContent);
            }

            break;
        }
    }
}
