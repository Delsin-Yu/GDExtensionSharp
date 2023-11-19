using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class TypeDefinition : CSyntaxNode
{
    public TypeDefinition(IEnumerable<Declarator> declarators, TypeSpecifier typeSpecifier)
    {
        Declarators = declarators.ToImmutableArray();
        TypeSpecifier = typeSpecifier;
    }
    public TypeSpecifier TypeSpecifier { get; }
    public ImmutableArray<Declarator> Declarators { get; }

    public override ImmutableArray<CSyntaxNode> Children => ImmutableArray.Create(TypeSpecifier as CSyntaxNode);
}