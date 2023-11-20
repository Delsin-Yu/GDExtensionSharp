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
                initializationContext.AddSource("GDExtensionSharpEntryPointAttribute.g.cs", """
                    namespace GDExtensionSharp;

                    [System.AttributeUsage(AttributeTargets.Class)]
                    internal class GDExtensionSharpEntryPointAttribute : System.Attribute
                    {
                        public GDExtensionSharpEntryPointAttribute(string name) { }
                    }
                    """);
            });
        }
    }
}
