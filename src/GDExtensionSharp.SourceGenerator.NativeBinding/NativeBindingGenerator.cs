using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GDExtensionSharp.SourceGenerator.NativeBinding
{
    [Generator(LanguageNames.CSharp)]
    public class NativeBindingGenerator : IIncrementalGenerator
    {
        private const string Indents = "    ";
        private const string MethodTableAccess = "global::GDExtensionSharp.GDExtensionSharpBinding.MethodTable";

        private const string GenerateUnmanagedBindingAttributeName = "GDExtensionSharp.GenerateUnmanagedBindingAttribute";
        private const string GDExtensionMethodAttributeName = "GDExtensionMethodAttribute";
        private const string GDExtensionNotSupportedAttributeName = "GDExtensionNotSupportedAttribute";
        private const string MarshalToBytePtrAttributeName = "MarshalToBytePtrAttribute";

        public void Initialize(IncrementalGeneratorInitializationContext initializationContext)
        {
            var source =
                initializationContext
                    .SyntaxProvider
                    .ForAttributeWithMetadataName(GenerateUnmanagedBindingAttributeName, (node, token) => node is ClassDeclarationSyntax, (context, token) => context);

            initializationContext
                .RegisterSourceOutput(source,
                    GenerateCore);
        }

        private static void GenerateCore(SourceProductionContext context, GeneratorAttributeSyntaxContext syntaxContext)
        {
            var builder = new StringBuilder();

            var symbol = (INamedTypeSymbol)syntaxContext.TargetSymbol;

            if (!symbol.ContainingNamespace.IsGlobalNamespace)
            {
                builder
                    .Append("namespace ")
                    .Append(symbol.ContainingNamespace.ToDisplayString())
                    .AppendLine(";")
                    .AppendLine();
            }

            builder
                .Append("unsafe partial class ")
                .AppendLine(symbol.Name)
                .AppendLine("{");

            foreach (ISymbol member in symbol.GetMembers())
            {
                if (member is not IMethodSymbol methodSymbol) continue;

                var attribute = methodSymbol.GetAttributes().FirstOrDefault(data => data.AttributeClass?.Name == GDExtensionMethodAttributeName);
                var notSupportedAttribute = methodSymbol.GetAttributes().FirstOrDefault(data => data.AttributeClass?.Name == GDExtensionNotSupportedAttributeName);

                if (notSupportedAttribute == null && attribute == null)
                {
                    continue;
                }

                builder
                        .Append(Indents)
                        .Append(methodSymbol.DeclaredAccessibility.ToString().ToLowerInvariant())
                        .Append(" static partial ")
                        .Append(methodSymbol.ReturnType.ToDisplayString())
                        .Append(" ")
                        .Append(methodSymbol.Name)
                        .Append('(');

                foreach (IParameterSymbol parameter in methodSymbol.Parameters)
                {
                    builder
                        .Append(parameter.ToDisplayString())
                        .Append(", ");
                }

                if (methodSymbol.Parameters.Length > 0)
                {
                    builder.Remove(builder.Length - 2, 2);
                }

                builder
                    .Append(")");


                if (notSupportedAttribute != null)
                {
                    builder
                        .AppendLine(" =>")
                        .Append(Indents)
                        .Append(Indents)
                        .AppendLine($"throw new global::System.NotSupportedException(\"{methodSymbol.Name}\");")
                        .AppendLine();
                    continue;
                }

                builder
                    .AppendLine()
                    .Append(Indents)
                    .AppendLine("{");

                string attributeContent = attribute.ConstructorArguments[0].Value?.ToString();

                string[] methodParameters = new string[methodSymbol.Parameters.Length];

                for (int i = 0; i < methodParameters.Length; i++)
                {
                    var originalParameter = methodSymbol.Parameters[i];

                    string originalParameterName = originalParameter.Name;
                    if (IsNativeCSharpBaseType(originalParameter.Type))
                    {
                        methodParameters[i] = originalParameterName;
                    }
                    else if (originalParameter.Type.Name == "godot_bool")
                    {
                        methodParameters[i] = $"(byte){originalParameterName}";
                    }
                    else if (originalParameter.GetAttributes().Any(x => x.AttributeClass?.Name == MarshalToBytePtrAttributeName))
                    {
                        methodParameters[i] = $"AsByte({originalParameterName})";
                    }
                    else
                    {
                        string methodParameterName = $"pointer_{originalParameterName}";
                        builder
                            .Append(Indents)
                            .Append(Indents)
                            .Append("var ")
                            .Append(methodParameterName)
                            .Append(" = ")
                            .AppendLine($"CustomUnsafe.ReadOnlyRefAsPointer(in {originalParameterName});");

                        methodParameters[i] = $"{methodParameterName}";
                    }
                }

                builder
                    .Append(Indents)
                    .Append(Indents);

                if (!methodSymbol.ReturnsVoid)
                {
                    builder
                        .Append("var returnValue = ");
                }

                builder
                    .Append(MethodTableAccess)
                    .Append('.')
                    .Append(attributeContent)
                    .Append('(');

                foreach (string parameter in methodParameters)
                {
                    builder
                        .Append(parameter)
                        .Append(", ");
                }

                if (methodParameters.Length > 0)
                {
                    builder.Remove(builder.Length - 2, 2);
                }


                builder.AppendLine(");");

                if (!methodSymbol.ReturnsVoid)
                {
                    builder
                        .Append(Indents)
                        .Append(Indents)
                        .AppendLine($"return ({methodSymbol.ReturnType.ToDisplayString()})returnValue;");
                }

                builder
                        .Append(Indents)
                        .AppendLine("}");
            }

            builder
                .AppendLine("}")
                .AppendLine();

            context.AddSource("NativeBinding.g.cs", builder.ToString());
        }

        static bool IsNativeCSharpBaseType(ITypeSymbol typeSymbol) => typeSymbol.SpecialType switch
        {
            SpecialType.System_Char => true,
            SpecialType.System_SByte => true,
            SpecialType.System_Byte => true,
            SpecialType.System_Int16 => true,
            SpecialType.System_UInt16 => true,
            SpecialType.System_Int32 => true,
            SpecialType.System_UInt32 => true,
            SpecialType.System_Int64 => true,
            SpecialType.System_UInt64 => true,
            SpecialType.System_Decimal => true,
            SpecialType.System_Single => true,
            SpecialType.System_Double => true,
            SpecialType.System_IntPtr => true,
            SpecialType.System_UIntPtr => true,
            _ => false
        };
    }
}
