namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class FieldDeclaration : CSyntaxNode
    {
        public FieldDeclaration(IFieldDeclarator declarator, ITypeIdentifier type)
        {
            Declarator = declarator;
            Type = type;
        }

        public IFieldDeclarator Declarator { get; }
        public ITypeIdentifier Type { get; }


    }
}
