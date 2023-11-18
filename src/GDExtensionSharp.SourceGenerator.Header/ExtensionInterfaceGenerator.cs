using System.Collections.Immutable;
using GDExtensionSharp.SourceGenerator.Header.BindingGeneratorImpl;
using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis;

namespace GDExtensionSharp.SourceGenerator.Header;

[Generator(LanguageNames.CSharp)]
public class ExtensionInterfaceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var source = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider).Where(t =>
                                 t.Right.GetOptions(t.Left).TryGetValue("build_metadata.AdditionalFiles.SourceItemGroup",
                                     out string? sourceItemGroup) && sourceItemGroup == "ExtensionInterface" &&
                                 t.Left.Path.EndsWith(".h"))
                            .Select((t, _) => t.Left).Collect();

        context.RegisterSourceOutput(source, (spc, input) =>
        {
            foreach (AdditionalText additionalText in input)
            {
                var parser = new CParser();
                var translationUnit = parser.Parse(additionalText.GetText()!.ToString());
                VisitEnumSpecifier(translationUnit);
            }
        });
    }

    private void VisitEnumSpecifier(CSyntaxNode node)
    {
        if (node is EnumSpecifier enumSpecifier)
        {

        }

        foreach (CSyntaxNode cSyntaxNode in node.Children)
        {
            VisitEnumSpecifier(cSyntaxNode);
        }
    }
}
