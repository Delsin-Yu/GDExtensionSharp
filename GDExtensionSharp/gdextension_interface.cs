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
        get_godot_version = (delegate* unmanaged[Cdecl]<GDExtensionGodotVersion*, void>)GetMethod("get_godot_version", p_get_proc_address);
        mem_alloc = (delegate* unmanaged[Cdecl]<size_t, void*>)GetMethod("mem_alloc", p_get_proc_address);
        mem_realloc = (delegate* unmanaged[Cdecl]<void*, size_t, void*>)GetMethod("mem_realloc", p_get_proc_address);
        mem_free = (delegate* unmanaged[Cdecl]<void*, void>)GetMethod("mem_free", p_get_proc_address);
        print_error = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_error", p_get_proc_address);
        print_error_with_message = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_error_with_message", p_get_proc_address);
        print_warning = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_warning", p_get_proc_address);
        print_warning_with_message = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_warning_with_message", p_get_proc_address);
        print_script_error = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_script_error", p_get_proc_address);
        print_script_error_with_message = (delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void>)GetMethod("print_script_error_with_message", p_get_proc_address);
        get_native_struct_size = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint64_t>)GetMethod("get_native_struct_size", p_get_proc_address);
        variant_new_copy = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_new_copy", p_get_proc_address);
        variant_new_nil = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_new_nil", p_get_proc_address);
        variant_destroy = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_destroy", p_get_proc_address);
        variant_call = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void>)GetMethod("variant_call", p_get_proc_address);
        variant_call_static = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void>)GetMethod("variant_call_static", p_get_proc_address);
        variant_evaluate = (delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_evaluate", p_get_proc_address);
        variant_set = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_set", p_get_proc_address);
        variant_set_named = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_set_named", p_get_proc_address);
        variant_set_keyed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_set_keyed", p_get_proc_address);
        variant_set_indexed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void>)GetMethod("variant_set_indexed", p_get_proc_address);
        variant_get = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_get", p_get_proc_address);
        variant_get_named = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_get_named", p_get_proc_address);
        variant_get_keyed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_get_keyed", p_get_proc_address);
        variant_get_indexed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void>)GetMethod("variant_get_indexed", p_get_proc_address);
        variant_iter_init = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool>)GetMethod("variant_iter_init", p_get_proc_address);
        variant_iter_next = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool>)GetMethod("variant_iter_next", p_get_proc_address);
        variant_iter_get = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void>)GetMethod("variant_iter_get", p_get_proc_address);
        variant_hash = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt>)GetMethod("variant_hash", p_get_proc_address);
        variant_recursive_hash = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionInt>)GetMethod("variant_recursive_hash", p_get_proc_address);
        variant_hash_compare = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_hash_compare", p_get_proc_address);
        variant_booleanize = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_booleanize", p_get_proc_address);
        variant_duplicate = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool, void>)GetMethod("variant_duplicate", p_get_proc_address);
        variant_stringify = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_stringify", p_get_proc_address);
        variant_get_type = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType>)GetMethod("variant_get_type", p_get_proc_address);
        variant_has_method = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_has_method", p_get_proc_address);
        variant_has_member = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("variant_has_member", p_get_proc_address);
        variant_has_key = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool>)GetMethod("variant_has_key", p_get_proc_address);
        variant_get_type_name = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_get_type_name", p_get_proc_address);
        variant_can_convert = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool>)GetMethod("variant_can_convert", p_get_proc_address);
        variant_can_convert_strict = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool>)GetMethod("variant_can_convert_strict", p_get_proc_address);
        get_variant_from_type_constructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("get_variant_from_type_constructor", p_get_proc_address);
        get_variant_to_type_constructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("get_variant_to_type_constructor", p_get_proc_address);
        variant_get_ptr_operator_evaluator = (delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, GDExtensionVariantType, GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_operator_evaluator", p_get_proc_address);
        variant_get_ptr_builtin_method = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, int, void>>)GetMethod("variant_get_ptr_builtin_method", p_get_proc_address);
        variant_get_ptr_constructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, int32_t, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, void>>)GetMethod("variant_get_ptr_constructor", p_get_proc_address);
        variant_get_ptr_destructor = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_destructor", p_get_proc_address);
        variant_construct = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int32_t, GDExtensionCallError*, void>)GetMethod("variant_construct", p_get_proc_address);
        variant_get_ptr_setter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_setter", p_get_proc_address);
        variant_get_ptr_getter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_getter", p_get_proc_address);
        variant_get_ptr_indexed_setter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_indexed_setter", p_get_proc_address);
        variant_get_ptr_indexed_getter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_indexed_getter", p_get_proc_address);
        variant_get_ptr_keyed_setter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_keyed_setter", p_get_proc_address);
        variant_get_ptr_keyed_getter = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>>)GetMethod("variant_get_ptr_keyed_getter", p_get_proc_address);
        variant_get_ptr_keyed_checker = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, uint32_t>>)GetMethod("variant_get_ptr_keyed_checker", p_get_proc_address);
        variant_get_constant_value = (delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("variant_get_constant_value", p_get_proc_address);
        variant_get_ptr_utility_function = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int, void>>)GetMethod("variant_get_ptr_utility_function", p_get_proc_address);
        string_new_with_latin1_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void>)GetMethod("string_new_with_latin1_chars", p_get_proc_address);
        string_new_with_utf8_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void>)GetMethod("string_new_with_utf8_chars", p_get_proc_address);
        string_new_with_utf16_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, void>)GetMethod("string_new_with_utf16_chars", p_get_proc_address);
        string_new_with_utf32_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void>)GetMethod("string_new_with_utf32_chars", p_get_proc_address);
        string_new_with_wide_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void>)GetMethod("string_new_with_wide_chars", p_get_proc_address);
        string_new_with_latin1_chars_and_len = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void>)GetMethod("string_new_with_latin1_chars_and_len", p_get_proc_address);
        string_new_with_utf8_chars_and_len = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void>)GetMethod("string_new_with_utf8_chars_and_len", p_get_proc_address);
        string_new_with_utf16_chars_and_len = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, void>)GetMethod("string_new_with_utf16_chars_and_len", p_get_proc_address);
        string_new_with_utf32_chars_and_len = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, void>)GetMethod("string_new_with_utf32_chars_and_len", p_get_proc_address);
        string_new_with_wide_chars_and_len = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, void>)GetMethod("string_new_with_wide_chars_and_len", p_get_proc_address);
        string_to_latin1_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_latin1_chars", p_get_proc_address);
        string_to_utf8_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_utf8_chars", p_get_proc_address);
        string_to_utf16_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_utf16_chars", p_get_proc_address);
        string_to_utf32_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_utf32_chars", p_get_proc_address);
        string_to_wide_chars = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, GDExtensionInt>)GetMethod("string_to_wide_chars", p_get_proc_address);
        string_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*>)GetMethod("string_operator_index", p_get_proc_address);
        string_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*>)GetMethod("string_operator_index_const", p_get_proc_address);
        string_operator_plus_eq_string = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("string_operator_plus_eq_string", p_get_proc_address);
        string_operator_plus_eq_char = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t, void>)GetMethod("string_operator_plus_eq_char", p_get_proc_address);
        string_operator_plus_eq_cstr = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void>)GetMethod("string_operator_plus_eq_cstr", p_get_proc_address);
        string_operator_plus_eq_wcstr = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void>)GetMethod("string_operator_plus_eq_wcstr", p_get_proc_address);
        string_operator_plus_eq_c32str = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void>)GetMethod("string_operator_plus_eq_c32str", p_get_proc_address);
        xml_parser_open_buffer = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, size_t, GDExtensionInt>)GetMethod("xml_parser_open_buffer", p_get_proc_address);
        file_access_store_buffer = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, void>)GetMethod("file_access_store_buffer", p_get_proc_address);
        file_access_get_buffer = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, uint64_t>)GetMethod("file_access_get_buffer", p_get_proc_address);
        worker_thread_pool_add_native_group_task = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*>)GetMethod("worker_thread_pool_add_native_group_task", p_get_proc_address);
        packed_byte_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*>)GetMethod("packed_byte_array_operator_index_const", p_get_proc_address);
        packed_color_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_color_array_operator_index", p_get_proc_address);
        packed_color_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_color_array_operator_index_const", p_get_proc_address);
        packed_float32_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*>)GetMethod("packed_float32_array_operator_index", p_get_proc_address);
        packed_float32_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*>)GetMethod("packed_float32_array_operator_index_const", p_get_proc_address);
        packed_float64_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*>)GetMethod("packed_float64_array_operator_index", p_get_proc_address);
        packed_float64_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*>)GetMethod("packed_float64_array_operator_index_const", p_get_proc_address);
        packed_int32_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*>)GetMethod("packed_int32_array_operator_index", p_get_proc_address);
        packed_int32_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*>)GetMethod("packed_int32_array_operator_index_const", p_get_proc_address);
        packed_int64_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*>)GetMethod("packed_int64_array_operator_index", p_get_proc_address);
        packed_int64_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*>)GetMethod("packed_int64_array_operator_index_const", p_get_proc_address);
        packed_string_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_string_array_operator_index", p_get_proc_address);
        packed_string_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_string_array_operator_index_const", p_get_proc_address);
        packed_vector2_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector2_array_operator_index", p_get_proc_address);
        packed_vector2_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector2_array_operator_index_const", p_get_proc_address);
        packed_vector3_array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector3_array_operator_index", p_get_proc_address);
        packed_vector3_array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("packed_vector3_array_operator_index_const", p_get_proc_address);
        array_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("array_operator_index", p_get_proc_address);
        array_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("array_operator_index_const", p_get_proc_address);
        array_ref = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("array_ref", p_get_proc_address);
        array_set_typed = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("array_set_typed", p_get_proc_address);
        dictionary_operator_index = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("dictionary_operator_index", p_get_proc_address);
        dictionary_operator_index_const = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("dictionary_operator_index_const", p_get_proc_address);
        object_method_bind_call = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void>)GetMethod("object_method_bind_call", p_get_proc_address);
        object_method_bind_ptrcall = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("object_method_bind_ptrcall", p_get_proc_address);
        object_destroy = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("object_destroy", p_get_proc_address);
        global_get_singleton = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("global_get_singleton", p_get_proc_address);
        object_get_instance_binding = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, GDExtensionInstanceBindingCallbacks*, void*>)GetMethod("object_get_instance_binding", p_get_proc_address);
        object_set_instance_binding = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, void*, GDExtensionInstanceBindingCallbacks*, void>)GetMethod("object_set_instance_binding", p_get_proc_address);
        object_set_instance = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("object_set_instance", p_get_proc_address);
        object_get_class_name = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool>)GetMethod("object_get_class_name", p_get_proc_address);
        object_cast_to = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, delegate* unmanaged[Cdecl]<void>>)GetMethod("object_cast_to", p_get_proc_address);
        object_get_instance_from_id = (delegate* unmanaged[Cdecl]<GDObjectInstanceID, delegate* unmanaged[Cdecl]<void>>)GetMethod("object_get_instance_from_id", p_get_proc_address);
        object_get_instance_id = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDObjectInstanceID>)GetMethod("object_get_instance_id", p_get_proc_address);
        ref_get_object = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("ref_get_object", p_get_proc_address);
        ref_set_object = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("ref_set_object", p_get_proc_address);
        script_instance_create = (delegate* unmanaged[Cdecl]<GDExtensionScriptInstanceInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("script_instance_create", p_get_proc_address);
        classdb_construct_object = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>>)GetMethod("classdb_construct_object", p_get_proc_address);
        classdb_get_method_bind = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>>)GetMethod("classdb_get_method_bind", p_get_proc_address);
        classdb_get_class_tag = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*>)GetMethod("classdb_get_class_tag", p_get_proc_address);
        classdb_register_extension_class = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassCreationInfo*, void>)GetMethod("classdb_register_extension_class", p_get_proc_address);
        classdb_register_extension_class_method = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassMethodInfo*, void>)GetMethod("classdb_register_extension_class_method", p_get_proc_address);
        classdb_register_extension_class_integer_constant = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionBool, void>)GetMethod("classdb_register_extension_class_integer_constant", p_get_proc_address);
        classdb_register_extension_class_property = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_register_extension_class_property", p_get_proc_address);
        classdb_register_extension_class_property_group = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_register_extension_class_property_group", p_get_proc_address);
        classdb_register_extension_class_property_subgroup = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_register_extension_class_property_subgroup", p_get_proc_address);
        classdb_register_extension_class_signal = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, GDExtensionInt, void>)GetMethod("classdb_register_extension_class_signal", p_get_proc_address);
        classdb_unregister_extension_class = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("classdb_unregister_extension_class", p_get_proc_address);
        get_library_path = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>)GetMethod("get_library_path", p_get_proc_address);
        editor_add_plugin = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("editor_add_plugin", p_get_proc_address);
        editor_remove_plugin = (delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>)GetMethod("editor_remove_plugin", p_get_proc_address);
        // ReSharper restore StringLiteralTypo
    }

#region Delegate Methods

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_godot_version">A pointer to the structure to write the version information into.</param>
    internal void GDExtensionInterfaceGetGodotVersion(GDExtensionGodotVersion* r_godot_version) => 
        get_godot_version(r_godot_version);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_bytes">The amount of memory to allocate in bytes.</param>
    /// <returns>A pointer to the allocated memory, or NULL if unsuccessful.</returns>
    internal void* GDExtensionInterfaceMemAlloc(size_t p_bytes) => 
        mem_alloc(p_bytes);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_ptr">A pointer to the previously allocated memory.</param>
    /// <param name="p_bytes">The number of bytes to resize the memory block to.</param>
    /// <returns>A pointer to the allocated memory, or NULL if unsuccessful.</returns>
    internal void* GDExtensionInterfaceMemRealloc(void* p_ptr, size_t p_bytes) => 
        mem_realloc(p_ptr, p_bytes);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_ptr">A pointer to the previously allocated memory.</param>
    internal void GDExtensionInterfaceMemFree(void* p_ptr) => 
        mem_free(p_ptr);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_description">The code trigging the error.</param>
    /// <param name="p_function">The function name where the error occurred.</param>
    /// <param name="p_file">The file where the error occurred.</param>
    /// <param name="p_line">The line where the error occurred.</param>
    /// <param name="p_editor_notify">Whether or not to notify the editor.</param>
    internal void GDExtensionInterfacePrintError(byte* p_description, byte* p_function, byte* p_file, int32_t p_line, GDExtensionBool p_editor_notify) => 
        print_error(p_description, p_function, p_file, p_line, p_editor_notify);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_description">The code trigging the error.</param>
    /// <param name="p_message">The message to show along with the error.</param>
    /// <param name="p_function">The function name where the error occurred.</param>
    /// <param name="p_file">The file where the error occurred.</param>
    /// <param name="p_line">The line where the error occurred.</param>
    /// <param name="p_editor_notify">Whether or not to notify the editor.</param>
    internal void GDExtensionInterfacePrintErrorWithMessage(byte* p_description, byte* p_message, byte* p_function, byte* p_file, int32_t p_line, GDExtensionBool p_editor_notify) => 
        print_error_with_message(p_description, p_message, p_function, p_file, p_line, p_editor_notify);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_description">The code trigging the warning.</param>
    /// <param name="p_function">The function name where the warning occurred.</param>
    /// <param name="p_file">The file where the warning occurred.</param>
    /// <param name="p_line">The line where the warning occurred.</param>
    /// <param name="p_editor_notify">Whether or not to notify the editor.</param>
    internal void GDExtensionInterfacePrintWarning(byte* p_description, byte* p_function, byte* p_file, int32_t p_line, GDExtensionBool p_editor_notify) => 
        print_warning(p_description, p_function, p_file, p_line, p_editor_notify);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_description">The code trigging the warning.</param>
    /// <param name="p_message">The message to show along with the warning.</param>
    /// <param name="p_function">The function name where the warning occurred.</param>
    /// <param name="p_file">The file where the warning occurred.</param>
    /// <param name="p_line">The line where the warning occurred.</param>
    /// <param name="p_editor_notify">Whether or not to notify the editor.</param>
    internal void GDExtensionInterfacePrintWarningWithMessage(byte* p_description, byte* p_message, byte* p_function, byte* p_file, int32_t p_line, GDExtensionBool p_editor_notify) => 
        print_warning_with_message(p_description, p_message, p_function, p_file, p_line, p_editor_notify);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_description">The code trigging the error.</param>
    /// <param name="p_function">The function name where the error occurred.</param>
    /// <param name="p_file">The file where the error occurred.</param>
    /// <param name="p_line">The line where the error occurred.</param>
    /// <param name="p_editor_notify">Whether or not to notify the editor.</param>
    internal void GDExtensionInterfacePrintScriptError(byte* p_description, byte* p_function, byte* p_file, int32_t p_line, GDExtensionBool p_editor_notify) => 
        print_script_error(p_description, p_function, p_file, p_line, p_editor_notify);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_description">The code trigging the error.</param>
    /// <param name="p_message">The message to show along with the error.</param>
    /// <param name="p_function">The function name where the error occurred.</param>
    /// <param name="p_file">The file where the error occurred.</param>
    /// <param name="p_line">The line where the error occurred.</param>
    /// <param name="p_editor_notify">Whether or not to notify the editor.</param>
    internal void GDExtensionInterfacePrintScriptErrorWithMessage(byte* p_description, byte* p_message, byte* p_function, byte* p_file, int32_t p_line, GDExtensionBool p_editor_notify) => 
        print_script_error_with_message(p_description, p_message, p_function, p_file, p_line, p_editor_notify);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_name">A pointer to a StringName identifying the struct name.</param>
    /// <returns>The size in bytes.</returns>
    internal uint64_t GDExtensionInterfaceGetNativeStructSize(delegate* unmanaged[Cdecl]<void> p_name) => 
        get_native_struct_size(p_name);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to the destination Variant.</param>
    /// <param name="p_src">A pointer to the source Variant.</param>
    internal void GDExtensionInterfaceVariantNewCopy(delegate* unmanaged[Cdecl]<void> r_dest, delegate* unmanaged[Cdecl]<void> p_src) => 
        variant_new_copy(r_dest, p_src);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to the destination Variant.</param>
    internal void GDExtensionInterfaceVariantNewNil(delegate* unmanaged[Cdecl]<void> r_dest) => 
        variant_new_nil(r_dest);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant to destroy.</param>
    internal void GDExtensionInterfaceVariantDestroy(delegate* unmanaged[Cdecl]<void> p_self) => 
        variant_destroy(p_self);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::callp()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_method">A pointer to a StringName identifying the method.</param>
    /// <param name="p_args">A pointer to a C array of Variant.</param>
    /// <param name="p_argument_count">The number of arguments.</param>
    /// <param name="r_return">A pointer a Variant which will be assigned the return value.</param>
    /// <param name="r_error">A pointer the structure which will hold error information.</param>
    internal void GDExtensionInterfaceVariantCall(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_method, delegate* unmanaged[Cdecl]<void>* p_args, GDExtensionInt p_argument_count, delegate* unmanaged[Cdecl]<void> r_return, GDExtensionCallError* r_error) => 
        variant_call(p_self, p_method, p_args, p_argument_count, r_return, r_error);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::call_static()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_method">A pointer to a StringName identifying the method.</param>
    /// <param name="p_args">A pointer to a C array of Variant.</param>
    /// <param name="p_argument_count">The number of arguments.</param>
    /// <param name="r_return">A pointer a Variant which will be assigned the return value.</param>
    /// <param name="r_error">A pointer the structure which will be updated with error information.</param>
    internal void GDExtensionInterfaceVariantCallStatic(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> p_method, delegate* unmanaged[Cdecl]<void>* p_args, GDExtensionInt p_argument_count, delegate* unmanaged[Cdecl]<void> r_return, GDExtensionCallError* r_error) => 
        variant_call_static(p_type, p_method, p_args, p_argument_count, r_return, r_error);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::evaluate()
    /// </summary>
    /// <param name="p_op">The operator to evaluate.</param>
    /// <param name="p_a">The first Variant.</param>
    /// <param name="p_b">The second Variant.</param>
    /// <param name="r_return">A pointer a Variant which will be assigned the return value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantEvaluate(GDExtensionVariantOperator p_op, delegate* unmanaged[Cdecl]<void> p_a, delegate* unmanaged[Cdecl]<void> p_b, delegate* unmanaged[Cdecl]<void> r_return, GDExtensionBool* r_valid) => 
        variant_evaluate(p_op, p_a, p_b, r_return, r_valid);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::set()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_key">A pointer to a Variant representing the key.</param>
    /// <param name="p_value">A pointer to a Variant representing the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantSet(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key, delegate* unmanaged[Cdecl]<void> p_value, GDExtensionBool* r_valid) => 
        variant_set(p_self, p_key, p_value, r_valid);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::set_named()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_key">A pointer to a StringName representing the key.</param>
    /// <param name="p_value">A pointer to a Variant representing the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantSetNamed(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key, delegate* unmanaged[Cdecl]<void> p_value, GDExtensionBool* r_valid) => 
        variant_set_named(p_self, p_key, p_value, r_valid);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::set_keyed()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_key">A pointer to a Variant representing the key.</param>
    /// <param name="p_value">A pointer to a Variant representing the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantSetKeyed(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key, delegate* unmanaged[Cdecl]<void> p_value, GDExtensionBool* r_valid) => 
        variant_set_keyed(p_self, p_key, p_value, r_valid);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_index">The index.</param>
    /// <param name="p_value">A pointer to a Variant representing the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    /// <param name="r_oob">A pointer to a boolean which will be set to true if the index is out of bounds.</param>
    internal void GDExtensionInterfaceVariantSetIndexed(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index, delegate* unmanaged[Cdecl]<void> p_value, GDExtensionBool* r_valid, GDExtensionBool* r_oob) => 
        variant_set_indexed(p_self, p_index, p_value, r_valid, r_oob);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_key">A pointer to a Variant representing the key.</param>
    /// <param name="r_ret">A pointer to a Variant which will be assigned the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantGet(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key, delegate* unmanaged[Cdecl]<void> r_ret, GDExtensionBool* r_valid) => 
        variant_get(p_self, p_key, r_ret, r_valid);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_key">A pointer to a StringName representing the key.</param>
    /// <param name="r_ret">A pointer to a Variant which will be assigned the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantGetNamed(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key, delegate* unmanaged[Cdecl]<void> r_ret, GDExtensionBool* r_valid) => 
        variant_get_named(p_self, p_key, r_ret, r_valid);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_key">A pointer to a Variant representing the key.</param>
    /// <param name="r_ret">A pointer to a Variant which will be assigned the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantGetKeyed(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key, delegate* unmanaged[Cdecl]<void> r_ret, GDExtensionBool* r_valid) => 
        variant_get_keyed(p_self, p_key, r_ret, r_valid);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_index">The index.</param>
    /// <param name="r_ret">A pointer to a Variant which will be assigned the value.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    /// <param name="r_oob">A pointer to a boolean which will be set to true if the index is out of bounds.</param>
    internal void GDExtensionInterfaceVariantGetIndexed(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index, delegate* unmanaged[Cdecl]<void> r_ret, GDExtensionBool* r_valid, GDExtensionBool* r_oob) => 
        variant_get_indexed(p_self, p_index, r_ret, r_valid, r_oob);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::iter_init()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="r_iter">A pointer to a Variant which will be assigned the iterator.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    /// <returns>true if the operation is valid; otherwise false.</returns>
    internal GDExtensionBool GDExtensionInterfaceVariantIterInit(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> r_iter, GDExtensionBool* r_valid) => 
        variant_iter_init(p_self, r_iter, r_valid);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::iter_next()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="r_iter">A pointer to a Variant which will be assigned the iterator.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    /// <returns>true if the operation is valid; otherwise false.</returns>
    internal GDExtensionBool GDExtensionInterfaceVariantIterNext(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> r_iter, GDExtensionBool* r_valid) => 
        variant_iter_next(p_self, r_iter, r_valid);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::iter_get()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="r_iter">A pointer to a Variant which will be assigned the iterator.</param>
    /// <param name="r_ret">A pointer to a Variant which will be assigned false if the operation is invalid.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the operation is invalid.</param>
    internal void GDExtensionInterfaceVariantIterGet(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> r_iter, delegate* unmanaged[Cdecl]<void> r_ret, GDExtensionBool* r_valid) => 
        variant_iter_get(p_self, r_iter, r_ret, r_valid);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::hash()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <returns>The hash value.</returns>
    internal GDExtensionInt GDExtensionInterfaceVariantHash(delegate* unmanaged[Cdecl]<void> p_self) => 
        variant_hash(p_self);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::recursive_hash()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_recursion_count">The number of recursive loops so far.</param>
    /// <returns>The hash value.</returns>
    internal GDExtensionInt GDExtensionInterfaceVariantRecursiveHash(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_recursion_count) => 
        variant_recursive_hash(p_self, p_recursion_count);

    /// <summary>
    /// Since: 4.1
    /// See: Variant::hash_compare()
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_other">A pointer to the other Variant to compare it to.</param>
    /// <returns>The hash value.</returns>
    internal GDExtensionBool GDExtensionInterfaceVariantHashCompare(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_other) => 
        variant_hash_compare(p_self, p_other);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <returns>The boolean value of the Variant.</returns>
    internal GDExtensionBool GDExtensionInterfaceVariantBooleanize(delegate* unmanaged[Cdecl]<void> p_self) => 
        variant_booleanize(p_self);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="r_ret">A pointer to a Variant to store the duplicated value.</param>
    /// <param name="p_deep">Whether or not to duplicate deeply (when supported by the Variant type).</param>
    internal void GDExtensionInterfaceVariantDuplicate(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> r_ret, GDExtensionBool p_deep) => 
        variant_duplicate(p_self, r_ret, p_deep);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="r_ret">A pointer to a String to store the resulting value.</param>
    internal void GDExtensionInterfaceVariantStringify(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> r_ret) => 
        variant_stringify(p_self, r_ret);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <returns>The variant type.</returns>
    internal GDExtensionVariantType GDExtensionInterfaceVariantGetType(delegate* unmanaged[Cdecl]<void> p_self) => 
        variant_get_type(p_self);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_method">A pointer to a StringName with the method name.</param>
    internal GDExtensionBool GDExtensionInterfaceVariantHasMethod(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_method) => 
        variant_has_method(p_self, p_method);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="p_member">A pointer to a StringName with the member name.</param>
    internal GDExtensionBool GDExtensionInterfaceVariantHasMember(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> p_member) => 
        variant_has_member(p_type, p_member);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Variant.</param>
    /// <param name="p_key">A pointer to a Variant representing the key.</param>
    /// <param name="r_valid">A pointer to a boolean which will be set to false if the key doesn't exist.</param>
    /// <returns>true if the key exists; otherwise false.</returns>
    internal GDExtensionBool GDExtensionInterfaceVariantHasKey(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key, GDExtensionBool* r_valid) => 
        variant_has_key(p_self, p_key, r_valid);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="r_name">A pointer to a String to store the Variant type name.</param>
    internal void GDExtensionInterfaceVariantGetTypeName(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> r_name) => 
        variant_get_type_name(p_type, r_name);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_from">The Variant type to convert from.</param>
    /// <param name="p_to">The Variant type to convert to.</param>
    /// <returns>true if the conversion is possible; otherwise false.</returns>
    internal GDExtensionBool GDExtensionInterfaceVariantCanConvert(GDExtensionVariantType p_from, GDExtensionVariantType p_to) => 
        variant_can_convert(p_from, p_to);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_from">The Variant type to convert from.</param>
    /// <param name="p_to">The Variant type to convert to.</param>
    /// <returns>true if the conversion is possible; otherwise false.</returns>
    internal GDExtensionBool GDExtensionInterfaceVariantCanConvertStrict(GDExtensionVariantType p_from, GDExtensionVariantType p_to) => 
        variant_can_convert_strict(p_from, p_to);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function that can create a Variant of the given type from a raw value.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceGetVariantFromTypeConstructor(GDExtensionVariantType p_type) => 
        get_variant_from_type_constructor(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function that can get the raw value from a Variant of the given type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceGetVariantToTypeConstructor(GDExtensionVariantType p_type) => 
        get_variant_to_type_constructor(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_operator">The variant operator.</param>
    /// <param name="p_type_a">The type of the first Variant.</param>
    /// <param name="p_type_b">The type of the second Variant.</param>
    /// <returns>A pointer to a function that can evaluate the given Variant operator on the given Variant types.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrOperatorEvaluator(GDExtensionVariantOperator p_operator, GDExtensionVariantType p_type_a, GDExtensionVariantType p_type_b) => 
        variant_get_ptr_operator_evaluator(p_operator, p_type_a, p_type_b);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="p_method">A pointer to a StringName with the method name.</param>
    /// <param name="p_hash">A hash representing the method signature.</param>
    /// <returns>A pointer to a function that can call a builtin method on a type of Variant.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, int, void> GDExtensionInterfaceVariantGetPtrBuiltinMethod(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> p_method, GDExtensionInt p_hash) => 
        variant_get_ptr_builtin_method(p_type, p_method, p_hash);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="p_constructor">The index of the constructor.</param>
    /// <returns>A pointer to a function that can call one of the constructors for a type of Variant.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, void> GDExtensionInterfaceVariantGetPtrConstructor(GDExtensionVariantType p_type, int32_t p_constructor) => 
        variant_get_ptr_constructor(p_type, p_constructor);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function than can call the destructor for a type of Variant.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrDestructor(GDExtensionVariantType p_type) => 
        variant_get_ptr_destructor(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="p_base">A pointer to a Variant to store the constructed value.</param>
    /// <param name="p_args">A pointer to a C array of Variant pointers representing the arguments for the constructor.</param>
    /// <param name="p_argument_count">The number of arguments to pass to the constructor.</param>
    /// <param name="r_error">A pointer the structure which will be updated with error information.</param>
    internal void GDExtensionInterfaceVariantConstruct(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> r_base, delegate* unmanaged[Cdecl]<void>* p_args, int32_t p_argument_count, GDExtensionCallError* r_error) => 
        variant_construct(p_type, r_base, p_args, p_argument_count, r_error);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="p_member">A pointer to a StringName with the member name.</param>
    /// <returns>A pointer to a function that can call a member's setter on the given Variant type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrSetter(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> p_member) => 
        variant_get_ptr_setter(p_type, p_member);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="p_member">A pointer to a StringName with the member name.</param>
    /// <returns>A pointer to a function that can call a member's getter on the given Variant type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrGetter(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> p_member) => 
        variant_get_ptr_getter(p_type, p_member);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function that can set an index on the given Variant type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrIndexedSetter(GDExtensionVariantType p_type) => 
        variant_get_ptr_indexed_setter(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function that can get an index on the given Variant type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrIndexedGetter(GDExtensionVariantType p_type) => 
        variant_get_ptr_indexed_getter(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function that can set a key on the given Variant type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrKeyedSetter(GDExtensionVariantType p_type) => 
        variant_get_ptr_keyed_setter(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function that can get a key on the given Variant type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> GDExtensionInterfaceVariantGetPtrKeyedGetter(GDExtensionVariantType p_type) => 
        variant_get_ptr_keyed_getter(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <returns>A pointer to a function that can check a key on the given Variant type.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, uint32_t> GDExtensionInterfaceVariantGetPtrKeyedChecker(GDExtensionVariantType p_type) => 
        variant_get_ptr_keyed_checker(p_type);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_type">The Variant type.</param>
    /// <param name="p_constant">A pointer to a StringName with the constant name.</param>
    /// <param name="r_ret">A pointer to a Variant to store the value.</param>
    internal void GDExtensionInterfaceVariantGetConstantValue(GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> p_constant, delegate* unmanaged[Cdecl]<void> r_ret) => 
        variant_get_constant_value(p_type, p_constant, r_ret);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_function">A pointer to a StringName with the function name.</param>
    /// <param name="p_hash">A hash representing the function signature.</param>
    /// <returns>A pointer to a function that can call a Variant utility function.</returns>
    internal delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int, void> GDExtensionInterfaceVariantGetPtrUtilityFunction(delegate* unmanaged[Cdecl]<void> p_function, GDExtensionInt p_hash) => 
        variant_get_ptr_utility_function(p_function, p_hash);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a Latin-1 encoded C string (null terminated).</param>
    internal void GDExtensionInterfaceStringNewWithLatin1Chars(delegate* unmanaged[Cdecl]<void> r_dest, byte* p_contents) => 
        string_new_with_latin1_chars(r_dest, p_contents);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a UTF-8 encoded C string (null terminated).</param>
    internal void GDExtensionInterfaceStringNewWithUtf8Chars(delegate* unmanaged[Cdecl]<void> r_dest, byte* p_contents) => 
        string_new_with_utf8_chars(r_dest, p_contents);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a UTF-16 encoded C string (null terminated).</param>
    internal void GDExtensionInterfaceStringNewWithUtf16Chars(delegate* unmanaged[Cdecl]<void> r_dest, char16_t* p_contents) => 
        string_new_with_utf16_chars(r_dest, p_contents);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a UTF-32 encoded C string (null terminated).</param>
    internal void GDExtensionInterfaceStringNewWithUtf32Chars(delegate* unmanaged[Cdecl]<void> r_dest, char32_t* p_contents) => 
        string_new_with_utf32_chars(r_dest, p_contents);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a wide C string (null terminated).</param>
    internal void GDExtensionInterfaceStringNewWithWideChars(delegate* unmanaged[Cdecl]<void> r_dest, wchar_t* p_contents) => 
        string_new_with_wide_chars(r_dest, p_contents);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a Latin-1 encoded C string.</param>
    /// <param name="p_size">The number of characters.</param>
    internal void GDExtensionInterfaceStringNewWithLatin1CharsAndLen(delegate* unmanaged[Cdecl]<void> r_dest, byte* p_contents, GDExtensionInt p_size) => 
        string_new_with_latin1_chars_and_len(r_dest, p_contents, p_size);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a UTF-8 encoded C string.</param>
    /// <param name="p_size">The number of characters.</param>
    internal void GDExtensionInterfaceStringNewWithUtf8CharsAndLen(delegate* unmanaged[Cdecl]<void> r_dest, byte* p_contents, GDExtensionInt p_size) => 
        string_new_with_utf8_chars_and_len(r_dest, p_contents, p_size);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a UTF-16 encoded C string.</param>
    /// <param name="p_size">The number of characters.</param>
    internal void GDExtensionInterfaceStringNewWithUtf16CharsAndLen(delegate* unmanaged[Cdecl]<void> r_dest, char16_t* p_contents, GDExtensionInt p_size) => 
        string_new_with_utf16_chars_and_len(r_dest, p_contents, p_size);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a UTF-32 encoded C string.</param>
    /// <param name="p_size">The number of characters.</param>
    internal void GDExtensionInterfaceStringNewWithUtf32CharsAndLen(delegate* unmanaged[Cdecl]<void> r_dest, char32_t* p_contents, GDExtensionInt p_size) => 
        string_new_with_utf32_chars_and_len(r_dest, p_contents, p_size);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="r_dest">A pointer to a Variant to hold the newly created String.</param>
    /// <param name="p_contents">A pointer to a wide C string.</param>
    /// <param name="p_size">The number of characters.</param>
    internal void GDExtensionInterfaceStringNewWithWideCharsAndLen(delegate* unmanaged[Cdecl]<void> r_dest, wchar_t* p_contents, GDExtensionInt p_size) => 
        string_new_with_wide_chars_and_len(r_dest, p_contents, p_size);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="r_text">A pointer to the buffer to hold the resulting data. If NULL is passed in, only the length will be computed.</param>
    /// <param name="p_max_write_length">The maximum number of characters that can be written to r_text. It has no affect on the return value.</param>
    /// <returns>The resulting encoded string length in characters (not bytes), not including a null terminator.</returns>
    internal GDExtensionInt GDExtensionInterfaceStringToLatin1Chars(delegate* unmanaged[Cdecl]<void> p_self, byte* r_text, GDExtensionInt p_max_write_length) => 
        string_to_latin1_chars(p_self, r_text, p_max_write_length);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="r_text">A pointer to the buffer to hold the resulting data. If NULL is passed in, only the length will be computed.</param>
    /// <param name="p_max_write_length">The maximum number of characters that can be written to r_text. It has no affect on the return value.</param>
    /// <returns>The resulting encoded string length in characters (not bytes), not including a null terminator.</returns>
    internal GDExtensionInt GDExtensionInterfaceStringToUtf8Chars(delegate* unmanaged[Cdecl]<void> p_self, byte* r_text, GDExtensionInt p_max_write_length) => 
        string_to_utf8_chars(p_self, r_text, p_max_write_length);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="r_text">A pointer to the buffer to hold the resulting data. If NULL is passed in, only the length will be computed.</param>
    /// <param name="p_max_write_length">The maximum number of characters that can be written to r_text. It has no affect on the return value.</param>
    /// <returns>The resulting encoded string length in characters (not bytes), not including a null terminator.</returns>
    internal GDExtensionInt GDExtensionInterfaceStringToUtf16Chars(delegate* unmanaged[Cdecl]<void> p_self, char16_t* r_text, GDExtensionInt p_max_write_length) => 
        string_to_utf16_chars(p_self, r_text, p_max_write_length);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="r_text">A pointer to the buffer to hold the resulting data. If NULL is passed in, only the length will be computed.</param>
    /// <param name="p_max_write_length">The maximum number of characters that can be written to r_text. It has no affect on the return value.</param>
    /// <returns>The resulting encoded string length in characters (not bytes), not including a null terminator.</returns>
    internal GDExtensionInt GDExtensionInterfaceStringToUtf32Chars(delegate* unmanaged[Cdecl]<void> p_self, char32_t* r_text, GDExtensionInt p_max_write_length) => 
        string_to_utf32_chars(p_self, r_text, p_max_write_length);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="r_text">A pointer to the buffer to hold the resulting data. If NULL is passed in, only the length will be computed.</param>
    /// <param name="p_max_write_length">The maximum number of characters that can be written to r_text. It has no affect on the return value.</param>
    /// <returns>The resulting encoded string length in characters (not bytes), not including a null terminator.</returns>
    internal GDExtensionInt GDExtensionInterfaceStringToWideChars(delegate* unmanaged[Cdecl]<void> p_self, wchar_t* r_text, GDExtensionInt p_max_write_length) => 
        string_to_wide_chars(p_self, r_text, p_max_write_length);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="p_index">The index.</param>
    /// <returns>A pointer to the requested character.</returns>
    internal char32_t* GDExtensionInterfaceStringOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        string_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="p_index">The index.</param>
    /// <returns>A const pointer to the requested character.</returns>
    internal char32_t* GDExtensionInterfaceStringOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        string_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="p_b">A pointer to the other String to append.</param>
    internal void GDExtensionInterfaceStringOperatorPlusEqString(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_b) => 
        string_operator_plus_eq_string(p_self, p_b);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="p_b">A pointer to the character to append.</param>
    internal void GDExtensionInterfaceStringOperatorPlusEqChar(delegate* unmanaged[Cdecl]<void> p_self, char32_t p_b) => 
        string_operator_plus_eq_char(p_self, p_b);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="p_b">A pointer to a Latin-1 encoded C string (null terminated).</param>
    internal void GDExtensionInterfaceStringOperatorPlusEqCstr(delegate* unmanaged[Cdecl]<void> p_self, byte* p_b) => 
        string_operator_plus_eq_cstr(p_self, p_b);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="p_b">A pointer to a wide C string (null terminated).</param>
    internal void GDExtensionInterfaceStringOperatorPlusEqWcstr(delegate* unmanaged[Cdecl]<void> p_self, wchar_t* p_b) => 
        string_operator_plus_eq_wcstr(p_self, p_b);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the String.</param>
    /// <param name="p_b">A pointer to a UTF-32 encoded C string (null terminated).</param>
    internal void GDExtensionInterfaceStringOperatorPlusEqC32str(delegate* unmanaged[Cdecl]<void> p_self, char32_t* p_b) => 
        string_operator_plus_eq_c32str(p_self, p_b);

    /// <summary>
    /// Since: 4.1
    /// See: XMLParser::open_buffer()
    /// </summary>
    /// <param name="p_instance">A pointer to an XMLParser object.</param>
    /// <param name="p_buffer">A pointer to the buffer.</param>
    /// <param name="p_size">The size of the buffer.</param>
    /// <returns>A Godot error code (ex. OK, ERR_INVALID_DATA, etc).</returns>
    internal GDExtensionInt GDExtensionInterfaceXmlParserOpenBuffer(delegate* unmanaged[Cdecl]<void> p_instance, uint8_t* p_buffer, size_t p_size) => 
        xml_parser_open_buffer(p_instance, p_buffer, p_size);

    /// <summary>
    /// Since: 4.1
    /// See: FileAccess::store_buffer()
    /// </summary>
    /// <param name="p_instance">A pointer to a FileAccess object.</param>
    /// <param name="p_src">A pointer to the buffer.</param>
    /// <param name="p_length">The size of the buffer.</param>
    internal void GDExtensionInterfaceFileAccessStoreBuffer(delegate* unmanaged[Cdecl]<void> p_instance, uint8_t* p_src, uint64_t p_length) => 
        file_access_store_buffer(p_instance, p_src, p_length);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_instance">A pointer to a FileAccess object.</param>
    /// <param name="p_dst">A pointer to the buffer to store the data.</param>
    /// <param name="p_length">The requested number of bytes to read.</param>
    /// <returns>The actual number of bytes read (may be less than requested).</returns>
    internal uint64_t GDExtensionInterfaceFileAccessGetBuffer(delegate* unmanaged[Cdecl]<void> p_instance, uint8_t* p_dst, uint64_t p_length) => 
        file_access_get_buffer(p_instance, p_dst, p_length);

    /// <summary>
    /// Since: 4.1
    /// See: WorkerThreadPool::add_group_task()
    /// </summary>
    /// <param name="p_instance">A pointer to a WorkerThreadPool object.</param>
    /// <param name="p_func">A pointer to a function to run in the thread pool.</param>
    /// <param name="p_userdata">A pointer to arbitrary data which will be passed to p_func.</param>
    /// <param name="p_tasks">The number of tasks needed in the group.</param>
    /// <param name="p_high_priority">Whether or not this is a high priority task.</param>
    /// <param name="p_description">A pointer to a String with the task description.</param>
    /// <param name="p_instance">A pointer to a WorkerThreadPool object.</param>
    /// <param name="p_func">A pointer to a function to run in the thread pool.</param>
    /// <param name="p_userdata">A pointer to arbitrary data which will be passed to p_func.</param>
    /// <param name="p_high_priority">Whether or not this is a high priority task.</param>
    /// <param name="p_description">A pointer to a String with the task description.</param>
    /// <param name="p_self">A pointer to a PackedByteArray object.</param>
    /// <param name="p_index">The index of the byte to get.</param>
    /// <returns>The task group ID.</returns>
    internal uint8_t* GDExtensionInterfacePackedByteArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        worker_thread_pool_add_native_group_task(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedByteArray object.</param>
    /// <param name="p_index">The index of the byte to get.</param>
    /// <returns>A const pointer to the requested byte.</returns>
    internal uint8_t* GDExtensionInterfacePackedByteArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_byte_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedColorArray object.</param>
    /// <param name="p_index">The index of the Color to get.</param>
    /// <returns>A pointer to the requested Color.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedColorArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_color_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a const PackedColorArray object.</param>
    /// <param name="p_index">The index of the Color to get.</param>
    /// <returns>A const pointer to the requested Color.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedColorArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_color_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedFloat32Array object.</param>
    /// <param name="p_index">The index of the float to get.</param>
    /// <returns>A pointer to the requested 32-bit float.</returns>
    internal float* GDExtensionInterfacePackedFloat32ArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_float32_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedFloat32Array object.</param>
    /// <param name="p_index">The index of the float to get.</param>
    /// <returns>A const pointer to the requested 32-bit float.</returns>
    internal float* GDExtensionInterfacePackedFloat32ArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_float32_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedFloat64Array object.</param>
    /// <param name="p_index">The index of the float to get.</param>
    /// <returns>A pointer to the requested 64-bit float.</returns>
    internal double* GDExtensionInterfacePackedFloat64ArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_float64_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedFloat64Array object.</param>
    /// <param name="p_index">The index of the float to get.</param>
    /// <returns>A const pointer to the requested 64-bit float.</returns>
    internal double* GDExtensionInterfacePackedFloat64ArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_float64_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedInt32Array object.</param>
    /// <param name="p_index">The index of the integer to get.</param>
    /// <returns>A pointer to the requested 32-bit integer.</returns>
    internal int32_t* GDExtensionInterfacePackedInt32ArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_int32_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedInt32Array object.</param>
    /// <param name="p_index">The index of the integer to get.</param>
    /// <returns>A const pointer to the requested 32-bit integer.</returns>
    internal int32_t* GDExtensionInterfacePackedInt32ArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_int32_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedInt64Array object.</param>
    /// <param name="p_index">The index of the integer to get.</param>
    /// <returns>A pointer to the requested 64-bit integer.</returns>
    internal int64_t* GDExtensionInterfacePackedInt64ArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_int64_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedInt64Array object.</param>
    /// <param name="p_index">The index of the integer to get.</param>
    /// <returns>A const pointer to the requested 64-bit integer.</returns>
    internal int64_t* GDExtensionInterfacePackedInt64ArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_int64_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedStringArray object.</param>
    /// <param name="p_index">The index of the String to get.</param>
    /// <returns>A pointer to the requested String.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedStringArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_string_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedStringArray object.</param>
    /// <param name="p_index">The index of the String to get.</param>
    /// <returns>A const pointer to the requested String.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedStringArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_string_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedVector2Array object.</param>
    /// <param name="p_index">The index of the Vector2 to get.</param>
    /// <returns>A pointer to the requested Vector2.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedVector2ArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_vector2_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedVector2Array object.</param>
    /// <param name="p_index">The index of the Vector2 to get.</param>
    /// <returns>A const pointer to the requested Vector2.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedVector2ArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_vector2_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a PackedVector3Array object.</param>
    /// <param name="p_index">The index of the Vector3 to get.</param>
    /// <returns>A pointer to the requested Vector3.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedVector3ArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_vector3_array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a PackedVector3Array object.</param>
    /// <param name="p_index">The index of the Vector3 to get.</param>
    /// <returns>A const pointer to the requested Vector3.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfacePackedVector3ArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        packed_vector3_array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to an Array object.</param>
    /// <param name="p_index">The index of the Variant to get.</param>
    /// <returns>A pointer to the requested Variant.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceArrayOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        array_operator_index(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to an Array object.</param>
    /// <param name="p_index">The index of the Variant to get.</param>
    /// <returns>A const pointer to the requested Variant.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceArrayOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionInt p_index) => 
        array_operator_index_const(p_self, p_index);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Array object to update.</param>
    /// <param name="p_from">A pointer to the Array object to reference.</param>
    internal void GDExtensionInterfaceArrayRef(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_from) => 
        array_ref(p_self, p_from);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to the Array.</param>
    /// <param name="p_type">The type of Variant the Array will store.</param>
    /// <param name="p_class_name">A pointer to a StringName with the name of the object (if p_type is GDEXTENSION_VARIANT_TYPE_OBJECT).</param>
    /// <param name="p_script">A pointer to a Script object (if p_type is GDEXTENSION_VARIANT_TYPE_OBJECT and the base class is extended by a script).</param>
    internal void GDExtensionInterfaceArraySetTyped(delegate* unmanaged[Cdecl]<void> p_self, GDExtensionVariantType p_type, delegate* unmanaged[Cdecl]<void> p_class_name, delegate* unmanaged[Cdecl]<void> p_script) => 
        array_set_typed(p_self, p_type, p_class_name, p_script);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A pointer to a Dictionary object.</param>
    /// <param name="p_key">A pointer to a Variant representing the key.</param>
    /// <returns>A pointer to a Variant representing the value at the given key.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceDictionaryOperatorIndex(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key) => 
        dictionary_operator_index(p_self, p_key);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_self">A const pointer to a Dictionary object.</param>
    /// <param name="p_key">A pointer to a Variant representing the key.</param>
    /// <returns>A const pointer to a Variant representing the value at the given key.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceDictionaryOperatorIndexConst(delegate* unmanaged[Cdecl]<void> p_self, delegate* unmanaged[Cdecl]<void> p_key) => 
        dictionary_operator_index_const(p_self, p_key);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_method_bind">A pointer to the MethodBind representing the method on the Object's class.</param>
    /// <param name="p_instance">A pointer to the Object.</param>
    /// <param name="p_args">A pointer to a C array of Variants representing the arguments.</param>
    /// <param name="p_arg_count">The number of arguments.</param>
    /// <param name="r_ret">A pointer to Variant which will receive the return value.</param>
    /// <param name="r_error">A pointer to a GDExtensionCallError struct that will receive error information.</param>
    internal void GDExtensionInterfaceObjectMethodBindCall(delegate* unmanaged[Cdecl]<void> p_method_bind, delegate* unmanaged[Cdecl]<void> p_instance, delegate* unmanaged[Cdecl]<void>* p_args, GDExtensionInt p_arg_count, delegate* unmanaged[Cdecl]<void> r_ret, GDExtensionCallError* r_error) => 
        object_method_bind_call(p_method_bind, p_instance, p_args, p_arg_count, r_ret, r_error);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_method_bind">A pointer to the MethodBind representing the method on the Object's class.</param>
    /// <param name="p_instance">A pointer to the Object.</param>
    /// <param name="p_args">A pointer to a C array representing the arguments.</param>
    /// <param name="r_ret">A pointer to the Object that will receive the return value.</param>
    internal void GDExtensionInterfaceObjectMethodBindPtrcall(delegate* unmanaged[Cdecl]<void> p_method_bind, delegate* unmanaged[Cdecl]<void> p_instance, delegate* unmanaged[Cdecl]<void>* p_args, delegate* unmanaged[Cdecl]<void> r_ret) => 
        object_method_bind_ptrcall(p_method_bind, p_instance, p_args, r_ret);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_o">A pointer to the Object.</param>
    internal void GDExtensionInterfaceObjectDestroy(delegate* unmanaged[Cdecl]<void> p_o) => 
        object_destroy(p_o);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_name">A pointer to a StringName with the singleton name.</param>
    /// <returns>A pointer to the singleton Object.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceGlobalGetSingleton(delegate* unmanaged[Cdecl]<void> p_name) => 
        global_get_singleton(p_name);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_o">A pointer to the Object.</param>
    /// <param name="p_library">A token the library received by the GDExtension's entry point function.</param>
    /// <param name="p_callbacks">A pointer to a GDExtensionInstanceBindingCallbacks struct.</param>
    internal void* GDExtensionInterfaceObjectGetInstanceBinding(delegate* unmanaged[Cdecl]<void> p_o, void* p_token, GDExtensionInstanceBindingCallbacks* p_callbacks) => 
        object_get_instance_binding(p_o, p_token, p_callbacks);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_o">A pointer to the Object.</param>
    /// <param name="p_library">A token the library received by the GDExtension's entry point function.</param>
    /// <param name="p_binding">A pointer to the instance binding.</param>
    /// <param name="p_callbacks">A pointer to a GDExtensionInstanceBindingCallbacks struct.</param>
    internal void GDExtensionInterfaceObjectSetInstanceBinding(delegate* unmanaged[Cdecl]<void> p_o, void* p_token, void* p_binding, GDExtensionInstanceBindingCallbacks* p_callbacks) => 
        object_set_instance_binding(p_o, p_token, p_binding, p_callbacks);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_o">A pointer to the Object.</param>
    /// <param name="p_classname">A pointer to a StringName with the registered extension class's name.</param>
    /// <param name="p_instance">A pointer to the extension class instance.</param>
    internal void GDExtensionInterfaceObjectSetInstance(delegate* unmanaged[Cdecl]<void> p_o, delegate* unmanaged[Cdecl]<void> p_classname, delegate* unmanaged[Cdecl]<void> p_instance) => 
        object_set_instance(p_o, p_classname, p_instance);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_object">A pointer to the Object.</param>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="r_class_name">A pointer to a String to receive the class name.</param>
    /// <returns>true if successful in getting the class name; otherwise false.</returns>
    internal GDExtensionBool GDExtensionInterfaceObjectGetClassName(delegate* unmanaged[Cdecl]<void> p_object, delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> r_class_name) => 
        object_get_class_name(p_object, p_library, r_class_name);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_object">A pointer to the Object.</param>
    /// <param name="p_class_tag">A pointer uniquely identifying a built-in class in the ClassDB.</param>
    /// <returns>Returns a pointer to the Object, or NULL if it can't be cast to the requested type.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceObjectCastTo(delegate* unmanaged[Cdecl]<void> p_object, void* p_class_tag) => 
        object_cast_to(p_object, p_class_tag);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_instance_id">The instance ID.</param>
    /// <returns>A pointer to the Object.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceObjectGetInstanceFromId(GDObjectInstanceID p_instance_id) => 
        object_get_instance_from_id(p_instance_id);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_object">A pointer to the Object.</param>
    /// <returns>The instance ID.</returns>
    internal GDObjectInstanceID GDExtensionInterfaceObjectGetInstanceId(delegate* unmanaged[Cdecl]<void> p_object) => 
        object_get_instance_id(p_object);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_ref">A pointer to the reference.</param>
    /// <returns>A pointer to the Object from the reference or NULL.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceRefGetObject(delegate* unmanaged[Cdecl]<void> p_ref) => 
        ref_get_object(p_ref);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_ref">A pointer to the reference.</param>
    /// <param name="p_object">A pointer to the Object to refer to.</param>
    internal void GDExtensionInterfaceRefSetObject(delegate* unmanaged[Cdecl]<void> p_ref, delegate* unmanaged[Cdecl]<void> p_object) => 
        ref_set_object(p_ref, p_object);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_info">A pointer to a GDExtensionScriptInstanceInfo struct.</param>
    /// <param name="p_instance_data">A pointer to a data representing the script instance in the GDExtension. This will be passed to all the function pointers on p_info.</param>
    /// <returns>A pointer to a ScriptInstanceExtension object.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceScriptInstanceCreate(GDExtensionScriptInstanceInfo* p_info, delegate* unmanaged[Cdecl]<void> p_instance_data) => 
        script_instance_create(p_info, p_instance_data);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_classname">A pointer to a StringName with the class name.</param>
    /// <returns>A pointer to the newly created Object.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceClassdbConstructObject(delegate* unmanaged[Cdecl]<void> p_classname) => 
        classdb_construct_object(p_classname);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_classname">A pointer to a StringName with the class name.</param>
    /// <param name="p_methodname">A pointer to a StringName with the method name.</param>
    /// <param name="p_hash">A hash representing the function signature.</param>
    /// <returns>A pointer to the MethodBind from ClassDB.</returns>
    internal delegate* unmanaged[Cdecl]<void> GDExtensionInterfaceClassdbGetMethodBind(delegate* unmanaged[Cdecl]<void> p_classname, delegate* unmanaged[Cdecl]<void> p_methodname, GDExtensionInt p_hash) => 
        classdb_get_method_bind(p_classname, p_methodname, p_hash);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_classname">A pointer to a StringName with the class name.</param>
    /// <returns>A pointer uniquely identifying the built-in class in the ClassDB.</returns>
    internal void* GDExtensionInterfaceClassdbGetClassTag(delegate* unmanaged[Cdecl]<void> p_classname) => 
        classdb_get_class_tag(p_classname);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    /// <param name="p_parent_class_name">A pointer to a StringName with the parent class name.</param>
    /// <param name="p_extension_funcs">A pointer to a GDExtensionClassCreationInfo struct.</param>
    internal void GDExtensionInterfaceClassdbRegisterExtensionClass(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name, delegate* unmanaged[Cdecl]<void> p_parent_class_name, GDExtensionClassCreationInfo* p_extension_funcs) => 
        classdb_register_extension_class(p_library, p_class_name, p_parent_class_name, p_extension_funcs);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    /// <param name="p_method_info">A pointer to a GDExtensionClassMethodInfo struct.</param>
    internal void GDExtensionInterfaceClassdbRegisterExtensionClassMethod(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name, GDExtensionClassMethodInfo* p_method_info) => 
        classdb_register_extension_class_method(p_library, p_class_name, p_method_info);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    /// <param name="p_enum_name">A pointer to a StringName with the enum name.</param>
    /// <param name="p_constant_name">A pointer to a StringName with the constant name.</param>
    /// <param name="p_constant_value">The constant value.</param>
    /// <param name="p_is_bitfield">Whether or not this is a bit field.</param>
    internal void GDExtensionInterfaceClassdbRegisterExtensionClassIntegerConstant(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name, delegate* unmanaged[Cdecl]<void> p_enum_name, delegate* unmanaged[Cdecl]<void> p_constant_name, GDExtensionInt p_constant_value, GDExtensionBool p_is_bitfield) => 
        classdb_register_extension_class_integer_constant(p_library, p_class_name, p_enum_name, p_constant_name, p_constant_value, p_is_bitfield);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    /// <param name="p_info">A pointer to a GDExtensionPropertyInfo struct.</param>
    /// <param name="p_setter">A pointer to a StringName with the name of the setter method.</param>
    /// <param name="p_getter">A pointer to a StringName with the name of the getter method.</param>
    internal void GDExtensionInterfaceClassdbRegisterExtensionClassProperty(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name, GDExtensionPropertyInfo* p_info, delegate* unmanaged[Cdecl]<void> p_setter, delegate* unmanaged[Cdecl]<void> p_getter) => 
        classdb_register_extension_class_property(p_library, p_class_name, p_info, p_setter, p_getter);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    /// <param name="p_group_name">A pointer to a String with the group name.</param>
    /// <param name="p_prefix">A pointer to a String with the prefix used by properties in this group.</param>
    internal void GDExtensionInterfaceClassdbRegisterExtensionClassPropertyGroup(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name, delegate* unmanaged[Cdecl]<void> p_group_name, delegate* unmanaged[Cdecl]<void> p_prefix) => 
        classdb_register_extension_class_property_group(p_library, p_class_name, p_group_name, p_prefix);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    /// <param name="p_subgroup_name">A pointer to a String with the subgroup name.</param>
    /// <param name="p_prefix">A pointer to a String with the prefix used by properties in this subgroup.</param>
    internal void GDExtensionInterfaceClassdbRegisterExtensionClassPropertySubgroup(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name, delegate* unmanaged[Cdecl]<void> p_subgroup_name, delegate* unmanaged[Cdecl]<void> p_prefix) => 
        classdb_register_extension_class_property_subgroup(p_library, p_class_name, p_subgroup_name, p_prefix);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    /// <param name="p_signal_name">A pointer to a StringName with the signal name.</param>
    /// <param name="p_argument_info">A pointer to a GDExtensionPropertyInfo struct.</param>
    /// <param name="p_argument_count">The number of arguments the signal receives.</param>
    internal void GDExtensionInterfaceClassdbRegisterExtensionClassSignal(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name, delegate* unmanaged[Cdecl]<void> p_signal_name, GDExtensionPropertyInfo* p_argument_info, GDExtensionInt p_argument_count) => 
        classdb_register_extension_class_signal(p_library, p_class_name, p_signal_name, p_argument_info, p_argument_count);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="p_class_name">A pointer to a StringName with the class name.</param>
    internal void GDExtensionInterfaceClassdbUnregisterExtensionClass(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> p_class_name) => 
        classdb_unregister_extension_class(p_library, p_class_name);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_library">A pointer the library received by the GDExtension's entry point function.</param>
    /// <param name="r_path">A pointer to a String which will receive the path.</param>
    internal void GDExtensionInterfaceGetLibraryPath(delegate* unmanaged[Cdecl]<void> p_library, delegate* unmanaged[Cdecl]<void> r_path) => 
        get_library_path(p_library, r_path);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_class_name">A pointer to a StringName with the name of a class (descending from EditorPlugin) which is already registered with ClassDB.</param>
    internal void GDExtensionInterfaceEditorAddPlugin(delegate* unmanaged[Cdecl]<void> p_class_name) => 
        editor_add_plugin(p_class_name);

    /// <summary>
    /// Since: 4.1
    /// </summary>
    /// <param name="p_class_name">A pointer to a StringName with the name of a class that was previously added as an editor plugin.</param>
    internal void GDExtensionInterfaceEditorRemovePlugin(delegate* unmanaged[Cdecl]<void> p_class_name) => 
        editor_remove_plugin(p_class_name);

#endregion

#region Delegates

    private readonly delegate* unmanaged[Cdecl]<GDExtensionGodotVersion*, void> get_godot_version;

    private readonly delegate* unmanaged[Cdecl]<size_t, void*> mem_alloc;

    private readonly delegate* unmanaged[Cdecl]<void*, size_t, void*> mem_realloc;

    private readonly delegate* unmanaged[Cdecl]<void*, void> mem_free;

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void> print_error;

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void> print_error_with_message;

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void> print_warning;

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void> print_warning_with_message;

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int32_t, GDExtensionBool, void> print_script_error;

    private readonly delegate* unmanaged[Cdecl]<byte*, byte*, byte*, byte*, int32_t, GDExtensionBool, void> print_script_error_with_message;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint64_t> get_native_struct_size;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> variant_new_copy;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> variant_new_nil;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> variant_destroy;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> variant_call;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> variant_call_static;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_evaluate;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_set;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_set_named;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_set_keyed;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void> variant_set_indexed;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_get;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_get_named;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_get_keyed;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool*, void> variant_get_indexed;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool> variant_iter_init;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool> variant_iter_next;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, void> variant_iter_get;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt> variant_hash;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionInt> variant_recursive_hash;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> variant_hash_compare;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionBool> variant_booleanize;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool, void> variant_duplicate;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> variant_stringify;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType> variant_get_type;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> variant_has_method;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> variant_has_member;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool*, GDExtensionBool> variant_has_key;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, void> variant_get_type_name;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool> variant_can_convert;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, GDExtensionBool> variant_can_convert_strict;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> get_variant_from_type_constructor;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> get_variant_to_type_constructor;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, GDExtensionVariantType, GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_operator_evaluator;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, int, void>> variant_get_ptr_builtin_method;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, int32_t, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, void>> variant_get_ptr_constructor;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_destructor;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int32_t, GDExtensionCallError*, void> variant_construct;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_setter;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_getter;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_indexed_setter;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_indexed_getter;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_keyed_setter;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void>> variant_get_ptr_keyed_getter;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, uint32_t>> variant_get_ptr_keyed_checker;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> variant_get_constant_value;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, int, void>> variant_get_ptr_utility_function;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void> string_new_with_latin1_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void> string_new_with_utf8_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, void> string_new_with_utf16_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void> string_new_with_utf32_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void> string_new_with_wide_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void> string_new_with_latin1_chars_and_len;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, void> string_new_with_utf8_chars_and_len;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, void> string_new_with_utf16_chars_and_len;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, void> string_new_with_utf32_chars_and_len;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, void> string_new_with_wide_chars_and_len;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt> string_to_latin1_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, GDExtensionInt, GDExtensionInt> string_to_utf8_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char16_t*, GDExtensionInt, GDExtensionInt> string_to_utf16_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, GDExtensionInt, GDExtensionInt> string_to_utf32_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, GDExtensionInt, GDExtensionInt> string_to_wide_chars;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*> string_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, char32_t*> string_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> string_operator_plus_eq_string;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t, void> string_operator_plus_eq_char;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, byte*, void> string_operator_plus_eq_cstr;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, wchar_t*, void> string_operator_plus_eq_wcstr;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, char32_t*, void> string_operator_plus_eq_c32str;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, size_t, GDExtensionInt> xml_parser_open_buffer;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, void> file_access_store_buffer;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, uint8_t*, uint64_t, uint64_t> file_access_get_buffer;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*> worker_thread_pool_add_native_group_task;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, uint8_t*> packed_byte_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_color_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_color_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*> packed_float32_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, float*> packed_float32_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*> packed_float64_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, double*> packed_float64_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*> packed_int32_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int32_t*> packed_int32_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*> packed_int64_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, int64_t*> packed_int64_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_string_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_string_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_vector2_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_vector2_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_vector3_array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> packed_vector3_array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> array_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> array_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> array_ref;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDExtensionVariantType, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> array_set_typed;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> dictionary_operator_index;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> dictionary_operator_index_const;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, GDExtensionInt, delegate* unmanaged[Cdecl]<void>, GDExtensionCallError*, void> object_method_bind_call;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>*, delegate* unmanaged[Cdecl]<void>, void> object_method_bind_ptrcall;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> object_destroy;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> global_get_singleton;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, GDExtensionInstanceBindingCallbacks*, void*> object_get_instance_binding;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, void*, GDExtensionInstanceBindingCallbacks*, void> object_set_instance_binding;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> object_set_instance;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionBool> object_get_class_name;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*, delegate* unmanaged[Cdecl]<void>> object_cast_to;

    private readonly delegate* unmanaged[Cdecl]<GDObjectInstanceID, delegate* unmanaged[Cdecl]<void>> object_get_instance_from_id;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, GDObjectInstanceID> object_get_instance_id;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> ref_get_object;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> ref_set_object;

    private readonly delegate* unmanaged[Cdecl]<GDExtensionScriptInstanceInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> script_instance_create;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>> classdb_construct_object;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, delegate* unmanaged[Cdecl]<void>> classdb_get_method_bind;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void*> classdb_get_class_tag;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassCreationInfo*, void> classdb_register_extension_class;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionClassMethodInfo*, void> classdb_register_extension_class_method;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionInt, GDExtensionBool, void> classdb_register_extension_class_integer_constant;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> classdb_register_extension_class_property;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> classdb_register_extension_class_property_group;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> classdb_register_extension_class_property_subgroup;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, GDExtensionPropertyInfo*, GDExtensionInt, void> classdb_register_extension_class_signal;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> classdb_unregister_extension_class;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, delegate* unmanaged[Cdecl]<void>, void> get_library_path;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> editor_add_plugin;

    private readonly delegate* unmanaged[Cdecl]<delegate* unmanaged[Cdecl]<void>, void> editor_remove_plugin;

#endregion
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
    /// </summary>
    internal delegate* unmanaged[Cdecl]<void*, GDExtensionInitializationLevel, void*> initialize;

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

