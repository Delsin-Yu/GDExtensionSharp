using System;
using System.Collections.Generic;
using System.Text;
using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDExtensionSharp.SourceGenerator.Header;

internal partial class AliasTranspiler
{
    private readonly TypeForwardResolver _typeForwardResolver;
    public AliasTranspiler(TypeForwardResolver typeForwardResolver) => _typeForwardResolver = typeForwardResolver;

    public IEnumerable<UsingDirectiveSyntax> Transpile(CSyntaxNode node)
    {
        foreach (var (typeDefinition, typeSpecifier) in Visit(node))
        {
            if (typeSpecifier.Type is { } right1 && typeDefinition.Declarators is [{ Identifier: { } left1 }])
            {
                _typeForwardResolver.Add(left1.Name, IdentifierName(right1.Name));
                yield return UsingDirective(NameEquals(left1.Name), _typeForwardResolver.Resolve(left1.Name))
                   .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword));
            }

            if (typeSpecifier.Type is { } right2 && typeDefinition.Declarators is [PointerDeclarator { Declarator: { Identifier: { } left2 } }])
            {
                _typeForwardResolver.Add(left2.Name, PointerType(IdentifierName(right2.Name)));
                yield return UsingDirective(NameEquals(left2.Name), _typeForwardResolver.Resolve(left2.Name))
                            .WithUnsafeKeyword(Token(SyntaxKind.UnsafeKeyword))
                            .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword));
            }

            if (typeSpecifier.Type is { } right3 && typeDefinition.Declarators is [FunctionDeclarator { ParameterList: { } parameterList1, Declarator: ParenthesizedDeclarator { Declarator: PointerDeclarator { Declarator.Identifier: { } left3 } } }])
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
                        var resolvedType = _typeForwardResolver.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }

                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(resolvedType));
                    }

                    if (parameter.Declarator is PointerDeclarator { Declarator.Identifier: not null })
                    {
                        var resolvedType = _typeForwardResolver.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }

                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(PointerType(resolvedType)));
                    }
                }
                functionPointerType = functionPointerType.AddParameterListParameters(FunctionPointerParameter(_typeForwardResolver.Resolve(right3.Name)));
                _typeForwardResolver.Add(left3.Name, functionPointerType);
                yield return UsingDirective(NameEquals(left3.Name), _typeForwardResolver.Resolve(left3.Name))
                            .WithUnsafeKeyword(Token(SyntaxKind.UnsafeKeyword))
                            .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword));
            }

            if (typeSpecifier.Type is { } right4 && typeDefinition.Declarators is [PointerDeclarator { Declarator: FunctionDeclarator { ParameterList: { } parameterList2, Declarator: ParenthesizedDeclarator { Declarator: PointerDeclarator { Declarator.Identifier: { } left4 } } } }])
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
                        var resolvedType = _typeForwardResolver.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }

                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(resolvedType));
                    }

                    if (parameter.Declarator is PointerDeclarator { Declarator.Identifier: not null })
                    {
                        var resolvedType = _typeForwardResolver.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }

                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(PointerType(resolvedType)));
                    }
                }

                functionPointerType = functionPointerType.AddParameterListParameters(FunctionPointerParameter(PointerType(_typeForwardResolver.Resolve(right4.Name))));
                _typeForwardResolver.Add(left4.Name, functionPointerType);
                yield return UsingDirective(NameEquals(left4.Name), _typeForwardResolver.Resolve(left4.Name))
                            .WithUnsafeKeyword(Token(SyntaxKind.UnsafeKeyword))
                            .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword));
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
