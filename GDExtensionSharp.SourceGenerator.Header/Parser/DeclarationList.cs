using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class DeclarationList : CSyntaxNode
    {
        public DeclarationList(IEnumerable<CSyntaxNode> declarations)
        {
            Declarations = declarations.ToImmutableArray();
        }

        public ImmutableArray<CSyntaxNode> Declarations { get; }
        public override ImmutableArray<CSyntaxNode> Children => Declarations;
    }
}
