using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDExtensionSharp.SourceGenerator.Header;

internal class TypeTranspiler
{
    public IEnumerable<MemberDeclarationSyntax> Transpile(CSyntaxNode node)
    {
        foreach (var (typeDefinition, typeSpecifier) in Visit(node))
        {
            if (typeDefinition.Declarators is not [{ Identifier: { } identifier }])
            {
                continue;
            }

            if (typeSpecifier is EnumSpecifier enumSpecifier)
            {
                var enumDeclaration = EnumDeclaration(identifier.Name)
                   .AddModifiers(Token(SyntaxKind.InternalKeyword));
                foreach (var value in enumSpecifier.Values)
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

            if (typeSpecifier is StructSpecifier structSpecifier)
            {
                var structDeclaration = StructDeclaration(identifier.Name)
                   .AddModifiers(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.UnsafeKeyword));
                foreach (var field in structSpecifier.Fields)
                {
                    if (field.Declarator.Identifier is { } fieldIdentifier1)
                    {
                        string fieldType = field.Type.Name;
                        string fieldIdentifier = fieldIdentifier1.Name;
                        structDeclaration = structDeclaration
                           .AddMembers(
                                FieldDeclaration(VariableDeclaration(IdentifierName(fieldType)))
                                   .AddModifiers(Token(SyntaxKind.PublicKeyword))
                                   .AddDeclarationVariables(VariableDeclarator(Identifier(fieldIdentifier)))
                            );
                    }

                    if (field.Declarator is PointerDeclarator { Declarator: { Identifier: { } fieldIdentifier2 } })
                    {
                        string fieldType = field.Type.Name;
                        if (fieldType == "char")
                        {
                            fieldType = "byte";
                        }
                        string fieldIdentifier = fieldIdentifier2.Name;
                        if (SyntaxFacts.IsKeywordKind(SyntaxFacts.GetKeywordKind(fieldIdentifier)))
                        {
                            fieldIdentifier = $"@{fieldIdentifier}";
                        }
                        structDeclaration = structDeclaration
                           .AddMembers(
                                FieldDeclaration(VariableDeclaration(PointerType(IdentifierName(fieldType))))
                                   .AddModifiers(Token(SyntaxKind.PublicKeyword))
                                   .AddDeclarationVariables(VariableDeclarator(Identifier(fieldIdentifier)))
                            );
                    }

                    if (field.Declarator is FunctionDeclarator { ParameterList: { } parameterList1, Declarator: ParenthesizedDeclarator { Declarator: PointerDeclarator { Declarator.Identifier: { } fieldIdentifier3 } } })
                    {
                        var functionPointerType = FunctionPointerType();
                        functionPointerType = functionPointerType
                           .WithCallingConvention(
                                FunctionPointerCallingConvention(Token(SyntaxKind.UnmanagedKeyword))
                                   .AddUnmanagedCallingConventionListCallingConventions(FunctionPointerUnmanagedCallingConvention(Identifier("Cdecl")))
                            );
                        foreach (var parameter in parameterList1.Parameters)
                        {
                            if (parameter.Declarator is { Identifier: not null })
                            {
                                functionPointerType = functionPointerType
                                   .AddParameterListParameters(FunctionPointerParameter(IdentifierName(parameter.Type.Name)));
                            }

                            if (parameter.Declarator is PointerDeclarator { Declarator.Identifier: not null })
                            {
                                functionPointerType = functionPointerType
                                   .AddParameterListParameters(FunctionPointerParameter(PointerType(IdentifierName(parameter.Type.Name))));
                            }
                        }

                        functionPointerType = functionPointerType.AddParameterListParameters(FunctionPointerParameter(IdentifierName(field.Type.Name))); //Add return type.
                        structDeclaration = structDeclaration
                           .AddMembers(
                                FieldDeclaration(VariableDeclaration(functionPointerType))
                                   .AddModifiers(Token(SyntaxKind.PublicKeyword))
                                   .AddDeclarationVariables(VariableDeclarator(Identifier(fieldIdentifier3.Name)))
                            );
                    }

                    if (field.Declarator is PointerDeclarator { Declarator: FunctionDeclarator { ParameterList: { } parameterList2, Declarator: ParenthesizedDeclarator { Declarator: PointerDeclarator { Declarator.Identifier: { } fieldIdentifier4 } } } })
                    {
                        var functionPointerType = FunctionPointerType();
                        functionPointerType = functionPointerType
                           .WithCallingConvention(
                                FunctionPointerCallingConvention(Token(SyntaxKind.UnmanagedKeyword))
                                   .AddUnmanagedCallingConventionListCallingConventions(FunctionPointerUnmanagedCallingConvention(Identifier("Cdecl")))
                            );
                        foreach (var parameter in parameterList2.Parameters)
                        {
                            if (parameter.Declarator is { Identifier: not null })
                            {
                                functionPointerType = functionPointerType
                                   .AddParameterListParameters(FunctionPointerParameter(IdentifierName(parameter.Type.Name)));
                            }

                            if (parameter.Declarator is PointerDeclarator { Declarator.Identifier: not null })
                            {
                                functionPointerType = functionPointerType
                                   .AddParameterListParameters(FunctionPointerParameter(PointerType(IdentifierName(parameter.Type.Name))));
                            }
                        }

                        functionPointerType = functionPointerType.AddParameterListParameters(FunctionPointerParameter(PointerType(IdentifierName(field.Type.Name)))); //Add return type.
                        structDeclaration = structDeclaration
                           .AddMembers(
                                FieldDeclaration(VariableDeclaration(functionPointerType))
                                   .AddModifiers(Token(SyntaxKind.PublicKeyword))
                                   .AddDeclarationVariables(VariableDeclarator(Identifier(fieldIdentifier4.Name)))
                            );
                    }
                }

                yield return structDeclaration;
            }
        }
    }

    private IEnumerable<(TypeDefinition typeDefinition, TypeSpecifier typeSpecifier)> Visit(CSyntaxNode? node)
    {
        if (node is null)
        {
            yield break;
        }

        if (node is TypeDefinition typeDefinition)
        {
            foreach (var typeSpecifier in VisitTypeDefinition(typeDefinition))
            {
                yield return (typeDefinition, typeSpecifier);
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

    private IEnumerable<TypeSpecifier> VisitTypeDefinition(TypeDefinition? node)
    {
        if (node is null)
        {
            yield break;
        }

        foreach (var nodeChild in node.Children)
        {
            if (nodeChild is TypeSpecifier typeSpecifier)
            {
                yield return typeSpecifier;
            }
        }
    }
}
