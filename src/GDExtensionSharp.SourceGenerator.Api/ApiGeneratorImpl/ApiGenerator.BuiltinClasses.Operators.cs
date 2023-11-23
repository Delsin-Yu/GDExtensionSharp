using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static string GenerateBuiltinClassOperators(StringBuilder stringBuilder, BuiltinClass builtinClass, Operator[] builtinClassOperators, long classSize)
    {
        if (builtinClassOperators == null) return string.Empty;

        foreach (Operator @operator in builtinClassOperators)
        {
            if (@operator.Name is "in" or "xor") continue;

            string correctedReturnType = CorrectType(@operator.ReturnType);


            if(@operator.Name == "not")
            {
                stringBuilder
                    .AppendLine($"public unsafe {GetGDExtensionType(builtinClass.Name)} Inverted()")
                    .AppendLine("{")
                    .AppendLine(GenerateBufferToPointerCommand(classSize).InsertIndentation())
                    .Append(Indents)
                    .AppendLine($"var returnValue = {MethodHelper}.{CallBuiltinOperatorPointer}<{GetGDExtensionType(builtinClass.Name)}>({BindingStructFieldName}.operator_{@operator.Name.OperatorIdToName()}, {OpaqueDataPointerName}, null);")
                    .AppendLine(GeneratePointerToBufferCommand(classSize).InsertIndentation())
                    .Append(Indents)
                    .AppendLine($"return {MethodHelper}.{StaticCast}(returnValue);")
                    .AppendLine("}");
                continue;
            }

            stringBuilder.Append("public static unsafe ");

            if (string.IsNullOrWhiteSpace(@operator.RightType))
            {
                stringBuilder
                    .AppendLine($"implicit operator {correctedReturnType} {@operator.Name.Replace("unary", "")}()")
                    .AppendLine("{")
                    .AppendLine(GenerateBufferToPointerCommand(classSize, "self").InsertIndentation())
                    .Append(Indents)
                    .AppendLine($"var returnValue = {MethodHelper}.{CallBuiltinOperatorPointer}<{GetGDExtensionType(correctedReturnType)}>({BindingStructFieldName}.operator_{@operator.Name.OperatorIdToName()}, {OpaqueDataPointerName}, null);")
                    .Append(Indents)
                    .AppendLine($"return {MethodHelper}.{StaticCast}(returnValue);")
                    .AppendLine("}");
            }
            else
            {
                var (encode, argName) = GetEncodedArgs("other", @operator.RightType, null);

                stringBuilder
                    .AppendLine($"{correctedReturnType} operator {@operator.Name}({GetTypeFromParameter(builtinClass.Name)}self, {GetTypeFromParameter(@operator.RightType)}other)")
                    .AppendLine("{");

                if (!string.IsNullOrWhiteSpace(encode))
                {
                    stringBuilder
                        .AppendLine(encode);
                }

                stringBuilder
                    .AppendLine(GenerateBufferToPointerCommand(classSize, "self").InsertIndentation(1))
                    .Append(Indents)
                    .AppendLine($"var returnValue = {MethodHelper}.{CallBuiltinOperatorPointer}<{GetGDExtensionType(correctedReturnType)}>({BindingStructFieldName}.operator_{@operator.Name.OperatorIdToName()}_{@operator.RightType}, {OpaqueDataPointerName}, {argName});")
                    .Append(Indents)
                    .AppendLine($"return {MethodHelper}.{StaticCast}(returnValue);")
                    .AppendLine("}");
            }
        }

        return stringBuilder.ToStringAndClear();
    }
}
