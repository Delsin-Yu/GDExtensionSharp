namespace GDExtensionSharp.SourceGenerator.Header.Parser.PreprocessorDirective;

internal class IncludeDirective : PreprocessorDirective
{
    public IncludeDirective(string path)
    {
        Path = path;
    }

    public string Path { get; }
}