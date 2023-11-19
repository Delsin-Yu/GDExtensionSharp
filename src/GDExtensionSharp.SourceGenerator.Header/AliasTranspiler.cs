using System;
using System.Collections.Generic;
using System.Text;
using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDExtensionSharp.SourceGenerator.Header;
internal class AliasTranspiler
{
    private class TypeForwarder
    {
        public TypeForwarder(IDictionary<string, TypeSyntax> aliases) => _aliases = new(aliases);

        private readonly Dictionary<string, TypeSyntax> _aliases;

        public void Add(string alias, TypeSyntax value) => _aliases[alias] = value;

        public TypeSyntax Resolve(string alias)
        {
            TypeSyntax ret = IdentifierName(alias);
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

            return ret;
        }
    }

    public IEnumerable<UsingDirectiveSyntax> Transpile(CSyntaxNode node)
    {
        var typeForwarder = new TypeForwarder(new Dictionary<string, TypeSyntax>()
                                              {
                                                  {"int",PredefinedType(Token(SyntaxKind.IntKeyword))},
                                                  {"void",PredefinedType(Token(SyntaxKind.VoidKeyword))},
                                                  {"char",PredefinedType(Token(SyntaxKind.CharKeyword))},
                                                  {"size_t",PredefinedType(Token(SyntaxKind.ULongKeyword))},
                                                  {"int32_t",PredefinedType(Token(SyntaxKind.IntKeyword))},
                                                  {"uint32_t",PredefinedType(Token(SyntaxKind.UIntKeyword))},
                                                  {"uint16_t",PredefinedType(Token(SyntaxKind.UShortKeyword))},
                                                  {"int64_t",PredefinedType(Token(SyntaxKind.LongKeyword))},
                                                  {"uint8_t",PredefinedType(Token(SyntaxKind.ByteKeyword))},
                                                  {"uint64_t",PredefinedType(Token(SyntaxKind.ULongKeyword))},
                                                  {"wchar_t",PredefinedType(Token(SyntaxKind.UShortKeyword))},
                                                  {"float",PredefinedType(Token(SyntaxKind.FloatKeyword))},
                                                  {"double",PredefinedType(Token(SyntaxKind.DoubleKeyword))}
                                              });

        foreach (var (typeDefinition, typeSpecifier) in Visit(node))
        {
            if (typeSpecifier.Type is { } right1 && typeDefinition.Declarators is [{ Identifier: { } left1 }])
            {
                typeForwarder.Add(left1.Name, IdentifierName(right1.Name));
                yield return UsingDirective(NameEquals(left1.Name), typeForwarder.Resolve(left1.Name))
                   .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword));

            }

            if (typeSpecifier.Type is { } right2 && typeDefinition.Declarators is [PointerDeclarator { Declarator: { Identifier: { } left2 } }])
            {
                typeForwarder.Add(left2.Name, PointerType(IdentifierName(right2.Name)));
                yield return UsingDirective(NameEquals(left2.Name), typeForwarder.Resolve(left2.Name))
                            .WithUnsafeKeyword(Token(SyntaxKind.UnsafeKeyword))
                            .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword));

            }

            if (typeSpecifier.Type is { } right3 && typeDefinition.Declarators is [FunctionDeclarator { ParameterList: { } parameterList1, Declarator: ParenthesizedDeclarator { Declarator: PointerDeclarator { Declarator.Identifier: { } left3 } } }])
            {
                var functionPointerType = FunctionPointerType();
                foreach (var parameter in parameterList1.Parameters)
                {
                    if (parameter.Declarator is { Identifier: not null })
                    {
                        var resolvedType = typeForwarder.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }
                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(resolvedType));
                    }
                    if (parameter.Declarator is PointerDeclarator { Declarator.Identifier: not null })
                    {
                        var resolvedType = typeForwarder.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }
                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(PointerType(resolvedType)));
                    }
                }
                var resolvedReturnType = typeForwarder.Resolve(right3.Name);
                if (resolvedReturnType is IdentifierNameSyntax returnIdentifierName)
                {
                    resolvedReturnType = IdentifierName($"global::GDExtensionSharp.{returnIdentifierName.Identifier.Text}");
                }
                functionPointerType = functionPointerType.AddParameterListParameters(FunctionPointerParameter(resolvedReturnType)); //Add return type.
                typeForwarder.Add(left3.Name, functionPointerType);
                yield return UsingDirective(NameEquals(left3.Name), typeForwarder.Resolve(left3.Name))
                            .WithUnsafeKeyword(Token(SyntaxKind.UnsafeKeyword))
                            .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword));
            }

            if (typeSpecifier.Type is { } right4 && typeDefinition.Declarators is [PointerDeclarator { Declarator: FunctionDeclarator { ParameterList: { } parameterList2, Declarator: ParenthesizedDeclarator { Declarator: PointerDeclarator { Declarator.Identifier: { } left4 } } } }])
            {
                var functionPointerType = FunctionPointerType();
                foreach (var parameter in parameterList2.Parameters)
                {
                    if (parameter.Declarator is { Identifier: not null })
                    {
                        var resolvedType = typeForwarder.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }
                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(resolvedType));
                    }
                    if (parameter.Declarator is PointerDeclarator { Declarator.Identifier: not null })
                    {
                        var resolvedType = typeForwarder.Resolve(parameter.Type.Name);
                        if (resolvedType is IdentifierNameSyntax identifierName)
                        {
                            resolvedType = IdentifierName($"global::GDExtensionSharp.{identifierName.Identifier.Text}");
                        }
                        functionPointerType = functionPointerType
                           .AddParameterListParameters(FunctionPointerParameter(PointerType(resolvedType)));
                    }
                }
                var resolvedReturnType = typeForwarder.Resolve(right4.Name);
                if (resolvedReturnType is IdentifierNameSyntax returnIdentifierName)
                {
                    resolvedReturnType = IdentifierName($"global::GDExtensionSharp.{returnIdentifierName.Identifier.Text}");
                }
                functionPointerType = functionPointerType.AddParameterListParameters(FunctionPointerParameter(PointerType(resolvedReturnType))); //Add return type.
                typeForwarder.Add(left4.Name, functionPointerType);
                yield return UsingDirective(NameEquals(left4.Name), typeForwarder.Resolve(left4.Name))
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
