using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class TranslationUnit : CSyntaxNode
    {
        public TranslationUnit(IEnumerable<CSyntaxNode> children)
        {
            Children = children.ToImmutableArray();
        }

        public override ImmutableArray<CSyntaxNode> Children { get; }
    }
}
