using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal abstract class CSyntaxNode
{
    public abstract ImmutableArray<CSyntaxNode> Children { get; }
}