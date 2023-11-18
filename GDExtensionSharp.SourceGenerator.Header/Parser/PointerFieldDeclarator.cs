namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class PointerFieldDeclarator : CSyntaxNode, IFieldDeclarator
    {
        public PointerFieldDeclarator(FieldDeclarator? declarator)
        {
            Declarator = declarator;
        }

        public FieldDeclarator? Declarator { get; }
    }
}
