using System.Text;
using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace GDExtensionSharp.SourceGenerator.Header;

[Generator(LanguageNames.CSharp)]
public class ExtensionInterfaceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var source = context.AdditionalTextsProvider
                            .Combine(context.AnalyzerConfigOptionsProvider)
                            .Where(t => t.Right.GetOptions(t.Left).TryGetValue("build_metadata.AdditionalFiles.SourceItemGroup", out var sourceItemGroup) && sourceItemGroup == "ExtensionInterface" && t.Left.Path.EndsWith(".h"))
                            .Select((t, _) => t.Left)
                            .Collect();

        context.RegisterSourceOutput(source, (spc, input) =>
        {
            foreach (var additionalText in input)
            {
                var parser = new CParser();
                var translationUnit = parser.Parse(additionalText.GetText()!.ToString());
                var typeTranspiler = new TypeTranspiler();
                var aliasTranspiler = new AliasTranspiler();
                var typeDeclarations = typeTranspiler.Transpile(translationUnit);
                var globalUsingDirectives = aliasTranspiler.Transpile(translationUnit);
                var aliasesCompilationUnit = CompilationUnit().AddUsings(globalUsingDirectives.ToArray());
                var typesCompilationUnit = CompilationUnit()
                                          .AddMembers(FileScopedNamespaceDeclaration(IdentifierName("GDExtensionSharp")))
                                          .AddMembers(typeDeclarations.ToArray());
                spc.AddSource("ExtensionInterface.Types.g.cs", typesCompilationUnit.NormalizeWhitespace().GetText(Encoding.UTF8, SourceHashAlgorithm.Sha256));
                spc.AddSource("ExtensionInterface.Aliases.g.cs", aliasesCompilationUnit.NormalizeWhitespace().GetText(Encoding.UTF8, SourceHashAlgorithm.Sha256));
            }
        });
    }


}
