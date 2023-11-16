// ReSharper disable RedundantUsingDirective.Global
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable InvalidXmlDocComment
// ReSharper disable RedundantUnsafeContext

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
#pragma warning disable CS0169 // Field is never used

global using int8_t = System.SByte;
global using int16_t = System.Int16;
global using int32_t = System.Int32;
global using int64_t = System.Int64;
global using uint8_t = System.Byte;
global using uint16_t = System.UInt16;
global using uint32_t = System.UInt32;
global using uint64_t = System.UInt64;

global using size_t = System.UInt64;

global using wchar_t = System.UInt16;
global using char16_t = System.UInt16;
global using char32_t = System.UInt32;

global using GDExtensionInt = System.Int64;
global using GDExtensionBool = System.Byte;
global using GDObjectInstanceID = System.UInt64;

using System.Runtime.InteropServices;


internal enum GDExtensionVariantType
{
    GDEXTENSION_VARIANT_TYPE_NIL,

    /* atomic types */
    GDEXTENSION_VARIANT_TYPE_BOOL,
    GDEXTENSION_VARIANT_TYPE_INT,
    GDEXTENSION_VARIANT_TYPE_FLOAT,
    GDEXTENSION_VARIANT_TYPE_STRING,

    /* math types */
    GDEXTENSION_VARIANT_TYPE_VECTOR2,
    GDEXTENSION_VARIANT_TYPE_VECTOR2I,
    GDEXTENSION_VARIANT_TYPE_RECT2,
    GDEXTENSION_VARIANT_TYPE_RECT2I,
    GDEXTENSION_VARIANT_TYPE_VECTOR3,
    GDEXTENSION_VARIANT_TYPE_VECTOR3I,
    GDEXTENSION_VARIANT_TYPE_TRANSFORM2D,
    GDEXTENSION_VARIANT_TYPE_VECTOR4,
    GDEXTENSION_VARIANT_TYPE_VECTOR4I,
    GDEXTENSION_VARIANT_TYPE_PLANE,
    GDEXTENSION_VARIANT_TYPE_QUATERNION,
    GDEXTENSION_VARIANT_TYPE_AABB,
    GDEXTENSION_VARIANT_TYPE_BASIS,
    GDEXTENSION_VARIANT_TYPE_TRANSFORM3D,
    GDEXTENSION_VARIANT_TYPE_PROJECTION,

    /* misc types */
    GDEXTENSION_VARIANT_TYPE_COLOR,
    GDEXTENSION_VARIANT_TYPE_STRING_NAME,
    GDEXTENSION_VARIANT_TYPE_NODE_PATH,
    GDEXTENSION_VARIANT_TYPE_RID,
    GDEXTENSION_VARIANT_TYPE_OBJECT,
    GDEXTENSION_VARIANT_TYPE_CALLABLE,
    GDEXTENSION_VARIANT_TYPE_SIGNAL,
    GDEXTENSION_VARIANT_TYPE_DICTIONARY,
    GDEXTENSION_VARIANT_TYPE_ARRAY,

    /* typed arrays */
    GDEXTENSION_VARIANT_TYPE_PACKED_BYTE_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_INT32_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_INT64_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_FLOAT32_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_FLOAT64_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_STRING_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_VECTOR2_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_VECTOR3_ARRAY,
    GDEXTENSION_VARIANT_TYPE_PACKED_COLOR_ARRAY,
    GDEXTENSION_VARIANT_TYPE_VARIANT_MAX,
}

internal enum GDExtensionVariantOperator
{

    /* comparison */
    GDEXTENSION_VARIANT_OP_EQUAL,
    GDEXTENSION_VARIANT_OP_NOT_EQUAL,
    GDEXTENSION_VARIANT_OP_LESS,
    GDEXTENSION_VARIANT_OP_LESS_EQUAL,
    GDEXTENSION_VARIANT_OP_GREATER,
    GDEXTENSION_VARIANT_OP_GREATER_EQUAL,

    /* mathematic */
    GDEXTENSION_VARIANT_OP_ADD,
    GDEXTENSION_VARIANT_OP_SUBTRACT,
    GDEXTENSION_VARIANT_OP_MULTIPLY,
    GDEXTENSION_VARIANT_OP_DIVIDE,
    GDEXTENSION_VARIANT_OP_NEGATE,
    GDEXTENSION_VARIANT_OP_POSITIVE,
    GDEXTENSION_VARIANT_OP_MODULE,
    GDEXTENSION_VARIANT_OP_POWER,

    /* bitwise */
    GDEXTENSION_VARIANT_OP_SHIFT_LEFT,
    GDEXTENSION_VARIANT_OP_SHIFT_RIGHT,
    GDEXTENSION_VARIANT_OP_BIT_AND,
    GDEXTENSION_VARIANT_OP_BIT_OR,
    GDEXTENSION_VARIANT_OP_BIT_XOR,
    GDEXTENSION_VARIANT_OP_BIT_NEGATE,

    /* logic */
    GDEXTENSION_VARIANT_OP_AND,
    GDEXTENSION_VARIANT_OP_OR,
    GDEXTENSION_VARIANT_OP_XOR,
    GDEXTENSION_VARIANT_OP_NOT,

    /* containment */
    GDEXTENSION_VARIANT_OP_IN,
    GDEXTENSION_VARIANT_OP_MAX,
}

internal enum GDExtensionCallErrorType
{
    GDEXTENSION_CALL_OK,
    GDEXTENSION_CALL_ERROR_INVALID_METHOD,

    /// <summary>
    ///  Expected a different variant type.
    /// </summary>
    GDEXTENSION_CALL_ERROR_INVALID_ARGUMENT,

    /// <summary>
    ///  Expected lower number of arguments.
    /// </summary>
    GDEXTENSION_CALL_ERROR_TOO_MANY_ARGUMENTS,

    /// <summary>
    ///  Expected higher number of arguments.
    /// </summary>
    GDEXTENSION_CALL_ERROR_TOO_FEW_ARGUMENTS,
    GDEXTENSION_CALL_ERROR_INSTANCE_IS_NULL,
    GDEXTENSION_CALL_ERROR_METHOD_NOT_CONST,
    Used,
    @for,
    @const,
    call,
}

internal enum GDExtensionClassMethodFlags
{
    GDEXTENSION_METHOD_FLAG_NORMAL = 1,
    GDEXTENSION_METHOD_FLAG_EDITOR = 2,
    GDEXTENSION_METHOD_FLAG_CONST = 4,
    GDEXTENSION_METHOD_FLAG_VIRTUAL = 8,
    GDEXTENSION_METHOD_FLAG_VARARG = 16,
    GDEXTENSION_METHOD_FLAG_STATIC = 32,
    GDEXTENSION_METHOD_FLAGS_DEFAULT = GDEXTENSION_METHOD_FLAG_NORMAL,
}

internal enum GDExtensionClassMethodArgumentMetadata
{
    GDEXTENSION_METHOD_ARGUMENT_METADATA_NONE,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_INT8,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_INT16,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_INT32,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_INT64,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_UINT8,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_UINT16,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_UINT32,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_INT_IS_UINT64,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_REAL_IS_FLOAT,
    GDEXTENSION_METHOD_ARGUMENT_METADATA_REAL_IS_DOUBLE,
}

internal enum GDExtensionInitializationLevel
{
    GDEXTENSION_INITIALIZATION_CORE,
    GDEXTENSION_INITIALIZATION_SERVERS,
    GDEXTENSION_INITIALIZATION_SCENE,
    GDEXTENSION_INITIALIZATION_EDITOR,
    GDEXTENSION_MAX_INITIALIZATION_LEVEL,
}

internal unsafe struct MethodTable
{

    private static void* GetMethod(string methodName, delegate* unmanaged <byte*, delegate* unmanaged <void>> p_get_proc_address)
    {
        var array = stackalloc byte[methodName.Length + 1];
        for (var i = 0; i < methodName.Length; i++)
        {
            array[i] = (byte)methodName[i];
        }
        array[methodName.Length] = 0;
        var methodPointer = p_get_proc_address(array);
        return methodPointer;
    }

    internal MethodTable(delegate* unmanaged <byte*, delegate* unmanaged <void>> p_get_proc_address)
    {
        // ReSharper disable StringLiteralTypo
        GDExtensionInterfaceGetGodotVersion = (delegate* unmanaged[Cdecl]<GDExtensionGodotVersion*, void>)GetMethod("get_godot_version", p_get_proc_address);
        GDExtensionInterfaceMemAlloc = (delegate* unmanaged[Cdecl]<size_t, void*>)GetMethod("mem_alloc", p_get_proc_address);
        GDExtensionInterfaceMemRealloc = (delegate* unmanaged[Cdecl]<void*, size_t, void*>)GetMethod("mem_realloc", p_get_proc_address);
        GDExtensionInterfaceMemFree = (delegate* unmanaged[Cdecl]<void*, void>)GetMethod("mem_free", p_get_proc_address);
        GDExtensionInterfacePrintError = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_error", p_get_proc_address);
        GDExtensionInterfacePrintErrorWithMessage = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_error_with_message", p_get_proc_address);
        GDExtensionInterfacePrintWarning = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_warning", p_get_proc_address);
        GDExtensionInterfacePrintWarningWithMessage = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_warning_with_message", p_get_proc_address);
        GDExtensionInterfacePrintScriptError = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_script_error", p_get_proc_address);
        GDExtensionInterfacePrintScriptErrorWithMessage = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_script_error_with_message", p_get_proc_address);
        GDExtensionInterfaceGetNativeStructSize = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint64_t>)GetMethod("get_native_struct_size", p_get_proc_address);
        GDExtensionInterfaceVariantNewCopy = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_new_copy", p_get_proc_address);
        GDExtensionInterfaceVariantNewNil = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_new_nil", p_get_proc_address);
        GDExtensionInterfaceVariantDestroy = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_destroy", p_get_proc_address);
        GDExtensionInterfaceVariantCall = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void>)GetMethod("variant_call", p_get_proc_address);
        GDExtensionInterfaceVariantCallStatic = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void>)GetMethod("variant_call_static", p_get_proc_address);
        GDExtensionInterfaceVariantEvaluate = (delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_evaluate", p_get_proc_address);
        GDExtensionInterfaceVariantSet = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_set", p_get_proc_address);
        GDExtensionInterfaceVariantSetNamed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_set_named", p_get_proc_address);
        GDExtensionInterfaceVariantSetKeyed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_set_keyed", p_get_proc_address);
        GDExtensionInterfaceVariantSetIndexed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void>)GetMethod("variant_set_indexed", p_get_proc_address);
        GDExtensionInterfaceVariantGet = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_get", p_get_proc_address);
        GDExtensionInterfaceVariantGetNamed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_get_named", p_get_proc_address);
        GDExtensionInterfaceVariantGetKeyed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_get_keyed", p_get_proc_address);
        GDExtensionInterfaceVariantGetIndexed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void>)GetMethod("variant_get_indexed", p_get_proc_address);
        GDExtensionInterfaceVariantIterInit = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool>)GetMethod("variant_iter_init", p_get_proc_address);
        GDExtensionInterfaceVariantIterNext = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool>)GetMethod("variant_iter_next", p_get_proc_address);
        GDExtensionInterfaceVariantIterGet = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_iter_get", p_get_proc_address);
        GDExtensionInterfaceVariantHash = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt>)GetMethod("variant_hash", p_get_proc_address);
        GDExtensionInterfaceVariantRecursiveHash = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionInt>)GetMethod("variant_recursive_hash", p_get_proc_address);
        GDExtensionInterfaceVariantHashCompare = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_hash_compare", p_get_proc_address);
        GDExtensionInterfaceVariantBooleanize = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_booleanize", p_get_proc_address);
        GDExtensionInterfaceVariantDuplicate = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool, void>)GetMethod("variant_duplicate", p_get_proc_address);
        GDExtensionInterfaceVariantStringify = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_stringify", p_get_proc_address);
        GDExtensionInterfaceVariantGetType = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType>)GetMethod("variant_get_type", p_get_proc_address);
        GDExtensionInterfaceVariantHasMethod = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_has_method", p_get_proc_address);
        GDExtensionInterfaceVariantHasMember = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_has_member", p_get_proc_address);
        GDExtensionInterfaceVariantHasKey = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool>)GetMethod("variant_has_key", p_get_proc_address);
        GDExtensionInterfaceVariantGetTypeName = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_get_type_name", p_get_proc_address);
        GDExtensionInterfaceVariantCanConvert = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool>)GetMethod("variant_can_convert", p_get_proc_address);
        GDExtensionInterfaceVariantCanConvertStrict = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool>)GetMethod("variant_can_convert_strict", p_get_proc_address);
        GDExtensionInterfaceGetVariantFromTypeConstructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("get_variant_from_type_constructor", p_get_proc_address);
        GDExtensionInterfaceGetVariantToTypeConstructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("get_variant_to_type_constructor", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrOperatorEvaluator = (delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, GDExtensionVariantType, GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_operator_evaluator", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrBuiltinMethod = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, int, void>>)GetMethod("variant_get_ptr_builtin_method", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrConstructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, int32_t, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, void>>)GetMethod("variant_get_ptr_constructor", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrDestructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_destructor", p_get_proc_address);
        GDExtensionInterfaceVariantConstruct = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int32_t, GDExtensionCallError*, void>)GetMethod("variant_construct", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrSetter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_setter", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrGetter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_getter", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrIndexedSetter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_indexed_setter", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrIndexedGetter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_indexed_getter", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrKeyedSetter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_keyed_setter", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrKeyedGetter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_keyed_getter", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrKeyedChecker = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, uint32_t>>)GetMethod("variant_get_ptr_keyed_checker", p_get_proc_address);
        GDExtensionInterfaceVariantGetConstantValue = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_get_constant_value", p_get_proc_address);
        GDExtensionInterfaceVariantGetPtrUtilityFunction = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int, void>>)GetMethod("variant_get_ptr_utility_function", p_get_proc_address);
        GDExtensionInterfaceStringNewWithLatin1Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void>)GetMethod("string_new_with_latin1_chars", p_get_proc_address);
        GDExtensionInterfaceStringNewWithUtf8Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void>)GetMethod("string_new_with_utf8_chars", p_get_proc_address);
        GDExtensionInterfaceStringNewWithUtf16Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, void>)GetMethod("string_new_with_utf16_chars", p_get_proc_address);
        GDExtensionInterfaceStringNewWithUtf32Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void>)GetMethod("string_new_with_utf32_chars", p_get_proc_address);
        GDExtensionInterfaceStringNewWithWideChars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void>)GetMethod("string_new_with_wide_chars", p_get_proc_address);
        GDExtensionInterfaceStringNewWithLatin1CharsAndLen = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void>)GetMethod("string_new_with_latin1_chars_and_len", p_get_proc_address);
        GDExtensionInterfaceStringNewWithUtf8CharsAndLen = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void>)GetMethod("string_new_with_utf8_chars_and_len", p_get_proc_address);
        GDExtensionInterfaceStringNewWithUtf16CharsAndLen = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, void>)GetMethod("string_new_with_utf16_chars_and_len", p_get_proc_address);
        GDExtensionInterfaceStringNewWithUtf32CharsAndLen = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, void>)GetMethod("string_new_with_utf32_chars_and_len", p_get_proc_address);
        GDExtensionInterfaceStringNewWithWideCharsAndLen = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, void>)GetMethod("string_new_with_wide_chars_and_len", p_get_proc_address);
        GDExtensionInterfaceStringToLatin1Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_latin1_chars", p_get_proc_address);
        GDExtensionInterfaceStringToUtf8Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_utf8_chars", p_get_proc_address);
        GDExtensionInterfaceStringToUtf16Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_utf16_chars", p_get_proc_address);
        GDExtensionInterfaceStringToUtf32Chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_utf32_chars", p_get_proc_address);
        GDExtensionInterfaceStringToWideChars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_wide_chars", p_get_proc_address);
        GDExtensionInterfaceStringOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*>)GetMethod("string_operator_index", p_get_proc_address);
        GDExtensionInterfaceStringOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*>)GetMethod("string_operator_index_const", p_get_proc_address);
        GDExtensionInterfaceStringOperatorPlusEqString = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("string_operator_plus_eq_string", p_get_proc_address);
        GDExtensionInterfaceStringOperatorPlusEqChar = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t, void>)GetMethod("string_operator_plus_eq_char", p_get_proc_address);
        GDExtensionInterfaceStringOperatorPlusEqCstr = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void>)GetMethod("string_operator_plus_eq_cstr", p_get_proc_address);
        GDExtensionInterfaceStringOperatorPlusEqWcstr = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void>)GetMethod("string_operator_plus_eq_wcstr", p_get_proc_address);
        GDExtensionInterfaceStringOperatorPlusEqC32str = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void>)GetMethod("string_operator_plus_eq_c32str", p_get_proc_address);
        GDExtensionInterfaceXmlParserOpenBuffer = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, size_t, GDExtensionInt>)GetMethod("xml_parser_open_buffer", p_get_proc_address);
        GDExtensionInterfaceFileAccessStoreBuffer = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, void>)GetMethod("file_access_store_buffer", p_get_proc_address);
        GDExtensionInterfaceFileAccessGetBuffer = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, uint64_t>)GetMethod("file_access_get_buffer", p_get_proc_address);
        GDExtensionInterfacePackedByteArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*>)GetMethod("worker_thread_pool_add_native_group_task", p_get_proc_address);
        GDExtensionInterfacePackedByteArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*>)GetMethod("packed_byte_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedColorArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_color_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedColorArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_color_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedFloat32ArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*>)GetMethod("packed_float32_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedFloat32ArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*>)GetMethod("packed_float32_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedFloat64ArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*>)GetMethod("packed_float64_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedFloat64ArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*>)GetMethod("packed_float64_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedInt32ArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*>)GetMethod("packed_int32_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedInt32ArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*>)GetMethod("packed_int32_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedInt64ArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*>)GetMethod("packed_int64_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedInt64ArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*>)GetMethod("packed_int64_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedStringArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_string_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedStringArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_string_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedVector2ArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector2_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedVector2ArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector2_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfacePackedVector3ArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector3_array_operator_index", p_get_proc_address);
        GDExtensionInterfacePackedVector3ArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector3_array_operator_index_const", p_get_proc_address);
        GDExtensionInterfaceArrayOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("array_operator_index", p_get_proc_address);
        GDExtensionInterfaceArrayOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("array_operator_index_const", p_get_proc_address);
        GDExtensionInterfaceArrayRef = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("array_ref", p_get_proc_address);
        GDExtensionInterfaceArraySetTyped = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("array_set_typed", p_get_proc_address);
        GDExtensionInterfaceDictionaryOperatorIndex = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("dictionary_operator_index", p_get_proc_address);
        GDExtensionInterfaceDictionaryOperatorIndexConst = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("dictionary_operator_index_const", p_get_proc_address);
        GDExtensionInterfaceObjectMethodBindCall = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void>)GetMethod("object_method_bind_call", p_get_proc_address);
        GDExtensionInterfaceObjectMethodBindPtrcall = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("object_method_bind_ptrcall", p_get_proc_address);
        GDExtensionInterfaceObjectDestroy = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("object_destroy", p_get_proc_address);
        GDExtensionInterfaceGlobalGetSingleton = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("global_get_singleton", p_get_proc_address);
        GDExtensionInterfaceObjectGetInstanceBinding = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, GDExtensionInstanceBindingCallbacks*, void*>)GetMethod("object_get_instance_binding", p_get_proc_address);
        GDExtensionInterfaceObjectSetInstanceBinding = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, void*, GDExtensionInstanceBindingCallbacks*, void>)GetMethod("object_set_instance_binding", p_get_proc_address);
        GDExtensionInterfaceObjectSetInstance = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("object_set_instance", p_get_proc_address);
        GDExtensionInterfaceObjectGetClassName = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("object_get_class_name", p_get_proc_address);
        GDExtensionInterfaceObjectCastTo = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, delegate* unmanaged[Cdecl]<void>>)GetMethod("object_cast_to", p_get_proc_address);
        GDExtensionInterfaceObjectGetInstanceFromId = (delegate* unmanaged[Cdecl]<GDObjectInstanceID, delegate* unmanaged[Cdecl]<void>>)GetMethod("object_get_instance_from_id", p_get_proc_address);
        GDExtensionInterfaceObjectGetInstanceId = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDObjectInstanceID>)GetMethod("object_get_instance_id", p_get_proc_address);
        GDExtensionInterfaceRefGetObject = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("ref_get_object", p_get_proc_address);
        GDExtensionInterfaceRefSetObject = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("ref_set_object", p_get_proc_address);
        GDExtensionInterfaceScriptInstanceCreate = (delegate* unmanaged[Cdecl]<GDExtensionScriptInstanceInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("script_instance_create", p_get_proc_address);
        GDExtensionInterfaceClassdbConstructObject = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("classdb_construct_object", p_get_proc_address);
        GDExtensionInterfaceClassdbGetMethodBind = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("classdb_get_method_bind", p_get_proc_address);
        GDExtensionInterfaceClassdbGetClassTag = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*>)GetMethod("classdb_get_class_tag", p_get_proc_address);
        GDExtensionInterfaceClassdbRegisterExtensionClass = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassCreationInfo*, void>)GetMethod("classdb_register_extension_class", p_get_proc_address);
        GDExtensionInterfaceClassdbRegisterExtensionClassMethod = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassMethodInfo*, void>)GetMethod("classdb_register_extension_class_method", p_get_proc_address);
        GDExtensionInterfaceClassdbRegisterExtensionClassIntegerConstant = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionBool, void>)GetMethod("classdb_register_extension_class_integer_constant", p_get_proc_address);
        GDExtensionInterfaceClassdbRegisterExtensionClassProperty = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_register_extension_class_property", p_get_proc_address);
        GDExtensionInterfaceClassdbRegisterExtensionClassPropertyGroup = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_register_extension_class_property_group", p_get_proc_address);
        GDExtensionInterfaceClassdbRegisterExtensionClassPropertySubgroup = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_register_extension_class_property_subgroup", p_get_proc_address);
        GDExtensionInterfaceClassdbRegisterExtensionClassSignal = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, GDExtensionInt, void>)GetMethod("classdb_register_extension_class_signal", p_get_proc_address);
        GDExtensionInterfaceClassdbUnregisterExtensionClass = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_unregister_extension_class", p_get_proc_address);
        GDExtensionInterfaceGetLibraryPath = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("get_library_path", p_get_proc_address);
        GDExtensionInterfaceEditorAddPlugin = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("editor_add_plugin", p_get_proc_address);
        GDExtensionInterfaceEditorRemovePlugin = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("editor_remove_plugin", p_get_proc_address);
        // ReSharper restore StringLiteralTypo
    }

    internal delegate* unmanaged[Cdecl]<GDExtensionGodotVersion*, void> GDExtensionInterfaceGetGodotVersion;

    internal delegate* unmanaged[Cdecl]<size_t, void*> GDExtensionInterfaceMemAlloc;

    internal delegate* unmanaged[Cdecl]<void*, size_t, void*> GDExtensionInterfaceMemRealloc;

    internal delegate* unmanaged[Cdecl]<void*, void> GDExtensionInterfaceMemFree;

    internal delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void> GDExtensionInterfacePrintError;

    internal delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void> GDExtensionInterfacePrintErrorWithMessage;

    internal delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void> GDExtensionInterfacePrintWarning;

    internal delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void> GDExtensionInterfacePrintWarningWithMessage;

    internal delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void> GDExtensionInterfacePrintScriptError;

    internal delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void> GDExtensionInterfacePrintScriptErrorWithMessage;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint64_t> GDExtensionInterfaceGetNativeStructSize;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantNewCopy;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantNewNil;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantDestroy;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> GDExtensionInterfaceVariantCall;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> GDExtensionInterfaceVariantCallStatic;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantEvaluate;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantSet;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantSetNamed;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantSetKeyed;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void> GDExtensionInterfaceVariantSetIndexed;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantGet;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantGetNamed;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantGetKeyed;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void> GDExtensionInterfaceVariantGetIndexed;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool> GDExtensionInterfaceVariantIterInit;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool> GDExtensionInterfaceVariantIterNext;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> GDExtensionInterfaceVariantIterGet;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt> GDExtensionInterfaceVariantHash;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionInt> GDExtensionInterfaceVariantRecursiveHash;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> GDExtensionInterfaceVariantHashCompare;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool> GDExtensionInterfaceVariantBooleanize;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool, void> GDExtensionInterfaceVariantDuplicate;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantStringify;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType> GDExtensionInterfaceVariantGetType;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> GDExtensionInterfaceVariantHasMethod;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> GDExtensionInterfaceVariantHasMember;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool> GDExtensionInterfaceVariantHasKey;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetTypeName;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool> GDExtensionInterfaceVariantCanConvert;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool> GDExtensionInterfaceVariantCanConvertStrict;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceGetVariantFromTypeConstructor;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceGetVariantToTypeConstructor;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, GDExtensionVariantType, GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrOperatorEvaluator;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, int, void>> GDExtensionInterfaceVariantGetPtrBuiltinMethod;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, int32_t, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, void>> GDExtensionInterfaceVariantGetPtrConstructor;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrDestructor;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int32_t, GDExtensionCallError*, void> GDExtensionInterfaceVariantConstruct;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrSetter;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrGetter;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrIndexedSetter;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrIndexedGetter;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrKeyedSetter;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> GDExtensionInterfaceVariantGetPtrKeyedGetter;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, uint32_t>> GDExtensionInterfaceVariantGetPtrKeyedChecker;

    internal delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetConstantValue;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int, void>> GDExtensionInterfaceVariantGetPtrUtilityFunction;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void> GDExtensionInterfaceStringNewWithLatin1Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void> GDExtensionInterfaceStringNewWithUtf8Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, void> GDExtensionInterfaceStringNewWithUtf16Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void> GDExtensionInterfaceStringNewWithUtf32Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void> GDExtensionInterfaceStringNewWithWideChars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void> GDExtensionInterfaceStringNewWithLatin1CharsAndLen;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void> GDExtensionInterfaceStringNewWithUtf8CharsAndLen;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, void> GDExtensionInterfaceStringNewWithUtf16CharsAndLen;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, void> GDExtensionInterfaceStringNewWithUtf32CharsAndLen;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, void> GDExtensionInterfaceStringNewWithWideCharsAndLen;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt> GDExtensionInterfaceStringToLatin1Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt> GDExtensionInterfaceStringToUtf8Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, GDExtensionInt> GDExtensionInterfaceStringToUtf16Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, GDExtensionInt> GDExtensionInterfaceStringToUtf32Chars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, GDExtensionInt> GDExtensionInterfaceStringToWideChars;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*> GDExtensionInterfaceStringOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*> GDExtensionInterfaceStringOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceStringOperatorPlusEqString;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t, void> GDExtensionInterfaceStringOperatorPlusEqChar;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void> GDExtensionInterfaceStringOperatorPlusEqCstr;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void> GDExtensionInterfaceStringOperatorPlusEqWcstr;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void> GDExtensionInterfaceStringOperatorPlusEqC32str;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, size_t, GDExtensionInt> GDExtensionInterfaceXmlParserOpenBuffer;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, void> GDExtensionInterfaceFileAccessStoreBuffer;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, uint64_t> GDExtensionInterfaceFileAccessGetBuffer;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*> GDExtensionInterfacePackedByteArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*> GDExtensionInterfacePackedByteArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedColorArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedColorArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*> GDExtensionInterfacePackedFloat32ArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*> GDExtensionInterfacePackedFloat32ArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*> GDExtensionInterfacePackedFloat64ArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*> GDExtensionInterfacePackedFloat64ArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*> GDExtensionInterfacePackedInt32ArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*> GDExtensionInterfacePackedInt32ArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*> GDExtensionInterfacePackedInt64ArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*> GDExtensionInterfacePackedInt64ArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedStringArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedStringArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedVector2ArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedVector2ArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedVector3ArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfacePackedVector3ArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceArrayOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceArrayOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceArrayRef;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceArraySetTyped;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceDictionaryOperatorIndex;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceDictionaryOperatorIndexConst;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> GDExtensionInterfaceObjectMethodBindCall;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceObjectMethodBindPtrcall;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceObjectDestroy;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceGlobalGetSingleton;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, GDExtensionInstanceBindingCallbacks*, void*> GDExtensionInterfaceObjectGetInstanceBinding;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, void*, GDExtensionInstanceBindingCallbacks*, void> GDExtensionInterfaceObjectSetInstanceBinding;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceObjectSetInstance;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> GDExtensionInterfaceObjectGetClassName;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceObjectCastTo;

    internal delegate* unmanaged[Cdecl]<GDObjectInstanceID, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceObjectGetInstanceFromId;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDObjectInstanceID> GDExtensionInterfaceObjectGetInstanceId;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceRefGetObject;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceRefSetObject;

    internal delegate* unmanaged[Cdecl]<GDExtensionScriptInstanceInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceScriptInstanceCreate;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceClassdbConstructObject;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> GDExtensionInterfaceClassdbGetMethodBind;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*> GDExtensionInterfaceClassdbGetClassTag;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassCreationInfo*, void> GDExtensionInterfaceClassdbRegisterExtensionClass;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassMethodInfo*, void> GDExtensionInterfaceClassdbRegisterExtensionClassMethod;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionBool, void> GDExtensionInterfaceClassdbRegisterExtensionClassIntegerConstant;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceClassdbRegisterExtensionClassProperty;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceClassdbRegisterExtensionClassPropertyGroup;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceClassdbRegisterExtensionClassPropertySubgroup;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, GDExtensionInt, void> GDExtensionInterfaceClassdbRegisterExtensionClassSignal;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceClassdbUnregisterExtensionClass;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceGetLibraryPath;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceEditorAddPlugin;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceEditorRemovePlugin;

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionCallError
{
    internal GDExtensionCallErrorType error;

    internal int32_t argument;

    internal int32_t expected;

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionInstanceBindingCallbacks
{
    internal delegate* unmanaged[Cdecl]<void*, void*, void*> create_callback;

    internal delegate* unmanaged[Cdecl]<void*, void*, void*, void> free_callback;

    internal delegate* unmanaged[Cdecl]<void*, void*, GDExtensionBool, GDExtensionBool> reference_callback;

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionPropertyInfo
{
    internal GDExtensionVariantType type;

    internal delegate* unmanaged[Cdecl]<void> name;

    internal delegate* unmanaged[Cdecl]<void> class_name;

    internal uint32_t hint; // Bitfield of `PropertyHint` (defined in `extension_api.json`).

    internal delegate* unmanaged[Cdecl]<void> hint_string;

    internal uint32_t usage; // Bitfield of `PropertyUsageFlags` (defined in `extension_api.json`).

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionMethodInfo
{
    internal delegate* unmanaged[Cdecl]<void> name;

    internal GDExtensionPropertyInfo return_value;

    internal uint32_t flags; // Bitfield of `GDExtensionClassMethodFlags`.

    internal int32_t id;

    /// <summary>
    /// Arguments: `default_arguments` is an array of size `argument_count`.<br/>
    /// </summary>
    internal uint32_t argument_count;

    internal GDExtensionPropertyInfo* arguments;

    /// <summary>
    /// Default arguments: `default_arguments` is an array of size `default_argument_count`.<br/>
    /// </summary>
    internal uint32_t default_argument_count;

    internal delegate* unmanaged[Cdecl]<void>* default_arguments;

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionClassCreationInfo
{
    internal GDExtensionBool is_virtual;

    internal GDExtensionBool is_abstract;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> set_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> get_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint32_t*, GDExtensionPropertyInfo*> get_property_list_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, void> free_property_list_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> property_can_revert_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> property_get_revert_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, int32_t, void> notification_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, delegate* unmanaged[Cdecl]<void>, void> to_string_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> reference_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> unreference_func;

    internal delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void>> create_instance_func; // (Default) constructor; mandatory. If the class is not instantiable, consider making it virtual or abstract.

    internal delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void>, void> free_instance_func; // Destructor; mandatory.

    internal delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, void>> get_virtual_func; // Queries a virtual function by name and returns a callback to invoke the requested virtual function.

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint64_t> get_rid_func;

    internal void* class_userdata; // Per-class user data, later accessible in instance bindings.

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionClassMethodInfo
{
    internal delegate* unmanaged[Cdecl]<void> name;

    internal void* method_userdata;

    internal delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> call_func;

    internal delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, void> ptrcall_func;

    internal uint32_t method_flags; // Bitfield of `GDExtensionClassMethodFlags`.

    /// <summary>
    /// If `has_return_value` is false, `return_value_info` and `return_value_metadata` are ignored.<br/>
    /// </summary>
    internal GDExtensionBool has_return_value;

    internal GDExtensionPropertyInfo* return_value_info;

    internal GDExtensionClassMethodArgumentMetadata return_value_metadata;

    /// <summary>
    /// Arguments: `arguments_info` and `arguments_metadata` are array of size `argument_count`.<br/>
    /// 	 * Name and hint information for the argument can be omitted in release builds. Class name should always be present if it applies.<br/>
    /// </summary>
    internal uint32_t argument_count;

    internal GDExtensionPropertyInfo* arguments_info;

    internal GDExtensionClassMethodArgumentMetadata* arguments_metadata;

    /// <summary>
    /// Default arguments: `default_arguments` is an array of size `default_argument_count`.<br/>
    /// </summary>
    internal uint32_t default_argument_count;

    internal delegate* unmanaged[Cdecl]<void>* default_arguments;

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionScriptInstanceInfo
{
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> set_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> get_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint32_t*, GDExtensionPropertyInfo*> get_property_list_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, void> free_property_list_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> property_can_revert_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> property_get_revert_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> get_owner_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void*, void>, void*, void> get_property_state_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint32_t*, GDExtensionMethodInfo*> get_method_list_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionMethodInfo*, void> free_method_list_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionVariantType> get_property_type_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> has_method_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> call_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, int32_t, void> notification_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, delegate* unmanaged[Cdecl]<void>, void> to_string_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> refcount_incremented_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool> refcount_decremented_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> get_script_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool> is_placeholder_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> set_fallback_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> get_fallback_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> get_language_func;

    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> free_func;

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionInitialization
{
    /// <summary>
    /// Minimum initialization level required.<br/>
    /// 	 * If Core or Servers, the extension needs editor or game restart to take effect<br/>
    /// </summary>
    internal GDExtensionInitializationLevel minimum_initialization_level;

    /// <summary>
    /// Up to the user to supply when initializing<br/>
    /// </summary>
    internal void* userdata;

    /// <summary>
    /// This function will be called multiple times for each initialization level.<br/>
    /// <br/>
    /// delegate void* initialize(void* userdata, GDExtensionInitializationLevel p_level)
    /// </summary>
    internal delegate* unmanaged[Cdecl]<void*, GDExtensionInitializationLevel, void*> initialize;

    /// <summary>
    /// delegate void* deinitialize(void* userdata, GDExtensionInitializationLevel p_level)
    /// </summary>
    internal delegate* unmanaged[Cdecl]<void*, GDExtensionInitializationLevel, void*> deinitialize;

}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct GDExtensionGodotVersion
{
    internal uint32_t major;

    internal uint32_t minor;

    internal uint32_t patch;

    internal byte* @string;

}

