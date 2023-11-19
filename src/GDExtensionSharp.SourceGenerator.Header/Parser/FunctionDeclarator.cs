namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class FunctionDeclarator : Declarator
{
    public FunctionDeclarator(Declarator declarator, ParameterListDeclaration parameterList) : base(null)
    {
        Declarator = declarator;
        ParameterList = parameterList;
    }

    public Declarator Declarator { get; }
    public ParameterListDeclaration ParameterList { get; }
}