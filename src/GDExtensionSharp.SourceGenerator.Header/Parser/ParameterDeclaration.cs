using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class ParameterDeclaration : CSyntaxNode
{
    public ParameterDeclaration(Declarator? declarator, ITypeIdentifier type)
    {
        Declarator = declarator;
        Type = type;
    }

    public Declarator? Declarator { get; }
    public ITypeIdentifier Type { get; }
    public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
}