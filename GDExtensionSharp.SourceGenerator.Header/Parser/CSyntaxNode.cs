using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class CSyntaxNode
    {
        public virtual ImmutableArray<CSyntaxNode> Children { get; }
    }
}
