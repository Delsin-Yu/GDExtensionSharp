using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal partial class PrimitiveType : CSyntaxNode, ITypeIdentifier
    {
        public PrimitiveType(string type)
        {
            Type = type;
        }

        public string Type { get; }
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
    }
}
