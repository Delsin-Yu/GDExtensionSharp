using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class ParameterListDeclaration
{
    public ParameterListDeclaration(IEnumerable<ParameterDeclaration> parameters)
    {
        Parameters = parameters.ToImmutableArray();
    }

    public ImmutableArray<ParameterDeclaration> Parameters { get; }
}