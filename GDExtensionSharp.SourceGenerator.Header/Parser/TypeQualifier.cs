namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class TypeQualifier:CSyntaxNode
    {
        public TypeQualifier(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
