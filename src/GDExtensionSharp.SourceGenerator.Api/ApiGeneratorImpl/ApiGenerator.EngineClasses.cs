using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static IEnumerable<(string sourceName, string sourceContent)> GenerateEngineClasses(StringBuilder stringBuilder, IReadOnlyList<Class> classes)
    {
        var godotTypes = new HashSet<string>(classes.Where(x => !x.IsRefcounted).Select(x => x.Name));
        godotTypes.Remove("Object");
        godotTypes.UnionWith(_interopHandler.Keys);
        var refCountedGodotTypes = new HashSet<string>(classes.Where(x => x.IsRefcounted).Select(x => x.Name));

        HashSet<string> virtualMethods = [];
        HashSet<string> propertyNames = [];
        HashSet<string> usedPropertyAccessor = [];
        Dictionary<string, string> methodReturnTypeMap = [];

        foreach (Class engineClass in classes)
        {
            string engineClassName = CorrectType(engineClass.Name);
            string engineClassInherits = CorrectType(engineClass.Inherits);

            if (string.IsNullOrWhiteSpace(engineClassInherits)) engineClassInherits = null;

            stringBuilder
                .AppendIndentLine("""
                                  using System.Diagnostics;
                                  using System.Runtime.InteropServices;
                                  using Godot.NativeInterop;
                                  """)
                .AppendIndentLine()
                .AppendIndentLine(NamespaceHeader)
                .AppendIndentLine();

            stringBuilder
                .AppendIndentLine(engineClassInherits != null ? $"public partial class {engineClassName} : {engineClassInherits}" : $"public partial class {engineClassName}")
                .AppendIndentLine("{");


            using (stringBuilder.AddIndent())
            {
                ResolveMethodInfo(methodReturnTypeMap,
                    virtualMethods,
                    propertyNames,
                    engineClass.Methods,
                    engineClass.Properties,
                    godotTypes,
                    refCountedGodotTypes);

                GenerateEnum(stringBuilder, propertyNames, engineClass.Enums);

                GenerateProperty(stringBuilder,
                    engineClass.Properties,
                    godotTypes,
                    refCountedGodotTypes,
                    usedPropertyAccessor,
                    virtualMethods,
                    methodReturnTypeMap);

                stringBuilder
                    .AppendIndentLine($"private static readonly StringName NativeName = \"{engineClassName}\";")
                    .AppendIndentLine();

                GenerateClassHeader(stringBuilder, engineClassInherits != null, engineClassName, engineClass);

                GenerateClassMethod(stringBuilder, engineClassName, engineClass.Methods, godotTypes, refCountedGodotTypes, usedPropertyAccessor);

                GenerateClassSignals(stringBuilder, engineClass.Signals, godotTypes, refCountedGodotTypes);

                GenerateClassEntry("PropertyName", stringBuilder, engineClassInherits, engineClass.Properties?.Select(x => x.Name));

                GenerateClassEntry("MethodName", stringBuilder, engineClassInherits, engineClass.Methods?.Select(x => (x.IsVirtual ? "_" : string.Empty) + x.Name));

                GenerateClassEntry("SignalName", stringBuilder, engineClassInherits, engineClass.Signals?.Select(x => x.Name));
            }


            stringBuilder
                .AppendLine()
                .AppendLine("}");

            yield return ($"EngineClass.{engineClassName}", stringBuilder.ToStringAndClear());

            usedPropertyAccessor.Clear();
            virtualMethods.Clear();
            methodReturnTypeMap.Clear();
            propertyNames.Clear();
        }
    }

    private static void ResolveMethodInfo(
        IDictionary<string, string> methodReturnTypeMap,
        ICollection<string> virtualMethods,
        ICollection<string> propertyNames,
        IReadOnlyList<ClassMethod> classMethods,
        IReadOnlyList<Property> classProperties,
        IReadOnlyCollection<string> godotTypes,
        IReadOnlyCollection<string> refCountedGodotTypes)
    {
        if (classMethods != null)
        {
            foreach (ClassMethod method in classMethods)
            {
                string returnType;
                if (method.ReturnValue == null)
                {
                    returnType = "void";
                }
                else
                {
                    returnType = GetInteropHandler(CorrectType(method.ReturnValue.Type), godotTypes, refCountedGodotTypes).CsharpType;
                }

                methodReturnTypeMap[method.Name] = returnType;
                if (method.IsVirtual) virtualMethods.Add(method.Name);
            }
        }

        if (classProperties != null)
        {
            foreach (Property property in classProperties)
            {
                propertyNames.Add(property.Name.SnakeCaseToPascalCase());
            }
        }
    }
}
