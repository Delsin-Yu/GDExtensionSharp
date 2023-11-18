using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class Declarator : CSyntaxNode
    {
        public Declarator(Identifier? identifier)
        {
            Identifier = identifier;
        }

        public Identifier? Identifier { get; }
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
    }
}
