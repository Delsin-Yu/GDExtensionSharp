using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class PointerFieldDeclarator : CSyntaxNode, IFieldDeclarator
    {
        public PointerFieldDeclarator(FieldDeclarator? declarator)
        {
            Declarator = declarator;
        }

        public FieldDeclarator? Declarator { get; }
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
    }
}
