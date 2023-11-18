namespace GDExtensionSharp.SourceGenerator.Header.Parser
{
    internal class FunctionPointerSpecifier : TypeSpecifier
    {
        public FunctionPointerSpecifier(ITypeIdentifier returnType, PointerDeclarator declarator, ParameterListDeclaration parameterList) : base(null)
        {
            ReturnType = returnType;
            Declarator = declarator;
            ParameterList = parameterList;
        }

        public ITypeIdentifier ReturnType { get; }
        public PointerDeclarator Declarator { get; }
        public ParameterListDeclaration ParameterList { get; }
    }
}
