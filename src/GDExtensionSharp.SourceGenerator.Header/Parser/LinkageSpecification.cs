using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class LinkageSpecification : CSyntaxNode
{
    public LinkageSpecification(string value, CSyntaxNode body)
    {
        Value = value;
        Body = body;
    }

    public string Value { get; }

    public CSyntaxNode Body { get; }

    public override ImmutableArray<CSyntaxNode> Children
    {
        get
        {
            if (Body is DeclarationList list)
            {
                return list.Children;
            }
            return ImmutableArray<CSyntaxNode>.Empty;
        }
    }
}