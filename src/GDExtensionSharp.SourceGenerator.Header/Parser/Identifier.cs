using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class Identifier : CSyntaxNode, ITypeIdentifier
{
    public Identifier(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
}