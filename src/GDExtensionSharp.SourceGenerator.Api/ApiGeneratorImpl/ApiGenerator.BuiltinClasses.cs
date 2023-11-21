using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static IEnumerable<(string sourceName, string sourceContent)> GenerateBuiltinClasses( StringBuilder stringBuilder, BuiltinClass[] builtinClasses, Dictionary<string, long> builtinClassSizes)
    {
        foreach (var builtinClass in builtinClasses)
        {
            if (builtinClass.Name.IsBuiltinType()) continue;

            if (!builtinClassSizes.TryGetValue(builtinClass.Name, out var classSize))
            {
                continue;
            }

            string enumTypeName = $"GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE{builtinClass.Name.PascalCaseToSnakeCase().ToUpper()}";

            string classBody =
                $$"""
                  {{NamespaceHeader}}

                  public class {{builtinClass.Name}}
                  {
                      private static MethodBindings {{BindingStructFieldName}};

                      private unsafe struct MethodBindings
                      {
                          // ReSharper disable InconsistentNaming
                  {{GenerateBuiltinClassBindingStruct(stringBuilder, builtinClass).InsertIndentation(2)}}
                          // ReSharper restore InconsistentNaming
                      }

                  {{GenerateBuiltinClassOpaqueData(classSize).InsertIndentation()}}

                  {{GenerateBuiltinClassCtor(stringBuilder, builtinClass, classSize).InsertIndentation()}}

                  {{GenerateBuiltinClassDtor(builtinClass, classSize).InsertIndentation()}}

                  {{GenerateBuiltinClassMethodsAndMembers(stringBuilder, builtinClass, classSize).InsertIndentation()}}

                  {{GenerateBuiltinClassEnums(stringBuilder, builtinClass.Enums).InsertIndentation()}}

                      internal static unsafe void {{InitializeBindings}}()
                      {
                  {{GenerateBuiltinClassInitializeBindingMethod(enumTypeName, stringBuilder, builtinClass).InsertIndentation(2)}}
                      }

                      internal static unsafe void {{InitializeBindings_CtorDtor}}()
                      {
                  {{GenerateBuiltinClassInitializeCtorDtorMethod(enumTypeName, stringBuilder, builtinClass).InsertIndentation(2)}}
                      }


                  }
                  """;


            yield return ($"Class.{builtinClass.Name}", classBody);
        }
    }
}
