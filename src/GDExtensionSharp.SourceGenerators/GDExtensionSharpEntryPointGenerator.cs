using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace GDExtensionSharp.SourceGenerators
{
    [Generator]

    internal class GDExtensionSharpEntryPointGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(initializationContext =>
            {
                initializationContext.AddSource("GDExtensionAttribute.g.cs", """
                    namespace GDExtensionSharp;

                    [System.AttributeUsage(AttributeTargets.Class)]
                    internal class GDExtensionAttribute : System.Attribute
                    {
                        public GDExtensionAttribute(string entryPoint) { }
                    }
                    """);
            });
        }
    }
}
