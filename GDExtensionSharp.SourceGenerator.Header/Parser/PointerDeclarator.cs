using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class PointerDeclarator : Declarator
    {
        public PointerDeclarator(Declarator? declarator) : base(null)
        {
            Declarator = declarator;
        }

        public Declarator? Declarator { get; }
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray.Create((CSyntaxNode)Declarator);
    }
}
