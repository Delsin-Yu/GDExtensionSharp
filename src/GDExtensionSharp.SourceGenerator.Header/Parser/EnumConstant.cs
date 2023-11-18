using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

class EnumConstant : CSyntaxNode
{
    public EnumConstant(Identifier name, CSyntaxNode? value)
    {
        Name = name;
        Value = value;
    }

    public Identifier Name { get; }
    public CSyntaxNode? Value { get; }
    public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
}