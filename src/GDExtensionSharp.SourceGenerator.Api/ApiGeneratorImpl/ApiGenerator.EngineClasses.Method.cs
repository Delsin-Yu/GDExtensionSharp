using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string ReturnVariableName = "ret";

    private enum GodotTypeKind
    {
        NotGodotType,
        GodotType,
        RefCountedType
    }

    private static IInteropHandler GetHandler(string type, string fallback)
    {
        if (!_interopHandler.TryGetValue(type, out var handler))
        {
            handler = new InteropHandlerWithConversion(_interopHandler[fallback], type);
        }

        return handler;
    }

    private static void BuildMethodReturnCall(StringBuilder stringBuilder, string returnType, GodotTypeKind godotTypeKind)
    {
        if (returnType == "void") return;

        string returnArg = godotTypeKind switch
        {
            GodotTypeKind.NotGodotType => "{0}",
            GodotTypeKind.GodotType => GetHandler(returnType, "GodotObject").InteropTypeToCSharpTypeArgs,
            GodotTypeKind.RefCountedType => GetHandler(returnType, "RefCounted").InteropTypeToCSharpTypeArgs,
            _ => throw new ArgumentOutOfRangeException(nameof(godotTypeKind), godotTypeKind, null)
        };

        stringBuilder
            .AppendIndent("return ")
            .AppendFormat(returnArg, ReturnVariableName)
            .AppendLine(";");
    }

    private static void BuildMethodBodyReturnInit(StringBuilder stringBuilder, string returnType, GodotTypeKind godotTypeKind)
    {
        if (returnType == "void") return;

        stringBuilder
            .AppendIndentLine("// Declare Return Value")
            .AppendIndent();

        IInteropHandler handler = godotTypeKind switch
        {
            GodotTypeKind.NotGodotType => null,
            GodotTypeKind.GodotType => GetHandler(returnType, "GodotObject"),
            GodotTypeKind.RefCountedType => GetHandler(returnType, "RefCounted"),
            _ => throw new ArgumentOutOfRangeException(nameof(godotTypeKind), godotTypeKind, null)
        };

        stringBuilder.Append(handler?.ReturnInteropTypeDeclare ?? returnType);

        stringBuilder
            .AppendLine($" {ReturnVariableName} = default;")
            .AppendIndentLine();


    }

    private static void BuildMethodHeaderReturn(StringBuilder stringBuilder, ReturnValue returnValue, out string returnTypeName)
    {
        if (returnValue == null)
        {
            stringBuilder.Append("void");
            returnTypeName = "void";
            return;
        }

        returnTypeName = CorrectType(returnValue.Type);

        if (_interopHandler.TryGetValue(returnTypeName, out var handler))
        {
            stringBuilder.Append(handler.CsharpType);
        }
        else
        {
            stringBuilder.Append(returnTypeName);
        }
    }

    private static void BuildMethodBodyArguments(StringBuilder stringBuilder, string methodBindName, string methodBindDebugName, string className, string returnType, IReadOnlyList<Argument> arguments, bool isVarArg, ICollection<string> godotTypes, ICollection<string> refCountedGodotTypes)
    {
        stringBuilder
            .AppendIndentLine($""""
                               // Method Bind Null Check
                               if ({methodBindName} == IntPtr.Zero) throw new InvalidOperationException("""Godot Method "{methodBindDebugName}" is uninitialized!""");

                               // Get Self Pointer & Self Pointer Null Check
                               IntPtr selfPtr = GodotObject.GetPtr(this);
                               if(selfPtr == IntPtr.Zero) throw new InvalidOperationException("""Godot Object "{className}" is uninitialized!""");

                               """");

        if (arguments == null)
        {
            stringBuilder.AppendIndent($"""
                                        // Native Method Call
                                        {MethodTableAccess}.object_method_bind_ptrcall((void*){methodBindName}, (void*)selfPtr, null,
                                        """);
        }
        else
        {
            var argumentList = new (string argumentInit, string argumentName, string argumentUsage)[arguments.Count];

            if (arguments.Count > 0)
            {
                stringBuilder.AppendIndentLine("// Method Arguments Interop");

            }

            for (int i = 0; i < arguments.Count; i++)
            {
                Argument classMethodArgument = arguments[i];

                string initArg;
                string argumentUsage;
                string argName = classMethodArgument.Name.EscapeContextualKeyWord();
                string originalArgName = argName;

                string classMethodArgumentType = CorrectType(classMethodArgument.Type);

                IInteropHandler handler = GetTypeKind(classMethodArgumentType, godotTypes, refCountedGodotTypes) switch
                {
                    GodotTypeKind.NotGodotType => null,
                    GodotTypeKind.GodotType => GetHandler(classMethodArgumentType, "GodotObject"),
                    GodotTypeKind.RefCountedType => GetHandler(classMethodArgumentType, "RefCounted"),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (handler != null)
                {
                    argName = $"interop_{classMethodArgument.Name}";
                    initArg = $"{handler.ParamInteropTypeDeclare} {argName} = {string.Format(handler.CsharpTypeToInteropTypeArgs, originalArgName)};";
                    argumentUsage = $"&{argName}";
                }
                else
                {
                    initArg = null;
                    argumentUsage = $"&{argName}";
                }

                argumentList[i] = (initArg, argName, argumentUsage);
            }

            foreach (var (argumentInit, _, _) in argumentList)
            {
                if (argumentInit == null) continue;
                stringBuilder.AppendIndentLine(argumentInit);
            }

            stringBuilder
                .AppendIndentLine()
                .AppendIndent($$"""
                                // Construct and Populate Stack Allocated Argument Array
                                void** argList = stackalloc void*[{{argumentList.Length.ToString(CultureInfo.InvariantCulture)}}]{
                                """);

            foreach (var (_, _, argumentUsage) in argumentList)
            {
                stringBuilder
                    .Append(argumentUsage)
                    .Append(", ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);


            stringBuilder
                .AppendLine(" };")
                .AppendIndent($"""

                               // Native Method Call with Arguments
                               {MethodTableAccess}.object_method_bind_ptrcall((void*){methodBindName}, (void*)GodotObject.GetPtr(this), argList,
                               """);
        }

        stringBuilder
            .Append(returnType != "void" ? $"&{ReturnVariableName}" : "null")
            .AppendLine(");");
    }

    private static void BuildMethodHeaderArguments(StringBuilder stringBuilder, IReadOnlyList<Argument> arguments, bool isVarArg)
    {
        if (arguments == null) return;

        foreach (Argument argument in arguments)
        {
            string correctType = CorrectType(argument.Type);

            if (_interopHandler.TryGetValue(correctType, out var handler))
            {
                correctType = handler.CsharpType;
            }

            stringBuilder
                .Append($"{correctType} {argument.Name.EscapeContextualKeyWord()}, ");
        }

        if (isVarArg)
        {
            stringBuilder.Append("params Variant[] @args, ");
        }

        stringBuilder.Remove(stringBuilder.Length - 2, 2);
    }

    private static GodotTypeKind GetTypeKind(string typeName, ICollection<string> godotTypes, ICollection<string> refCountedGodotTypes)
    {
        GodotTypeKind returnValueGodotTypeKind;

        if (godotTypes.Contains(typeName)) returnValueGodotTypeKind = GodotTypeKind.GodotType;
        else if (refCountedGodotTypes.Contains(typeName)) returnValueGodotTypeKind = GodotTypeKind.RefCountedType;
        else returnValueGodotTypeKind = GodotTypeKind.NotGodotType;

        return returnValueGodotTypeKind;
    }

    private static void GenerateClassMethod(StringBuilder stringBuilder, string className, IReadOnlyList<ClassMethod> engineClassMethods, ICollection<string> godotTypes, ICollection<string> refCountedGodotTypes)
    {
        if (engineClassMethods != null)
        {
            for (int index = 0; index < engineClassMethods.Count; index++)
            {
                ClassMethod classMethod = engineClassMethods[index];

                string methodName = classMethod.Name;
                string methodPascalName = methodName.SnakeCaseToPascalCase();

                string methodBindName = $"MethodBind{index.ToString(CultureInfo.InvariantCulture)}";

                stringBuilder
                    .AppendIndentLine($"""
                                       [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                                       private static readonly IntPtr {methodBindName} = ClassDB_get_method_with_compatibility(NativeName, MethodName.{methodPascalName}, {classMethod.Hash?.ToString(CultureInfo.InvariantCulture)}ul);
                                       """)
                    .AppendLine()
                    .AppendIndent("public unsafe ");

                BuildMethodHeaderReturn(stringBuilder, classMethod.ReturnValue, out string returnTypeName);

                stringBuilder
                    .Append(' ')
                    .Append(methodPascalName)
                    .Append('(');

                bool isVarArg = classMethod.IsVararg;
                
                BuildMethodHeaderArguments(stringBuilder, classMethod.Arguments, isVarArg);

                stringBuilder
                    .AppendLine(")")
                    .AppendIndentLine("{");

                {
                    using var handle = stringBuilder.AddIndent();

                    GodotTypeKind returnValueGodotTypeKind = GetTypeKind(returnTypeName, godotTypes, refCountedGodotTypes);

                    BuildMethodBodyReturnInit(stringBuilder, returnTypeName, returnValueGodotTypeKind);

                    BuildMethodBodyArguments(stringBuilder, methodBindName, methodName, className, returnTypeName, classMethod.Arguments, isVarArg, godotTypes, refCountedGodotTypes);

                    BuildMethodReturnCall(stringBuilder, returnTypeName, returnValueGodotTypeKind);
                }


                stringBuilder
                    .AppendIndentLine("}")
                    .AppendLine();
            }
        }
    }
}
