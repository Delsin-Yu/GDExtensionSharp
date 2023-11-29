using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string ReturnVariableName = "ret";
    private const string VarArgParameterName = "@args";

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
            .AppendIndentLine()
            .AppendIndentLine("// Returns")
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

    private static void BuildMethodBodyArguments(StringBuilder stringBuilder, string methodBindName, string methodBindDebugName, string methodBindPascalName, string className, string returnType, IReadOnlyList<Argument> arguments, bool isVarArg, ICollection<string> godotTypes, ICollection<string> refCountedGodotTypes)
    {
        const string selfPtr = "selfPtr";

        stringBuilder
            .AppendIndentLine($""""
                               // Method Bind Null Check
                               if ({methodBindName} == IntPtr.Zero) throw new InvalidOperationException("""Godot Method "{methodBindDebugName}" is uninitialized!""");

                               // Get Self Pointer & Self Pointer Null Check
                               IntPtr {selfPtr} = GodotObject.GetPtr(this);
                               if(selfPtr == IntPtr.Zero) throw new InvalidOperationException("""Godot Object "{className}" is uninitialized!""");

                               """");

        if (isVarArg)
        {
            const string varArgsSpanThreshold = "10";

            const string varArgsLength = $"{VarArgParameterName}_length";

            const string varArgsSpan = "varArgsSpan";

            const string callArgsSpan = "callArgsSpan";

            const string varArgs = "varArgs";
            const string callArgs = "callArgs";

            const string callError = $"{VarArgParameterName}_callError";


            if (arguments == null)
            {

                stringBuilder.AppendIndent($$"""
                                            // Params Impl
                                            {{VarArgParameterName}} = {{VarArgParameterName}} ?? Array.Empty<Variant>();

                                            int {{varArgsLength}} = {{VarArgParameterName}}.Length;

                                            // Construct Stack Allocated Argument Array
                                            Span<godot_variant.movable> {{varArgsSpan}} =
                                                {{varArgsLength}} <= {{varArgsSpanThreshold}} ?
                                                    stackalloc godot_variant.movable[{{varArgsSpanThreshold}}] :
                                                    new godot_variant.movable[{{varArgsLength}}];

                                            Span<IntPtr> {{callArgsSpan}} =
                                                {{varArgsLength}} <= {{varArgsSpanThreshold}} ?
                                                    stackalloc IntPtr[{{varArgsSpanThreshold}}] :
                                                    new IntPtr[{{varArgsLength}}];

                                            fixed (godot_variant.movable* {{varArgs}} = &MemoryMarshal.GetReference({{varArgsSpan}}))
                                            fixed (IntPtr* {{callArgs}} = &MemoryMarshal.GetReference({{callArgsSpan}}))
                                            {
                                                // Populate Stack Allocated Argument Array
                                                for (int i = 0; i < {{varArgsLength}}; i++)
                                                {
                                                    {{varArgs}}[i] = {{VarArgParameterName}}[i].NativeVar;
                                                    {{callArgs}}[i] = new IntPtr(&{{varArgs}}[i]);
                                                }

                                                // Declare Call Error Struct
                                                global::GDExtensionSharp.GDExtensionCallError {{callError}};

                                                // Native Method Call with Args
                                                {{MethodTableAccess}}.object_method_bind_call((void*){{methodBindName}}, (void*)selfPtr, (void**){{callArgs}}, {{varArgsLength}}, {{(returnType != "void" ? $"&{ReturnVariableName}" : "null")}}, &{{callError}});

                                                // Check Call Error and Log Result
                                                ExceptionUtils.DebugCheckCallError((godot_string_name)MethodName.{{methodBindPascalName}}.NativeValue, {{selfPtr}}, (godot_variant**){{callArgs}}, {{varArgsLength}}, {{callError}}.Bridge());
                                            }

                                            """);
            }
            else
            {
                var argumentList = ConstructArgumentList(arguments, godotTypes, refCountedGodotTypes);

                const string totalArgsLength = "@totalArgLength";

                string argumentListLength = argumentList.Length.ToString(CultureInfo.InvariantCulture);

                stringBuilder.AppendIndentLine($$"""
                                                 // Params Impl
                                                 {{VarArgParameterName}} = {{VarArgParameterName}} ?? Array.Empty<Variant>();

                                                 int {{varArgsLength}} = {{VarArgParameterName}}.Length;
                                                 int {{totalArgsLength}} = {{argumentListLength}} + {{varArgsLength}};

                                                 // Construct Stack Allocated Argument Array
                                                 Span<godot_variant.movable> {{varArgsSpan}} =
                                                 {{varArgsLength}} <= {{varArgsSpanThreshold}} ?
                                                     stackalloc godot_variant.movable[{{varArgsSpanThreshold}}] :
                                                     new godot_variant.movable[{{varArgsLength}}];

                                                 Span<IntPtr> {{callArgsSpan}} =
                                                 {{totalArgsLength}} <= {{varArgsSpanThreshold}} ?
                                                     stackalloc IntPtr[{{varArgsSpanThreshold}}] :
                                                     new IntPtr[{{totalArgsLength}}];

                                                 fixed (godot_variant.movable* {{varArgs}} = &MemoryMarshal.GetReference({{varArgsSpan}}))
                                                 fixed (IntPtr* {{callArgs}} = &MemoryMarshal.GetReference({{callArgsSpan}}))
                                                 {
                                                     // Populate Other Arguments
                                                 """);

                using (stringBuilder.AddIndent())
                {
                    for (int index = 0; index < argumentList.Length; index++)
                    {
                        (_, _, _, string argumentOriginalName) = argumentList[index];

                        stringBuilder.AppendIndentLine($"""
                                                        using godot_variant variant_{argumentOriginalName} = InteropStructUtils.CreateVariant({argumentOriginalName.EscapeContextualKeyWord()});
                                                        {callArgs}[{index}] = new IntPtr(&variant_{argumentOriginalName});
                                                        """);
                    }
                }


                stringBuilder.AppendIndentLine($$"""

                                                   // Populate Params into Stack Allocated Argument Array
                                                   for (int i = 0; i < {{varArgsLength}}; i++)
                                                   {
                                                       {{varArgs}}[i] = {{VarArgParameterName}}[i].NativeVar;
                                                       {{callArgs}}[{{argumentListLength}} + i] = new IntPtr(&{{varArgs}}[i]);
                                                   }

                                                   // Declare Call Error Struct
                                                   global::GDExtensionSharp.GDExtensionCallError {{callError}};

                                                   // Native Method Call with Args
                                                   {{MethodTableAccess}}.object_method_bind_call((void*){{methodBindName}}, (void*)selfPtr, (void**){{callArgs}}, {{varArgsLength}}, {{(returnType != "void" ? $"&{ReturnVariableName}" : "null")}}, &{{callError}});

                                                   // Check Call Error and Log Result
                                                   ExceptionUtils.DebugCheckCallError((godot_string_name)MethodName.{{methodBindPascalName}}.NativeValue, {{selfPtr}}, (godot_variant**){{callArgs}}, {{varArgsLength}}, {{callError}}.Bridge());
                                               }

                                               """);
            }
        }
        else
        {
            if (arguments == null)
            {
                stringBuilder.AppendIndent($"""
                                            // Native Method Call
                                            {MethodTableAccess}.object_method_bind_ptrcall((void*){methodBindName}, (void*)selfPtr, null,
                                            """);
            }
            else
            {
                if (arguments.Count > 0)
                {
                    stringBuilder.AppendIndentLine("// Method Arguments Interop");
                }

                var argumentList = ConstructArgumentList(arguments, godotTypes, refCountedGodotTypes);

                foreach (var (argumentInit, argumentName, _, _) in argumentList)
                {
                    if (argumentInit == null) continue;
                    (string argDeclare, string argInterop) = argumentInit.Value;
                    stringBuilder.AppendIndentLine($"{argDeclare} {argumentName} = {argInterop};");
                }

                stringBuilder
                    .AppendIndentLine()
                    .AppendIndent($$"""
                                    // Construct and Populate Stack Allocated Argument Array
                                    void** argList = stackalloc void*[{{argumentList.Length.ToString(CultureInfo.InvariantCulture)}}]{
                                    """);

                foreach (var (_, _, argumentUsage, _) in argumentList)
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



    }

    private static ((string argDeclare, string argInterop)? argumentInit, string argumentName, string argumentUsage, string argumentOriginalName)[] ConstructArgumentList(IReadOnlyList<Argument> arguments, ICollection<string> godotTypes, ICollection<string> refCountedGodotTypes)
    {
        var argumentList = new ((string argDeclare, string argInterop)? argumentInit, string argumentName, string argumentUsage, string argumentOriginalName)[arguments.Count];

        for (int i = 0; i < arguments.Count; i++)
        {
            Argument classMethodArgument = arguments[i];

            (string argDeclare, string argInterop)? initArg;
            string argumentUsage;
            string argumentName = classMethodArgument.Name.EscapeContextualKeyWord();
            string originalArgName = argumentName;

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
                argumentName = $"interop_{classMethodArgument.Name}";
                initArg = (handler.ParamInteropTypeDeclare, string.Format(handler.CsharpTypeToInteropTypeArgs, originalArgName));
                argumentUsage = $"&{argumentName}";
            }
            else
            {
                initArg = null;
                argumentUsage = $"&{argumentName}";
            }

            argumentList[i] = (initArg, argumentName, argumentUsage, classMethodArgument.Name);
        }

        return argumentList;
    }

    private static void BuildMethodHeaderArguments(StringBuilder stringBuilder, IReadOnlyList<Argument> arguments, bool isVarArg)
    {
        if (arguments == null)
        {
            if (isVarArg)
            {
                stringBuilder.Append($"params Variant[] {VarArgParameterName}");
            }
        }
        else
        {
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
                stringBuilder.Append($"params Variant[] {VarArgParameterName}, ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
        }
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

                    BuildMethodBodyArguments(stringBuilder, methodBindName, methodName, methodPascalName, className, returnTypeName, classMethod.Arguments, isVarArg, godotTypes, refCountedGodotTypes);

                    BuildMethodReturnCall(stringBuilder, returnTypeName, returnValueGodotTypeKind);
                }


                stringBuilder
                    .AppendIndentLine("}")
                    .AppendLine();
            }
        }
    }
}
