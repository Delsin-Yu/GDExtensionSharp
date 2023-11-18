namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class IntegerLiteral : CSyntaxNode
    {
        public IntegerLiteral(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
