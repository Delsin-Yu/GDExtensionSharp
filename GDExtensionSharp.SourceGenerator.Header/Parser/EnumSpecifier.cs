using System.Collections.Immutable;

namespace GDExtensionSharp.SourceGenerator.Header.Parser;

class EnumSpecifier : TypeSpecifier
{
    public EnumSpecifier(ITypeIdentifier? name, IEnumerable<EnumConstant> values) : base(name)
    {
        Name = name;
        Values = values.ToImmutableArray();
    }

    public ITypeIdentifier? Name { get; }
    public ImmutableArray<EnumConstant> Values { get; }
}