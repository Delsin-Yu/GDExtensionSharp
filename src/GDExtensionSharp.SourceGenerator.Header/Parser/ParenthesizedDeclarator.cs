namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class ParenthesizedDeclarator : Declarator
{
    public ParenthesizedDeclarator(Declarator declarator) : base(null)
    {
        Declarator = declarator;
    }

    public Declarator Declarator { get; }
}