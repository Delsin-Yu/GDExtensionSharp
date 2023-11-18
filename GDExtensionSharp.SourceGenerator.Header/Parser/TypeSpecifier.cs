namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal partial class TypeSpecifier : CSyntaxNode
    {
        public TypeSpecifier(ITypeIdentifier? type)
        {
            Type = type;
        }

        public ITypeIdentifier? Type { get; }
    }
}
