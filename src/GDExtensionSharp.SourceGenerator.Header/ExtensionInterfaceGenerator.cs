using System.Text;
using GDExtensionSharp.SourceGenerator.Header.Parser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace GDExtensionSharp.SourceGenerator.Header;

[Generator(LanguageNames.CSharp)]
public class ExtensionInterfaceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var source = context.AdditionalTextsProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Where(t =>
                t.Right.GetOptions(t.Left).TryGetValue("build_metadata.AdditionalFiles.SourceItemGroup",
                    out var sourceItemGroup) && sourceItemGroup == "ExtensionInterface" && t.Left.Path.EndsWith(".h"))
            .Select((t, _) => t.Left)
            .Collect();

        var typeForwardResolver = new TypeForwardResolver(
            new Dictionary<string, TypeSyntax>()
            {
                { "char", PredefinedType(Token(SyntaxKind.ByteKeyword)) },
                { "int", PredefinedType(Token(SyntaxKind.IntKeyword)) },
                { "void", PredefinedType(Token(SyntaxKind.VoidKeyword)) },
                { "size_t", PredefinedType(Token(SyntaxKind.ULongKeyword)) },
                { "int32_t", PredefinedType(Token(SyntaxKind.IntKeyword)) },
                { "uint32_t", PredefinedType(Token(SyntaxKind.UIntKeyword)) },
                { "uint16_t", PredefinedType(Token(SyntaxKind.UShortKeyword)) },
                { "int64_t", PredefinedType(Token(SyntaxKind.LongKeyword)) },
                { "uint8_t", PredefinedType(Token(SyntaxKind.ByteKeyword)) },
                { "uint64_t", PredefinedType(Token(SyntaxKind.ULongKeyword)) },
                { "wchar_t", PredefinedType(Token(SyntaxKind.UShortKeyword)) },
                { "float", PredefinedType(Token(SyntaxKind.FloatKeyword)) },
                { "double", PredefinedType(Token(SyntaxKind.DoubleKeyword)) }
            }
        );

        context.RegisterSourceOutput(source, (spc, input) =>
        {
            foreach (var additionalText in input)
            {
                var parser = new CParser();
                var translationUnit = parser.Parse(additionalText.GetText()!.ToString());
                spc.AddSource("ExtensionInterface.Aliases.g.cs",
                    GenerateAliases(translationUnit, typeForwardResolver).GetText(Encoding.UTF8, SourceHashAlgorithm.Sha256));
                spc.AddSource("ExtensionInterface.Types.g.cs",
                    GenerateTypes(translationUnit, typeForwardResolver).GetText(Encoding.UTF8, SourceHashAlgorithm.Sha256));
            }
        });
        context.RegisterPostInitializationOutput(initializationContext =>
        {
            initializationContext.AddSource("ExtensionInterface.MethodTable.g.cs",
                GenerateMethodTable().GetText(Encoding.UTF8, SourceHashAlgorithm.Sha256));
        });
    }

    private static CompilationUnitSyntax GenerateAliases(TranslationUnit translationUnit, TypeForwardResolver typeForwardResolver)
    {
        var aliasTranspiler = new AliasTranspiler(typeForwardResolver);
        var globalUsingDirectives = aliasTranspiler.Transpile(translationUnit);
        var aliasesCompilationUnit = CompilationUnit().AddUsings(globalUsingDirectives.ToArray());
        return aliasesCompilationUnit.NormalizeWhitespace();
    }

    private static CompilationUnitSyntax GenerateTypes(TranslationUnit translationUnit, TypeForwardResolver typeForwardResolver)
    {
        var typeTranspiler = new TypeTranspiler();
        var typeDeclarations = typeTranspiler.Transpile(translationUnit);
        var typesCompilationUnit = CompilationUnit()
                                  .AddMembers(FileScopedNamespaceDeclaration(IdentifierName("GDExtensionSharp")))
                                  .AddMembers(typeDeclarations.ToArray());
        return typesCompilationUnit.NormalizeWhitespace();
    }

    private static CompilationUnitSyntax GenerateMethodTable()
    {
        var table = new Dictionary<string, (string longName, string shortName)>()
        {
            {
                "GDExtensionInterfaceGetGodotVersion", ("gdextension_interface_get_godot_version", "get_godot_version")
            },
            { "GDExtensionInterfaceMemAlloc", ("gdextension_interface_mem_alloc", "mem_alloc") },
            { "GDExtensionInterfaceMemRealloc", ("gdextension_interface_mem_realloc", "mem_realloc") },
            { "GDExtensionInterfaceMemFree", ("gdextension_interface_mem_free", "mem_free") },
            { "GDExtensionInterfacePrintError", ("gdextension_interface_print_error", "print_error") },
            {
                "GDExtensionInterfacePrintErrorWithMessage",
                ("gdextension_interface_print_error_with_message", "print_error_with_message")
            },
            { "GDExtensionInterfacePrintWarning", ("gdextension_interface_print_warning", "print_warning") },
            {
                "GDExtensionInterfacePrintWarningWithMessage",
                ("gdextension_interface_print_warning_with_message", "print_warning_with_message")
            },
            {
                "GDExtensionInterfacePrintScriptError",
                ("gdextension_interface_print_script_error", "print_script_error")
            },
            {
                "GDExtensionInterfacePrintScriptErrorWithMessage",
                ("gdextension_interface_print_script_error_with_message", "print_script_error_with_message")
            },
            {
                "GDExtensionInterfaceGetNativeStructSize",
                ("gdextension_interface_get_native_struct_size", "get_native_struct_size")
            },
            { "GDExtensionInterfaceVariantNewCopy", ("gdextension_interface_variant_new_copy", "variant_new_copy") },
            { "GDExtensionInterfaceVariantNewNil", ("gdextension_interface_variant_new_nil", "variant_new_nil") },
            { "GDExtensionInterfaceVariantDestroy", ("gdextension_interface_variant_destroy", "variant_destroy") },
            { "GDExtensionInterfaceVariantCall", ("gdextension_interface_variant_call", "variant_call") },
            {
                "GDExtensionInterfaceVariantCallStatic",
                ("gdextension_interface_variant_call_static", "variant_call_static")
            },
            { "GDExtensionInterfaceVariantEvaluate", ("gdextension_interface_variant_evaluate", "variant_evaluate") },
            { "GDExtensionInterfaceVariantSet", ("gdextension_interface_variant_set", "variant_set") },
            { "GDExtensionInterfaceVariantSetNamed", ("gdextension_interface_variant_set_named", "variant_set_named") },
            { "GDExtensionInterfaceVariantSetKeyed", ("gdextension_interface_variant_set_keyed", "variant_set_keyed") },
            {
                "GDExtensionInterfaceVariantSetIndexed",
                ("gdextension_interface_variant_set_indexed", "variant_set_indexed")
            },
            { "GDExtensionInterfaceVariantGet", ("gdextension_interface_variant_get", "variant_get") },
            { "GDExtensionInterfaceVariantGetNamed", ("gdextension_interface_variant_get_named", "variant_get_named") },
            { "GDExtensionInterfaceVariantGetKeyed", ("gdextension_interface_variant_get_keyed", "variant_get_keyed") },
            {
                "GDExtensionInterfaceVariantGetIndexed",
                ("gdextension_interface_variant_get_indexed", "variant_get_indexed")
            },
            { "GDExtensionInterfaceVariantIterInit", ("gdextension_interface_variant_iter_init", "variant_iter_init") },
            { "GDExtensionInterfaceVariantIterNext", ("gdextension_interface_variant_iter_next", "variant_iter_next") },
            { "GDExtensionInterfaceVariantIterGet", ("gdextension_interface_variant_iter_get", "variant_iter_get") },
            { "GDExtensionInterfaceVariantHash", ("gdextension_interface_variant_hash", "variant_hash") },
            {
                "GDExtensionInterfaceVariantRecursiveHash",
                ("gdextension_interface_variant_recursive_hash", "variant_recursive_hash")
            },
            {
                "GDExtensionInterfaceVariantHashCompare",
                ("gdextension_interface_variant_hash_compare", "variant_hash_compare")
            },
            {
                "GDExtensionInterfaceVariantBooleanize",
                ("gdextension_interface_variant_booleanize", "variant_booleanize")
            },
            {
                "GDExtensionInterfaceVariantDuplicate", ("gdextension_interface_variant_duplicate", "variant_duplicate")
            },
            {
                "GDExtensionInterfaceVariantStringify", ("gdextension_interface_variant_stringify", "variant_stringify")
            },
            { "GDExtensionInterfaceVariantGetType", ("gdextension_interface_variant_get_type", "variant_get_type") },
            {
                "GDExtensionInterfaceVariantHasMethod",
                ("gdextension_interface_variant_has_method", "variant_has_method")
            },
            {
                "GDExtensionInterfaceVariantHasMember",
                ("gdextension_interface_variant_has_member", "variant_has_member")
            },
            { "GDExtensionInterfaceVariantHasKey", ("gdextension_interface_variant_has_key", "variant_has_key") },
            {
                "GDExtensionInterfaceVariantGetTypeName",
                ("gdextension_interface_variant_get_type_name", "variant_get_type_name")
            },
            {
                "GDExtensionInterfaceVariantCanConvert",
                ("gdextension_interface_variant_can_convert", "variant_can_convert")
            },
            {
                "GDExtensionInterfaceVariantCanConvertStrict",
                ("gdextension_interface_variant_can_convert_strict", "variant_can_convert_strict")
            },
            {
                "GDExtensionInterfaceGetVariantFromTypeConstructor",
                ("gdextension_interface_get_variant_from_type_constructor", "get_variant_from_type_constructor")
            },
            {
                "GDExtensionInterfaceGetVariantToTypeConstructor",
                ("gdextension_interface_get_variant_to_type_constructor", "get_variant_to_type_constructor")
            },
            {
                "GDExtensionInterfaceVariantGetPtrOperatorEvaluator",
                ("gdextension_interface_variant_get_ptr_operator_evaluator", "variant_get_ptr_operator_evaluator")
            },
            {
                "GDExtensionInterfaceVariantGetPtrBuiltinMethod",
                ("gdextension_interface_variant_get_ptr_builtin_method", "variant_get_ptr_builtin_method")
            },
            {
                "GDExtensionInterfaceVariantGetPtrConstructor",
                ("gdextension_interface_variant_get_ptr_constructor", "variant_get_ptr_constructor")
            },
            {
                "GDExtensionInterfaceVariantGetPtrDestructor",
                ("gdextension_interface_variant_get_ptr_destructor", "variant_get_ptr_destructor")
            },
            {
                "GDExtensionInterfaceVariantConstruct", ("gdextension_interface_variant_construct", "variant_construct")
            },
            {
                "GDExtensionInterfaceVariantGetPtrSetter",
                ("gdextension_interface_variant_get_ptr_setter", "variant_get_ptr_setter")
            },
            {
                "GDExtensionInterfaceVariantGetPtrGetter",
                ("gdextension_interface_variant_get_ptr_getter", "variant_get_ptr_getter")
            },
            {
                "GDExtensionInterfaceVariantGetPtrIndexedSetter",
                ("gdextension_interface_variant_get_ptr_indexed_setter", "variant_get_ptr_indexed_setter")
            },
            {
                "GDExtensionInterfaceVariantGetPtrIndexedGetter",
                ("gdextension_interface_variant_get_ptr_indexed_getter", "variant_get_ptr_indexed_getter")
            },
            {
                "GDExtensionInterfaceVariantGetPtrKeyedSetter",
                ("gdextension_interface_variant_get_ptr_keyed_setter", "variant_get_ptr_keyed_setter")
            },
            {
                "GDExtensionInterfaceVariantGetPtrKeyedGetter",
                ("gdextension_interface_variant_get_ptr_keyed_getter", "variant_get_ptr_keyed_getter")
            },
            {
                "GDExtensionInterfaceVariantGetPtrKeyedChecker",
                ("gdextension_interface_variant_get_ptr_keyed_checker", "variant_get_ptr_keyed_checker")
            },
            {
                "GDExtensionInterfaceVariantGetConstantValue",
                ("gdextension_interface_variant_get_constant_value", "variant_get_constant_value")
            },
            {
                "GDExtensionInterfaceVariantGetPtrUtilityFunction",
                ("gdextension_interface_variant_get_ptr_utility_function", "variant_get_ptr_utility_function")
            },
            {
                "GDExtensionInterfaceStringNewWithLatin1Chars",
                ("gdextension_interface_string_new_with_latin1_chars", "string_new_with_latin1_chars")
            },
            {
                "GDExtensionInterfaceStringNewWithUtf8Chars",
                ("gdextension_interface_string_new_with_utf8_chars", "string_new_with_utf8_chars")
            },
            {
                "GDExtensionInterfaceStringNewWithUtf16Chars",
                ("gdextension_interface_string_new_with_utf16_chars", "string_new_with_utf16_chars")
            },
            {
                "GDExtensionInterfaceStringNewWithUtf32Chars",
                ("gdextension_interface_string_new_with_utf32_chars", "string_new_with_utf32_chars")
            },
            {
                "GDExtensionInterfaceStringNewWithWideChars",
                ("gdextension_interface_string_new_with_wide_chars", "string_new_with_wide_chars")
            },
            {
                "GDExtensionInterfaceStringNewWithLatin1CharsAndLen",
                ("gdextension_interface_string_new_with_latin1_chars_and_len", "string_new_with_latin1_chars_and_len")
            },
            {
                "GDExtensionInterfaceStringNewWithUtf8CharsAndLen",
                ("gdextension_interface_string_new_with_utf8_chars_and_len", "string_new_with_utf8_chars_and_len")
            },
            {
                "GDExtensionInterfaceStringNewWithUtf16CharsAndLen",
                ("gdextension_interface_string_new_with_utf16_chars_and_len", "string_new_with_utf16_chars_and_len")
            },
            {
                "GDExtensionInterfaceStringNewWithUtf32CharsAndLen",
                ("gdextension_interface_string_new_with_utf32_chars_and_len", "string_new_with_utf32_chars_and_len")
            },
            {
                "GDExtensionInterfaceStringNewWithWideCharsAndLen",
                ("gdextension_interface_string_new_with_wide_chars_and_len", "string_new_with_wide_chars_and_len")
            },
            {
                "GDExtensionInterfaceStringToLatin1Chars",
                ("gdextension_interface_string_to_latin1_chars", "string_to_latin1_chars")
            },
            {
                "GDExtensionInterfaceStringToUtf8Chars",
                ("gdextension_interface_string_to_utf8_chars", "string_to_utf8_chars")
            },
            {
                "GDExtensionInterfaceStringToUtf16Chars",
                ("gdextension_interface_string_to_utf16_chars", "string_to_utf16_chars")
            },
            {
                "GDExtensionInterfaceStringToUtf32Chars",
                ("gdextension_interface_string_to_utf32_chars", "string_to_utf32_chars")
            },
            {
                "GDExtensionInterfaceStringToWideChars",
                ("gdextension_interface_string_to_wide_chars", "string_to_wide_chars")
            },
            {
                "GDExtensionInterfaceStringOperatorIndex",
                ("gdextension_interface_string_operator_index", "string_operator_index")
            },
            {
                "GDExtensionInterfaceStringOperatorIndexConst",
                ("gdextension_interface_string_operator_index_const", "string_operator_index_const")
            },
            {
                "GDExtensionInterfaceStringOperatorPlusEqString",
                ("gdextension_interface_string_operator_plus_eq_string", "string_operator_plus_eq_string")
            },
            {
                "GDExtensionInterfaceStringOperatorPlusEqChar",
                ("gdextension_interface_string_operator_plus_eq_char", "string_operator_plus_eq_char")
            },
            {
                "GDExtensionInterfaceStringOperatorPlusEqCstr",
                ("gdextension_interface_string_operator_plus_eq_cstr", "string_operator_plus_eq_cstr")
            },
            {
                "GDExtensionInterfaceStringOperatorPlusEqWcstr",
                ("gdextension_interface_string_operator_plus_eq_wcstr", "string_operator_plus_eq_wcstr")
            },
            {
                "GDExtensionInterfaceStringOperatorPlusEqC32str",
                ("gdextension_interface_string_operator_plus_eq_c32str", "string_operator_plus_eq_c32str")
            },
            { "GDExtensionInterfaceStringResize", ("gdextension_interface_string_resize", "string_resize") },
            {
                "GDExtensionInterfaceStringNameNewWithLatin1Chars",
                ("gdextension_interface_string_name_new_with_latin1_chars", "string_name_new_with_latin1_chars")
            },
            {
                "GDExtensionInterfaceXmlParserOpenBuffer",
                ("gdextension_interface_xml_parser_open_buffer", "xml_parser_open_buffer")
            },
            {
                "GDExtensionInterfaceFileAccessStoreBuffer",
                ("gdextension_interface_file_access_store_buffer", "file_access_store_buffer")
            },
            {
                "GDExtensionInterfaceFileAccessGetBuffer",
                ("gdextension_interface_file_access_get_buffer", "file_access_get_buffer")
            },
            {
                "GDExtensionInterfaceWorkerThreadPoolAddNativeGroupTask",
                ("gdextension_interface_worker_thread_pool_add_native_group_task",
                    "worker_thread_pool_add_native_group_task")
            },
            {
                "GDExtensionInterfaceWorkerThreadPoolAddNativeTask",
                ("gdextension_interface_worker_thread_pool_add_native_task", "worker_thread_pool_add_native_task")
            },
            {
                "GDExtensionInterfacePackedByteArrayOperatorIndex",
                ("gdextension_interface_packed_byte_array_operator_index", "packed_byte_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedByteArrayOperatorIndexConst",
                ("gdextension_interface_packed_byte_array_operator_index_const",
                    "packed_byte_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedColorArrayOperatorIndex",
                ("gdextension_interface_packed_color_array_operator_index", "packed_color_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedColorArrayOperatorIndexConst",
                ("gdextension_interface_packed_color_array_operator_index_const",
                    "packed_color_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedFloat32ArrayOperatorIndex",
                ("gdextension_interface_packed_float32_array_operator_index", "packed_float32_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedFloat32ArrayOperatorIndexConst",
                ("gdextension_interface_packed_float32_array_operator_index_const",
                    "packed_float32_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedFloat64ArrayOperatorIndex",
                ("gdextension_interface_packed_float64_array_operator_index", "packed_float64_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedFloat64ArrayOperatorIndexConst",
                ("gdextension_interface_packed_float64_array_operator_index_const",
                    "packed_float64_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedInt32ArrayOperatorIndex",
                ("gdextension_interface_packed_int32_array_operator_index", "packed_int32_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedInt32ArrayOperatorIndexConst",
                ("gdextension_interface_packed_int32_array_operator_index_const",
                    "packed_int32_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedInt64ArrayOperatorIndex",
                ("gdextension_interface_packed_int64_array_operator_index", "packed_int64_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedInt64ArrayOperatorIndexConst",
                ("gdextension_interface_packed_int64_array_operator_index_const",
                    "packed_int64_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedStringArrayOperatorIndex",
                ("gdextension_interface_packed_string_array_operator_index", "packed_string_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedStringArrayOperatorIndexConst",
                ("gdextension_interface_packed_string_array_operator_index_const",
                    "packed_string_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedVector2ArrayOperatorIndex",
                ("gdextension_interface_packed_vector2_array_operator_index", "packed_vector2_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedVector2ArrayOperatorIndexConst",
                ("gdextension_interface_packed_vector2_array_operator_index_const",
                    "packed_vector2_array_operator_index_const")
            },
            {
                "GDExtensionInterfacePackedVector3ArrayOperatorIndex",
                ("gdextension_interface_packed_vector3_array_operator_index", "packed_vector3_array_operator_index")
            },
            {
                "GDExtensionInterfacePackedVector3ArrayOperatorIndexConst",
                ("gdextension_interface_packed_vector3_array_operator_index_const",
                    "packed_vector3_array_operator_index_const")
            },
            {
                "GDExtensionInterfaceArrayOperatorIndex",
                ("gdextension_interface_array_operator_index", "array_operator_index")
            },
            {
                "GDExtensionInterfaceArrayOperatorIndexConst",
                ("gdextension_interface_array_operator_index_const", "array_operator_index_const")
            },
            { "GDExtensionInterfaceArrayRef", ("gdextension_interface_array_ref", "array_ref") },
            { "GDExtensionInterfaceArraySetTyped", ("gdextension_interface_array_set_typed", "array_set_typed") },
            {
                "GDExtensionInterfaceDictionaryOperatorIndex",
                ("gdextension_interface_dictionary_operator_index", "dictionary_operator_index")
            },
            {
                "GDExtensionInterfaceDictionaryOperatorIndexConst",
                ("gdextension_interface_dictionary_operator_index_const", "dictionary_operator_index_const")
            },
            {
                "GDExtensionInterfaceObjectMethodBindCall",
                ("gdextension_interface_object_method_bind_call", "object_method_bind_call")
            },
            {
                "GDExtensionInterfaceObjectMethodBindPtrcall",
                ("gdextension_interface_object_method_bind_ptrcall", "object_method_bind_ptrcall")
            },
            { "GDExtensionInterfaceObjectDestroy", ("gdextension_interface_object_destroy", "object_destroy") },
            {
                "GDExtensionInterfaceGlobalGetSingleton",
                ("gdextension_interface_global_get_singleton", "global_get_singleton")
            },
            {
                "GDExtensionInterfaceObjectGetInstanceBinding",
                ("gdextension_interface_object_get_instance_binding", "object_get_instance_binding")
            },
            {
                "GDExtensionInterfaceObjectSetInstanceBinding",
                ("gdextension_interface_object_set_instance_binding", "object_set_instance_binding")
            },
            {
                "GDExtensionInterfaceObjectSetInstance",
                ("gdextension_interface_object_set_instance", "object_set_instance")
            },
            {
                "GDExtensionInterfaceObjectGetClassName",
                ("gdextension_interface_object_get_class_name", "object_get_class_name")
            },
            { "GDExtensionInterfaceObjectCastTo", ("gdextension_interface_object_cast_to", "object_cast_to") },
            {
                "GDExtensionInterfaceObjectGetInstanceFromId",
                ("gdextension_interface_object_get_instance_from_id", "object_get_instance_from_id")
            },
            {
                "GDExtensionInterfaceObjectGetInstanceId",
                ("gdextension_interface_object_get_instance_id", "object_get_instance_id")
            },
            {
                "GDExtensionInterfaceCallableCustomCreate",
                ("gdextension_interface_callable_custom_create", "callable_custom_create")
            },
            {
                "GDExtensionInterfaceCallableCustomGetUserData",
                ("gdextension_interface_callable_custom_get_userdata", "callable_custom_get_userdata")
            },
            { "GDExtensionInterfaceRefGetObject", ("gdextension_interface_ref_get_object", "ref_get_object") },
            { "GDExtensionInterfaceRefSetObject", ("gdextension_interface_ref_set_object", "ref_set_object") },
            {
                "GDExtensionInterfaceScriptInstanceCreate2",
                ("gdextension_interface_script_instance_create2", "script_instance_create2")
            },
            {
                "GDExtensionInterfacePlaceHolderScriptInstanceCreate",
                ("gdextension_interface_placeholder_script_instance_create", "placeholder_script_instance_create")
            },
            {
                "GDExtensionInterfacePlaceHolderScriptInstanceUpdate",
                ("gdextension_interface_placeholder_script_instance_update", "placeholder_script_instance_update")
            },
            {
                "GDExtensionInterfaceClassdbConstructObject",
                ("gdextension_interface_classdb_construct_object", "classdb_construct_object")
            },
            {
                "GDExtensionInterfaceClassdbGetMethodBind",
                ("gdextension_interface_classdb_get_method_bind", "classdb_get_method_bind")
            },
            {
                "GDExtensionInterfaceClassdbGetClassTag",
                ("gdextension_interface_classdb_get_class_tag", "classdb_get_class_tag")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClass2",
                ("gdextension_interface_classdb_register_extension_class2", "classdb_register_extension_class2")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClassMethod",
                ("gdextension_interface_classdb_register_extension_class_method",
                    "classdb_register_extension_class_method")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClassIntegerConstant",
                ("gdextension_interface_classdb_register_extension_class_integer_constant",
                    "classdb_register_extension_class_integer_constant")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClassProperty",
                ("gdextension_interface_classdb_register_extension_class_property",
                    "classdb_register_extension_class_property")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClassPropertyIndexed",
                ("gdextension_interface_classdb_register_extension_class_property_indexed",
                    "classdb_register_extension_class_property_indexed")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClassPropertyGroup",
                ("gdextension_interface_classdb_register_extension_class_property_group",
                    "classdb_register_extension_class_property_group")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClassPropertySubgroup",
                ("gdextension_interface_classdb_register_extension_class_property_subgroup",
                    "classdb_register_extension_class_property_subgroup")
            },
            {
                "GDExtensionInterfaceClassdbRegisterExtensionClassSignal",
                ("gdextension_interface_classdb_register_extension_class_signal",
                    "classdb_register_extension_class_signal")
            },
            {
                "GDExtensionInterfaceClassdbUnregisterExtensionClass",
                ("gdextension_interface_classdb_unregister_extension_class", "classdb_unregister_extension_class")
            },
            { "GDExtensionInterfaceGetLibraryPath", ("gdextension_interface_get_library_path", "get_library_path") },
            { "GDExtensionInterfaceEditorAddPlugin", ("gdextension_interface_editor_add_plugin", "editor_add_plugin") },
            {
                "GDExtensionInterfaceEditorRemovePlugin",
                ("gdextension_interface_editor_remove_plugin", "editor_remove_plugin")
            }
        };
        var compilationUnit = CompilationUnit();
        var @namespace = FileScopedNamespaceDeclaration(IdentifierName("GDExtensionSharp"));
        var getProcAddressMethod =
            MethodDeclaration(
                    FunctionPointerType()
                        .WithCallingConvention(FunctionPointerCallingConvention(Token(SyntaxKind.UnmanagedKeyword))
                            .AddUnmanagedCallingConventionListCallingConventions(
                                FunctionPointerUnmanagedCallingConvention(Identifier("Cdecl"))))
                        .AddParameterListParameters(
                            FunctionPointerParameter(PredefinedType(Token(SyntaxKind.VoidKeyword)))),
                    Identifier("GetProcAddress"))
                .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword))
                .AddParameterListParameters(
                    Parameter(Identifier("p_get_proc_address"))
                        .WithType(IdentifierName("GDExtensionInterfaceGetProcAddress")),
                    Parameter(Identifier("methodName")).WithType(PredefinedType(Token(SyntaxKind.StringKeyword))))
                .AddBodyStatements(
                    LocalDeclarationStatement(
                        VariableDeclaration(ArrayType(PredefinedType(Token(SyntaxKind.ByteKeyword)))
                                .AddRankSpecifiers(
                                    ArrayRankSpecifier().AddSizes(OmittedArraySizeExpression())))
                            .AddVariables(VariableDeclarator(Identifier("bytes")).WithInitializer(EqualsValueClause(
                                InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("global::System.Text.Encoding.ASCII"),
                                        IdentifierName("GetBytes")))
                                    .AddArgumentListArguments(Argument(IdentifierName("methodName"))))))),
                    LocalDeclarationStatement(VariableDeclaration(FunctionPointerType()
                            .WithCallingConvention(FunctionPointerCallingConvention(Token(SyntaxKind.UnmanagedKeyword))
                                .AddUnmanagedCallingConventionListCallingConventions(
                                    FunctionPointerUnmanagedCallingConvention(Identifier("Cdecl"))))
                            .AddParameterListParameters(
                                FunctionPointerParameter(PredefinedType(Token(SyntaxKind.VoidKeyword)))))
                        .AddVariables(VariableDeclarator(Identifier("ret")))),
                    FixedStatement(
                        VariableDeclaration(PointerType(PredefinedType(Token(SyntaxKind.ByteKeyword))))
                            .AddVariables(VariableDeclarator(Identifier("p"))
                                .WithInitializer(EqualsValueClause(IdentifierName("bytes")))),
                        Block().AddStatements(ExpressionStatement(AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression, IdentifierName("ret"),
                            InvocationExpression(IdentifierName("p_get_proc_address"))
                                .AddArgumentListArguments(Argument(IdentifierName("p"))))))),
                    ReturnStatement(IdentifierName("ret")));
        var methodTable = ClassDeclaration("MethodTable")
            .AddModifiers(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.UnsafeKeyword))
            .AddMembers(getProcAddressMethod);
        var constructor = ConstructorDeclaration(Identifier("MethodTable"))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(Parameter(Identifier("p_get_proc_address"))
                .WithType(IdentifierName("GDExtensionInterfaceGetProcAddress")));
        foreach (KeyValuePair<string, (string longName, string shortName)> kvp in table)
        {
            constructor = constructor.AddBodyStatements(ExpressionStatement(AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression, IdentifierName(kvp.Value.longName),
                CastExpression(IdentifierName(kvp.Key), InvocationExpression(IdentifierName("GetProcAddress"))
                    .AddArgumentListArguments(
                        Argument(IdentifierName("p_get_proc_address")),
                        Argument(LiteralExpression(SyntaxKind.StringLiteralExpression,
                            Literal(kvp.Value.shortName))))))));
        }
        methodTable = methodTable.AddMembers(constructor);
        foreach (KeyValuePair<string, (string longName, string shortName)> kvp in table)
        {
            var fieldDeclaration = FieldDeclaration(VariableDeclaration(IdentifierName(kvp.Key)))
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddDeclarationVariables(VariableDeclarator(Identifier(kvp.Value.longName)));
            methodTable = methodTable.AddMembers(fieldDeclaration);
        }
        compilationUnit = compilationUnit.AddMembers(@namespace, methodTable);
        return compilationUnit.NormalizeWhitespace();
    }
}
