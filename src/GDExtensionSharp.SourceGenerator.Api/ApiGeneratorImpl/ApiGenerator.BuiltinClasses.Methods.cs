using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private static readonly StringBuilder _classMethodsStringBuilder = new();
    private static readonly StringBuilder _classMethodsArgBuilder = new();

    private static string GenerateBuiltinClassMethodsAndMembers(StringBuilder stringBuilder, BuiltinClass builtinClass, long size)
    {
        if (builtinClass.Methods == null) return string.Empty;

        var methodList = new HashSet<string>();
        GenerateBuiltinClassMethods(stringBuilder, builtinClass, size, methodList);

        GenerateBuiltinClassMembers(stringBuilder, builtinClass, size, methodList);

        return stringBuilder.ToStringAndClear();
    }

    private static void GenerateBuiltinClassMethods(StringBuilder stringBuilder, BuiltinClass builtinClass, long size, HashSet<string> methodList)
    {
        foreach (var method in builtinClass.Methods)
        {
            methodList.Add(method.Name);

            const string returnValueVariableName = "returnValue";

            string signature = MakeSignature(builtinClass.Name, method, forBuiltin: true);

            if (method.IsVararg)
            {
                const string argListPointer = "argListPointer";

                bool hasExtraArguments = method.Arguments is {Length: > 0};

                BuildMethodBody(stringBuilder, size, signature.Replace(VarArgReplaceKey, string.Empty), method, returnValueVariableName, extraParams: "null");

                string argsParameter = hasExtraArguments ? ", params Variant[] args" : "params Variant[] args";

                BuildMethodBody(stringBuilder, size, signature.Replace(VarArgReplaceKey, argsParameter), method, returnValueVariableName,
                    $$"""
                    int argLength = args.Length;
                    Variant* {{argListPointer}} = (Variant*)global::System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(Variant) * argLength);
                    for(int index = 0; index < argLength; index++)
                    {
                    {{Indents}}{{argListPointer}}[index] = args[index];
                    }
                    """,
                    argListPointer
                    );

                for (int methodAlias = 1; methodAlias < 5 + 1; methodAlias++)
                {
                    _classMethodsArgBuilder
                        .Append($"Variant* {argListPointer} = stackalloc Variant[")
                        .Append(methodAlias)
                        .AppendLine("];");

                    if (hasExtraArguments)
                    {
                        _classMethodsStringBuilder
                            .Append(", ");
                    }

                    for (int i = 0; i < methodAlias; i++)
                    {
                        _classMethodsStringBuilder
                            .Append("Variant __arg")
                            .Append(i)
                            .Append(", ");

                        _classMethodsArgBuilder
                            .Append(argListPointer)
                            .Append('[')
                            .Append(i)
                            .Append(']')
                            .Append(" = __arg")
                            .Append(i)
                            .AppendLine(";");

                    }

                    _classMethodsArgBuilder.AppendLine();

                    _classMethodsStringBuilder.Remove(_classMethodsStringBuilder.Length - 2, 2);

                    string extraArg = _classMethodsStringBuilder.ToStringAndClear();
                    string extraBody = _classMethodsArgBuilder.ToStringAndClear();

                    BuildMethodBody(stringBuilder, size, signature.Replace(VarArgReplaceKey, extraArg), method, returnValueVariableName, extraBody, argListPointer,
                        $"global::System.Runtime.InteropServices.Marshal.FreeHGlobal((nint){argListPointer});");
                }

            }
            else
            {
                BuildMethodBody(stringBuilder, size, signature, method, returnValueVariableName);
            }
        }
    }

    private static void BuildMethodBody(StringBuilder stringBuilder, long size, string signature, BuiltinClassMethod method, string returnValueVariableName, string extraBody = null, string extraParams = null, string extraCleanupBody = null)
    {
        stringBuilder
            .AppendLine(signature)
            .AppendLine("{");

        if (!string.IsNullOrWhiteSpace(extraBody))
        {
            stringBuilder.AppendLine(extraBody.InsertIndentation());
        }

        if (!method.IsStatic)
        {
            stringBuilder.AppendLine(GenerateBufferToPointerCommand(size).InsertIndentation());
        }

        if (method.ReturnType != null)
        {
            string correctType = CorrectType(method.ReturnType);
            _classMethodsStringBuilder
                .Append($"{MethodHelper}.{CallBuiltinMethodPointerReturn}<{correctType}>(&{returnValueVariableName}, ");

            stringBuilder
                .Append(Indents)
                .AppendLine($"{correctType} {returnValueVariableName} = default;");
        }
        else
        {
            _classMethodsStringBuilder
                .Append($"{MethodHelper}.{CallBuiltinMethodPointerNoReturn}(");
        }

        _classMethodsStringBuilder.Append($"{BindingStructFieldName}.method_{method.Name}, ");

        if (method.IsStatic)
        {
            _classMethodsStringBuilder.Append("null");
        }
        else
        {
            _classMethodsStringBuilder.Append(OpaqueDataPointerName);
        }

        if (method.Arguments != null)
        {
            foreach (var argument in method.Arguments)
            {
                var (encode, argName) = GetEncodedArgs(argument.Name, argument.Type, argument.Meta);

                if (!string.IsNullOrWhiteSpace(encode))
                {
                    stringBuilder
                        .Append(Indents)
                        .AppendLine(encode);
                }

                _classMethodsStringBuilder
                    .Append(", ")
                    .Append(argName);
            }
        }

        if (!string.IsNullOrWhiteSpace(extraParams))
        {
            _classMethodsStringBuilder
                .Append(", ")
                .Append(extraParams);
        }

        _classMethodsStringBuilder.Append(");");


        stringBuilder
            .Append(Indents)
            .AppendLine(_classMethodsStringBuilder.ToStringAndClear());

        if (!method.IsStatic)
        {
            stringBuilder
                .AppendLine(GeneratePointerToBufferCommand(size).InsertIndentation());
        }

        if (!string.IsNullOrWhiteSpace(extraCleanupBody))
        {
            stringBuilder.AppendLine(extraCleanupBody.InsertIndentation());
        }

        if (method.ReturnType != null)
        {
            stringBuilder
                .Append(Indents)
                .AppendLine($"return {returnValueVariableName};");
        }

        stringBuilder
            .AppendLine("}")
            .AppendLine();
    }

    private static void GenerateBuiltinClassMembers(StringBuilder stringBuilder, BuiltinClass builtinClass, long classSize, HashSet<string> methodList)
    {
        if(builtinClass.Members == null) return;

        foreach (var member in builtinClass.Members)
        {
            bool hasGet = !methodList.Contains($"get_{member.Name}");
            bool hasSet = !methodList.Contains($"set_{member.Name}");

            if(!hasGet && !hasSet) continue;

            string correctedMemberType = CorrectType(member.Type);
            stringBuilder
                .AppendLine($"{MethodBindingModifier} unsafe {correctedMemberType} {member.Name.EscapeContextualKeyWord().SnakeCaseToPascalCase()}")
                .AppendLine("{");

            if (hasGet)
            {
                stringBuilder
                    .Append(Indents)
                    .AppendLine("get")
                    .Append(Indents)
                    .AppendLine("{")
                    .AppendLine(GenerateBufferToPointerCommand(classSize).InsertIndentation(2))
                    .Append(Indents)
                    .Append(Indents)
                    .AppendLine($"var returnValue = {MethodHelper}.{CallBuiltinPointerGetter}<{correctedMemberType}>({BindingStructFieldName}.member_{member.Name}_getter, {OpaqueDataPointerName});")
                    .AppendLine(GeneratePointerToBufferCommand(classSize).InsertIndentation(2))
                    .Append(Indents)
                    .Append(Indents)
                    .AppendLine("return returnValue;")
                    .Append(Indents)
                    .AppendLine("}");
            }

            if (hasSet)
            {
                var (encode, argName) = GetEncodedArgs("value", member.Type, null);
                stringBuilder
                    .Append(Indents)
                    .AppendLine("set")
                    .Append(Indents)
                    .AppendLine("{")
                    .AppendLine(GenerateBufferToPointerCommand(classSize).InsertIndentation(2))
                    .Append(Indents)
                    .Append(Indents)
                    .AppendLine(encode)
                    .Append(Indents)
                    .Append(Indents)
                    .AppendLine($"_method_bindings.member_{member.Name}_setter({OpaqueDataPointerName}, {argName});")
                    .AppendLine(GeneratePointerToBufferCommand(classSize).InsertIndentation(2))
                    .Append(Indents)
                    .AppendLine("}");
            }

            stringBuilder
                .AppendLine("}");
        }
    }
}
