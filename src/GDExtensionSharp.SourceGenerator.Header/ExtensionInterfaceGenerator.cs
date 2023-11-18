using System.Text;
// using GDExtensionSharp.SourceGenerator.Header.RegexParser;
using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace GDExtensionSharp.SourceGenerator.Header;

[Generator(LanguageNames.CSharp)]
public class ExtensionInterfaceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var source = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider).Where(t =>
                                 t.Right.GetOptions(t.Left).TryGetValue("build_metadata.AdditionalFiles.SourceItemGroup",
                                     out var sourceItemGroup) && sourceItemGroup == "ExtensionInterface" &&
                                 t.Left.Path.EndsWith(".h"))
                            .Select((t, _) => t.Left).Collect();

        context.RegisterSourceOutput(source, (spc, input) =>
        {
            foreach (var additionalText in input)
            {
                // foreach (var (sourceName, sourceContent) in HeaderGenerator.Generate(additionalText.GetText()!.ToString()))
                // {
                //     spc.AddSource($"{sourceName}.g.cs", sourceContent);
                // }
                // return;
                
                var parser = new CParser();
                var translationUnit = parser.Parse(additionalText.GetText()!.ToString());
                var enumTranspiler = new EnumTranspiler();
                var enumDeclarations = enumTranspiler.Transpile(translationUnit);
                var enumCompilationUnit = CompilationUnit().AddMembers(enumDeclarations.OfType<MemberDeclarationSyntax>().ToArray());
                spc.AddSource("ExtensionInterface.Enum.g.cs", enumCompilationUnit.NormalizeWhitespace().GetText(Encoding.UTF8));
            }
        });
    }


}
