using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class IntegerLiteral : CSyntaxNode
    {
        public IntegerLiteral(int value)
        {
            Value = value;
        }

        public int Value { get; }
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
    }
}
