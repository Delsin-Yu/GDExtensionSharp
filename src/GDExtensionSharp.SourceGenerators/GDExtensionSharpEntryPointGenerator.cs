using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDExtensionSharp.SourceGenerators;

[Generator]

internal class GDExtensionSharpEntryPointGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        const string GDExtensionAttributeName = "GDExtensionSharp.GDExtensionAttribute";

        var source1 = context.SyntaxProvider.CreateSyntaxProvider((node, _) => node is ClassDeclarationSyntax,
            (syntaxContext, _) => syntaxContext);
        var source2 = context.SyntaxProvider.ForAttributeWithMetadataName(GDExtensionAttributeName,
            (node, _) => node is ClassDeclarationSyntax, (syntaxContext, _) => syntaxContext)
            .Combine(source1.Collect());

        context.RegisterSourceOutput(source2, (spc, input) =>
        {
            if (input.Left.TargetNode is not ClassDeclarationSyntax targetClassDeclaration)
            {
                return;
            }

            if (input.Left.TargetSymbol is not INamedTypeSymbol targetSymbol)
            {
                return;
            }

            if (input.Left.SemanticModel.Compilation.GetTypeByMetadataName("GDExtensionSharp.ExtensionInitializer") is not { DeclaredAccessibility: Accessibility.Public, Constructors: [var extensionInitializerConstructor] } extensionInitializer)
            {
                // GDExtensionSharp doesn't exist, maybe a diagnostic.
                return;
            }

            if (input.Left.Attributes is not [{ NamedArguments: [{ Key: "EntryPoint", Value.Value: string entryPointName }] }])
            {
                entryPointName = "gdextension_csharp_init";
            }

            var compilationUnit = CompilationUnit();
            compilationUnit = compilationUnit.If(!targetSymbol.ContainingNamespace.IsGlobalNamespace,
                _ => _.AddMembers(
                    FileScopedNamespaceDeclaration(IdentifierName(targetSymbol.ContainingNamespace.ToString()))));
            var classDeclaration = ClassDeclaration(targetClassDeclaration.Identifier)
                .WithModifiers(targetClassDeclaration.Modifiers);
            var unmanagedCallersOnlyAttributeList = AttributeList().AddAttributes(
                Attribute(IdentifierName("global::System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute"))
                    .AddArgumentListArguments(
                        AttributeArgument(
                                LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(entryPointName)))
                            .WithNameEquals(NameEquals("EntryPoint"))));
            var initializeMethodDeclaration =
                MethodDeclaration(PredefinedType(Token(SyntaxKind.ByteKeyword)), "Initialize")
                    .AddAttributeLists(unmanagedCallersOnlyAttributeList)
                    .AddModifiers(Token(SyntaxKind.StaticKeyword))
                    .AddParameterListParameters(extensionInitializerConstructor.Parameters.Select(_ =>
                        Parameter(Identifier(_.Name.ToString())).WithType(IdentifierName(_.Type.ToString()))).ToArray())
                    .AddBodyStatements(LocalDeclarationStatement(VariableDeclaration(IdentifierName("var"))
                        .AddVariables(VariableDeclarator("extensionInitializer")
                            .WithInitializer(EqualsValueClause(
                                ObjectCreationExpression(IdentifierName(extensionInitializer.ToString()))
                                    .AddArgumentListArguments(extensionInitializerConstructor.Parameters
                                        .Select(_ => Argument(IdentifierName(_.Name.ToString()))).ToArray()))))),
                        ReturnStatement(InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("extensionInitializer"), IdentifierName("Initialize")))));
            classDeclaration = classDeclaration.AddMembers(initializeMethodDeclaration);
            foreach (GeneratorSyntaxContext generatorSyntaxContext in input.Right)
            {

            }
            compilationUnit = compilationUnit.AddMembers(classDeclaration);
            spc.AddSource($"{targetClassDeclaration.Identifier.Text}.g.cs",
                compilationUnit.NormalizeWhitespace().GetText(Encoding.UTF8, SourceHashAlgorithm.Sha256));

        });

        context.RegisterPostInitializationOutput(initializationContext =>
        {
            initializationContext.AddSource("GDExtensionAttribute.g.cs", """
                    namespace GDExtensionSharp;

                    [System.AttributeUsage(AttributeTargets.Class)]
                    internal class GDExtensionAttribute : System.Attribute
                    {
                        public GDExtensionAttribute() { }

                        public string EntryPoint { get; set; }
                    }
                    """);
        });
    }
}
