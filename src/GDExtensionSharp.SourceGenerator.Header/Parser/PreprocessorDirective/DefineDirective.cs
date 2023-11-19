using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser.PreprocessorDirective;

internal class DefineDirective : PreprocessorDirective
{
    public DefineDirective(string defined, IEnumerable<string> args)
    {
        Defined = defined;
        Args = args.ToImmutableArray();
    }

    public string Defined { get; }
    public ImmutableArray<string> Args { get; }
}