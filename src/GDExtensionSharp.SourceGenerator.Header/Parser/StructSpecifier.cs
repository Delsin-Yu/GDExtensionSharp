using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

internal class StructSpecifier : TypeSpecifier
{
    public StructSpecifier(ITypeIdentifier? name, IEnumerable<FieldDeclaration> fields) : base(name)
    {
        Name = name;
        Fields = fields.ToImmutableArray();
    }

    public ITypeIdentifier? Name { get; }
    public ImmutableArray<FieldDeclaration> Fields { get; }
}