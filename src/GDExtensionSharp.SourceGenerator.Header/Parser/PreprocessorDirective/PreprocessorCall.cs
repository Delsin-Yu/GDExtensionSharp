using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser.PreprocessorDirective;

internal class PreprocessorCall : CSyntaxNode
{
    public PreprocessorCall(PreprocessorDirective preprocessorDirective)
    {
        PreprocessorDirective = preprocessorDirective;
    }

    public PreprocessorDirective PreprocessorDirective { get; }
    public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
}