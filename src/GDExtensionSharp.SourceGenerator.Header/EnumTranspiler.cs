using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDExtensionSharp.SourceGenerator.Header;
internal class EnumTranspiler
{
    public ImmutableArray<EnumDeclarationSyntax> Transpile(CSyntaxNode node)
    {
        return Transpile1(node).ToImmutableArray();

        IEnumerable<EnumDeclarationSyntax> Transpile1(CSyntaxNode node1)
        {
            foreach (var valueTuple in Visit(node1))
            {
                if (valueTuple.typeDefinition.Declarators is not [{ Identifier: { } identifier } declarator])
                {
                    continue;
                }

                var enumDeclaration = EnumDeclaration(identifier.Name)
                   .AddModifiers(Token(SyntaxKind.PublicKeyword));
                foreach (var value in valueTuple.enumSpecifier.Values)
                {
                    if (value.Value is null)
                    {
                        enumDeclaration = enumDeclaration.AddMembers(EnumMemberDeclaration(value.Name.Name));
                    }
                    if (value.Value is Identifier i)
                    {
                        enumDeclaration = enumDeclaration.AddMembers(
                            EnumMemberDeclaration(value.Name.Name)
                               .WithEqualsValue(EqualsValueClause(IdentifierName(i.Name)))
                        );
                    }

                    if (value.Value is IntegerLiteral l)
                    {
                        enumDeclaration = enumDeclaration.AddMembers(
                            EnumMemberDeclaration(value.Name.Name)
                               .WithEqualsValue(EqualsValueClause(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(l.Value))))
                        );
                    }
                }
                yield return enumDeclaration;
            }
        }
    }

    private IEnumerable<(TypeDefinition typeDefinition, EnumSpecifier enumSpecifier)> Visit(CSyntaxNode? node)
    {
        if (node is null)
        {
            yield break;
        }

        if (node is TypeDefinition typeDefinition)
        {
            foreach (var enumSpecifier in VisitTypeDefinition(typeDefinition))
            {
                yield return (typeDefinition, enumSpecifier);
            }
        }

        foreach (var nodeChild in node.Children)
        {
            foreach (var valueTuple in Visit(nodeChild))
            {
                yield return valueTuple;
            }
        }
    }

    private IEnumerable<EnumSpecifier> VisitTypeDefinition(TypeDefinition? node)
    {
        if (node is null)
        {
            yield break;
        }

        foreach (var nodeChild in node.Children)
        {
            if (nodeChild is EnumSpecifier enumSpecifier)
            {
                yield return enumSpecifier;
            }
        }
    }
}
