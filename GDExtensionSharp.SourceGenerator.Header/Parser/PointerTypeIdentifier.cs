namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class PointerTypeIdentifier : ITypeIdentifier
    {
        public PointerTypeIdentifier(bool isConstant, ITypeIdentifier type)
        {
            IsConstant = isConstant;
            Type = type;
        }

        public bool IsConstant { get; }
        public ITypeIdentifier Type { get; }
    }
}
