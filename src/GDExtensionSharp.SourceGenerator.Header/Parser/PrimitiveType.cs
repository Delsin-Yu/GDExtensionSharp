using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal partial class PrimitiveType : CSyntaxNode, ITypeIdentifier
{
    public PrimitiveType(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
}