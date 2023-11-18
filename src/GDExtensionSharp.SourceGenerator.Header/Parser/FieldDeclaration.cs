using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class FieldDeclaration : CSyntaxNode
    {
        public FieldDeclaration(IFieldDeclarator declarator, ITypeIdentifier type)
        {
            Declarator = declarator;
            Type = type;
        }

        public IFieldDeclarator Declarator { get; }
        public ITypeIdentifier Type { get; }
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;

    }
}
