using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GDExtensionSharp.SourceGenerator.Header;

internal class TypeForwardResolver
{
    public TypeForwardResolver(IDictionary<string, TypeSyntax> aliases) => _aliases = new(aliases);

    private readonly Dictionary<string, TypeSyntax> _aliases;

    public void Add(string alias, TypeSyntax value) => _aliases[alias] = value;

    public TypeSyntax Resolve(string alias)
    {
        TypeSyntax ret = SyntaxFactory.IdentifierName(alias);
        while (_aliases.TryGetValue(alias, out var value))
        {
            if (value is IdentifierNameSyntax identifierName)
            {
                alias = identifierName.Identifier.Text;
            }
            else
            {
                ret = value;
                break;
            }
        }

        if (ret is IdentifierNameSyntax returnIdentifierName)
        {
            ret = SyntaxFactory.IdentifierName($"global::GDExtensionSharp.{returnIdentifierName.Identifier.Text}");
        }
        return ret;
    }
}
