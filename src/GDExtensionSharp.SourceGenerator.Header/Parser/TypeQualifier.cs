using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class TypeQualifier : CSyntaxNode
    {
        public TypeQualifier(string value)
        {
            Value = value;
        }

        public string Value { get; }
        public override ImmutableArray<CSyntaxNode> Children => ImmutableArray<CSyntaxNode>.Empty;
    }
}
