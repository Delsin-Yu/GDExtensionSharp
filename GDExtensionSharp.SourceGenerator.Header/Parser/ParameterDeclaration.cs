namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class ParameterDeclaration : CSyntaxNode
    {
        public ParameterDeclaration(Declarator? declarator, ITypeIdentifier type)
        {
            Declarator = declarator;
            Type = type;
        }

        public Declarator? Declarator { get; }
        public ITypeIdentifier Type { get; }
    }
}
