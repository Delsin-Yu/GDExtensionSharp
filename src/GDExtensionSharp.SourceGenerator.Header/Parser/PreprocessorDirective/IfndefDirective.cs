namespace GDExtensionSharp.SourceGenerator.Header.Parser.PreprocessorDirective;

internal class IfndefDirective : PreprocessorDirective
{
    public IfndefDirective(string condition)
    {
        Condition = condition;
    }

    public string Condition { get; }
}