using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static readonly StringBuilder _methodCallerBuilder = new();

    private static string GenerateBuiltinClassCtor(StringBuilder stringBuilder, BuiltinClass builtinClass)
    {
        if (builtinClass.Constructors == null) return string.Empty;

        // TODO: SWAP??
        // var copyCtorIndex = -1L;

        foreach (var constructor in builtinClass.Constructors)
        {
            stringBuilder
                .Append($"{MethodBindingModifier} unsafe ")
                .Append(builtinClass.Name)
                .Append('(')
                .Append(MakeFunctionParameters(constructor.Arguments, false, true))
                .AppendLine(")")
                .AppendLine("{");

            _methodCallerBuilder
                .Append(MethodHelper)
                .Append('.')
                .Append(CallBuiltinConstructor)
                .Append("(_method_bindings.constructor_")
                .Append(constructor.Index)
                .Append($", &{OpaqueFieldName}");

            if (constructor.Arguments != null)
            {
                // if (constructor.Arguments.Length == 1 && constructor.Arguments[0].Type == builtinClass.IndexingReturnType)
                // {
                //     copyCtorIndex = constructor.Index;
                // }

                foreach (var argument in constructor.Arguments)
                {
                    var (encode, argName) = GetEncodedArgs(argument.Name, argument.Type, argument.Meta);

                    if (!string.IsNullOrWhiteSpace(encode))
                    {
                        stringBuilder
                            .AppendLine(encode.InsertIndentation());
                    }

                    _methodCallerBuilder
                        .Append(", (void*)")
                        .Append(argName);
                }
            }

            stringBuilder
                .Append(Indents)
                .Append(_methodCallerBuilder)
                .Append(");")
                .AppendLine()
                .AppendLine("}");

            _methodCallerBuilder.Clear();
        }

        return stringBuilder.ToStringAndClear();
    }

    private static string GenerateBuiltinClassDtor(BuiltinClass builtinClass)
    {
        if (!builtinClass.HasDestructor) return string.Empty;

        return $"unsafe ~{builtinClass.Name}() => _method_bindings.destructor(&{OpaqueFieldName});";
    }
}
