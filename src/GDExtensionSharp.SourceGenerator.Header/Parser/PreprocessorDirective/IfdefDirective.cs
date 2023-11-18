namespace GDExtensionSharp.SourceGenerator.Header.Parser.PreprocessorDirective
{
    internal class IfdefDirective : PreprocessorDirective
    {
        public IfdefDirective(string condition)
        {
            Condition = condition;
        }

        public string Condition { get; }
    }
}
