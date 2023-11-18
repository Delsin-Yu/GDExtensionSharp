using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser.PreprocessorDirective
{
    internal class PreprocessorDirective : CSyntaxNode
    {
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
    }
}
