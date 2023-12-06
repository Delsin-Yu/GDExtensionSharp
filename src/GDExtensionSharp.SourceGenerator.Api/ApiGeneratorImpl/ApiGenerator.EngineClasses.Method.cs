using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string ReturnVariableName = "ret";
    private const string VarArgParameterName = "@args";

    private static readonly Regex _godotArrayRegex = new(@"Godot\.Collections\.Array<(?<InnerType>\w+)>", RegexOptions.Compiled);

    private static IInteropHandler GetInteropHandler(string type, IReadOnlyCollection<string> godotTypes, IReadOnlyCollection<string> refCountedGodotTypes)
    {
        var arrayMatch = _godotArrayRegex.Match(type);

        IInteropHandler handler;
        if (arrayMatch.Success)
        {
            string innerType = arrayMatch.Groups["InnerType"].Value;
            handler = new GodotArrayInteropHandler(innerType);
        }
        else if (!_interopHandler.TryGetValue(type, out handler))
        {
            if (godotTypes.Contains(type))
            {
                handler = new InteropHandlerWithConversion(_interopHandler["GodotObject"], type);
            }
            else if (refCountedGodotTypes.Contains(type))
            {
                handler = new InteropHandlerWithConversion(_interopHandler["RefCounted"], type);
            }
            else
            {
                handler = new NonInteropHandler(type);
            }
        }

        return handler;
    }

    private static void BuildMethodReturnCall(StringBuilder stringBuilder, string returnType, IReadOnlyCollection<string> godotTypes, IReadOnlyCollection<string> refCountedGodotTypes)
    {
        if (returnType == "void") return;

        string returnArg = GetInteropHandler(returnType, godotTypes, refCountedGodotTypes).InteropTypeToCSharpTypeArgs;

        stringBuilder
            .AppendIndentLine()
            .AppendIndentLine("// Returns")
            .AppendIndent("return ")
            .AppendFormat(returnArg, ReturnVariableName)
            .AppendLine(";");
    }

    private static void BuildMethodBodyReturnInit(StringBuilder stringBuilder, string returnType, IReadOnlyCollection<string> godotTypes, IReadOnlyCollection<string> refCountedGodotTypes)
    {
        if (returnType == "void") return;

        stringBuilder
            .AppendIndentLine("// Declare Return Value")
            .AppendIndent();

        stringBuilder.Append(GetInteropHandler(returnType, godotTypes, refCountedGodotTypes).ReturnInteropTypeDeclare);

        stringBuilder
            .AppendLine($" {ReturnVariableName} = default;")
            .AppendIndentLine();
    }

    private static void BuildMethodHeaderReturn(StringBuilder stringBuilder, ReturnValue returnValue, IReadOnlyCollection<string> godotTypes, IReadOnlyCollection<string> refCountedGodotTypes, out string returnTypeName)
    {
        if (returnValue == null)
        {
            stringBuilder.Append("void");
            returnTypeName = "void";
            return;
        }

        returnTypeName = CorrectType(returnValue.Type);

        stringBuilder.Append(GetInteropHandler(returnTypeName, godotTypes, refCountedGodotTypes).CsharpType);
    }

    private static void BuildMethodBodyArguments(StringBuilder stringBuilder, string methodBindName, string methodBindDebugName, string methodBindPascalName, string className, string returnType, IReadOnlyList<Argument> arguments, bool isVarArg, IReadOnlyCollection<string> godotTypes, IReadOnlyCollection<string> refCountedGodotTypes)
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

                const string totalArgsLength = "@totalArg_length";

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

    private static ((string argDeclare, string argInterop)? argumentInit, string argumentName, string argumentUsage, string argumentOriginalName)[] ConstructArgumentList(IReadOnlyList<Argument> arguments, IReadOnlyCollection<string> godotTypes, IReadOnlyCollection<string> refCountedGodotTypes)
    {
        var argumentList = new ((string argDeclare, string argInterop)? argumentInit, string argumentName, string argumentUsage, string argumentOriginalName)[arguments.Count];

        for (int i = 0; i < arguments.Count; i++)
        {
            Argument classMethodArgument = arguments[i];

            (string argDeclare, string argInterop)? initArg;
            string argumentName = classMethodArgument.Name.EscapeContextualKeyWord();
            string originalArgName = argumentName;

            string classMethodArgumentType = CorrectType(classMethodArgument.Type);

            IInteropHandler handler = GetInteropHandler(classMethodArgumentType, godotTypes, refCountedGodotTypes);

            if (handler.ParamInteropTypeDeclare != null)
            {
                argumentName = $"interop_{classMethodArgument.Name}";
                initArg = (handler.ParamInteropTypeDeclare, string.Format(handler.CsharpTypeToInteropTypeArgs, originalArgName));
            }
            else
            {
                initArg = null;
            }

            argumentList[i] = (initArg, argumentName, argumentUsage: $"&{argumentName}", classMethodArgument.Name);
        }

        return argumentList;
    }

    private static void BuildMethodHeaderArguments(StringBuilder stringBuilder, IReadOnlyList<Argument> arguments, IReadOnlyCollection<string> godotTypes, IReadOnlyCollection<string> refCountedGodotTypes, bool isVarArg)
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
                string correctType = GetInteropHandler(CorrectType(argument.Type), godotTypes, refCountedGodotTypes).CsharpType;
                stringBuilder
                    .Append($"{correctType} {argument.Name.EscapeContextualKeyWord()}");


                switch (argument.DefaultValue)
                {
                    case null:
                        break;
                    default:
                        stringBuilder.Append(" = default");
                        break;
                }

                stringBuilder.Append(", ");
            }

            if (isVarArg)
            {
                stringBuilder.Append($"params Variant[] {VarArgParameterName}, ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
        }
    }

    private static void GenerateClassMethod(
        StringBuilder stringBuilder,
        string className,
        IReadOnlyList<ClassMethod> engineClassMethods,
        IReadOnlyCollection<string> godotTypes,
        IReadOnlyCollection<string> refCountedGodotTypes,
        IReadOnlyCollection<string> usedPropertyAccessor)
    {
        if (engineClassMethods == null)
        {
            return;
        }

        stringBuilder.AppendLine("#region Method Bind");

        for (int index = 0; index < engineClassMethods.Count; index++)
        {
            ClassMethod classMethod = engineClassMethods[index];

            if(classMethod.IsVirtual) continue;

            string methodName = classMethod.Name;
            string methodPascalName = methodName.SnakeCaseToPascalCase();

            string methodBindName = $"MethodBind{index.ToString(CultureInfo.InvariantCulture)}";

            stringBuilder
                .AppendIndentLine($"""
                                   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                                   private static readonly IntPtr {methodBindName} = ClassDB_get_method_with_compatibility(NativeName, MethodName.{methodPascalName}, {classMethod.Hash?.ToString(CultureInfo.InvariantCulture)}ul);
                                   """);
        }

        stringBuilder
            .AppendLine("#endregion")
            .AppendLine();

        for (int index = 0; index < engineClassMethods.Count; index++)
        {
            ClassMethod classMethod = engineClassMethods[index];

            string methodName = classMethod.Name;
            string methodPascalName = methodName.SnakeCaseToPascalCase();

            string methodBindName = $"MethodBind{index.ToString(CultureInfo.InvariantCulture)}";

            stringBuilder.AppendIndent();

            if (usedPropertyAccessor.Contains(methodPascalName))
            {
                stringBuilder.Append("private");
            }
            else
            {
                stringBuilder.Append("public");
            }

            bool isVirtual = classMethod.IsVirtual;

            if (isVirtual)
            {
                stringBuilder.Append(" virtual ");
            }
            else
            {
                stringBuilder.Append(" unsafe ");
            }

            BuildMethodHeaderReturn(stringBuilder, classMethod.ReturnValue, godotTypes, refCountedGodotTypes, out string returnTypeName);

            stringBuilder
                .Append(' ');

            if (isVirtual)
            {
                stringBuilder.Append('_');
            }

            stringBuilder
                .Append(methodPascalName)
                .Append('(');

            bool isVarArg = classMethod.IsVararg;

            BuildMethodHeaderArguments(stringBuilder, classMethod.Arguments, godotTypes, refCountedGodotTypes, isVarArg);

            stringBuilder
                .AppendLine(")")
                .AppendIndentLine("{");

            using (stringBuilder.AddIndent())
            {
                if (!isVirtual)
                {
                    BuildMethodBodyReturnInit(stringBuilder, returnTypeName, godotTypes, refCountedGodotTypes);

                    BuildMethodBodyArguments(stringBuilder, methodBindName, methodName, methodPascalName, className, returnTypeName, classMethod.Arguments, isVarArg, godotTypes, refCountedGodotTypes);

                    BuildMethodReturnCall(stringBuilder, returnTypeName, godotTypes, refCountedGodotTypes);
                }
                else if(returnTypeName != "void")
                {
                    stringBuilder.AppendIndentLine("""
                                                   // Virtual Method Default Return
                                                   return default;
                                                   """);
                }
            }

            stringBuilder
                .AppendIndentLine("}")
                .AppendLine();
        }
    }
}
