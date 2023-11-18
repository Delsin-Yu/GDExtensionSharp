using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api;

internal static partial class ApiGenerator
{
    private static IEnumerable<(string sourceName, string sourceContent)> GenerateBuiltinClasses(StringBuilder stringBuilder, BuiltinClass[] builtinClasses, BuiltinClassSize[] builtinClassSizes)
    {
        foreach (var builtinClass in builtinClasses)
        {
            if (builtinClass.Name.IsBuiltinType()) continue;

            var classBody =
                $$"""
                  namespace Godot;

                  public class {{builtinClass.Name}}
                  {
                  
                      private static MethodBindings _method_bindings;
                  
                      private unsafe struct MethodBindings
                      {
                          // ReSharper disable InconsistentNaming
                  {{GenerateBuiltinClassBindingStruct(stringBuilder, builtinClass).InsertIndentation(2)}}
                          // ReSharper restore InconsistentNaming
                      }

                  {{GenerateBuiltinClassEnums(stringBuilder, builtinClass.Enums).InsertIndentation()}}
                  
                      internal static unsafe void {{InitializeBindings}}()
                      {
                  {{GenerateBuiltinClassInitializeBindingMethod(stringBuilder, builtinClass).InsertIndentation(2)}}
                      }
                      
                      internal static unsafe void {{InitializeBindings_CtorDtor}}()
                      {
                      
                      }


                  }
                  """;


            yield return ($"Class.{builtinClass.Name}", classBody);
        }
    }

    #warning TODO: IMPL
    // private static string GenerateBuiltinClassConstructors(StringBuilder stringBuilder, BuiltinClass builtinClass)
    // {
    //     if (builtinClass.Constructors == null) return string.Empty;
    //
    //     foreach (var constructor in builtinClass.Constructors)
    //     {
    //         stringBuilder.Append($"public {builtinClass.Name}(");
    //
    //         if (constructor.Arguments != null)
    //         {
    //             stringBuilder.Append(MakeFunctionParameters(constructor.Arguments, includeDefault : false, forBuiltin : true));
    //         }
    //
    //         stringBuilder
    //            .AppendLine(")")
    //            .AppendLine("{");
    //
    //         stringBuilder
    //            .Append(Indents)
    //            .Append($"{MethodHelper}.{CallBuiltinConstructor}(_method_bindings.constructor_{constructor.Index.ToString(CultureInfo.InvariantCulture)}, &opaque");
    //     }
    // }

    private static string GenerateBuiltinClassEnums(StringBuilder stringBuilder, IReadOnlyList<BuiltinClassEnum> builtinClassEnums)
    {
        if (builtinClassEnums == null) return string.Empty;

        foreach (var builtinClassEnum in builtinClassEnums)
        {
            stringBuilder
               .AppendLine(
                    $$"""
                      public enum {{builtinClassEnum.Name}}
                      {
                      """
                );

            foreach (var valueElement in builtinClassEnum.Values)
            {
                stringBuilder.AppendLine($"    {valueElement.Name} = {valueElement.Value.ToString(CultureInfo.InvariantCulture)},");
            }

            stringBuilder
               .AppendLine("}");
        }

        return stringBuilder.ToStringAndClear();
    }
}
