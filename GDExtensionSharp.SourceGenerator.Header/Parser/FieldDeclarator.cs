namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class FieldDeclarator : Declarator
    {
        public FieldDeclarator(Identifier identifier) : base(identifier)
        {
            Identifier = identifier;
        }

        public Identifier Identifier { get; }
    }
}
