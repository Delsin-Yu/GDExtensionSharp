namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class Declarator : CSyntaxNode
    {
        public Declarator(Identifier? identifier)
        {
            Identifier = identifier;
        }

        public Identifier? Identifier { get; }
    }
}
