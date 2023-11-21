using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string GDE_LOCAL_PARAMETER_NAME = "gde_name";

    private static string GenerateBuiltinClassInitializeBindingMethod(StringBuilder stringBuilder, BuiltinClass builtinClass)
    {
        var enumTypeName = $"GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE{builtinClass.Name.PascalCaseToSnakeCase().ToUpper()}";
        GenerateBuiltinClassCtorDtorInitialization(stringBuilder, builtinClass);
        GenerateLocalStringNameInitialization(stringBuilder);
        GenerateBuiltinClassMethodsInitialization(stringBuilder, enumTypeName, builtinClass.Methods);
        GenerateBuiltinClassMembersInitialization(stringBuilder, enumTypeName, builtinClass.Members);
        GenerateBuiltinClassIndexerInitialization(stringBuilder, enumTypeName, builtinClass.IndexingReturnType != null);
        GenerateBuiltinClassKeyedInitialization(stringBuilder, enumTypeName, builtinClass.IsKeyed);
        GenerateBuiltinClassOperatorsInitialization(stringBuilder, enumTypeName, builtinClass.Operators);

        return stringBuilder.ToStringAndClear();
    }

    private static void GenerateBuiltinClassCtorDtorInitialization(StringBuilder stringBuilder, BuiltinClass builtinClass)
    {
        if (builtinClass.Name == "StringName")
        {
            stringBuilder.AppendLine($"String.{InitializeBindings_CtorDtor}();");
        }

        stringBuilder
           .AppendLine($"{InitializeBindings_CtorDtor}();")
           .AppendLine();
    }

    private static void GenerateLocalStringNameInitialization(StringBuilder stringBuilder) =>
        stringBuilder
           .AppendLine($"StringName {GDE_LOCAL_PARAMETER_NAME};")
           .AppendLine();

    private static void GenerateBuiltinClassMethodsInitialization(StringBuilder stringBuilder, string enumTypeName, IReadOnlyList<BuiltinClassMethod> builtinClassMethods)
    {
        if (builtinClassMethods == null) return;

        foreach (var builtinClassMethod in builtinClassMethods)
        {
            stringBuilder
               .AppendLine($"{GDE_LOCAL_PARAMETER_NAME} = new StringName(\"{builtinClassMethod.Name}\");")
               .AppendLine($"{BindingStructFieldName}.method_{builtinClassMethod.Name} = {MethodTableAccess}.variant_get_ptr_builtin_method({enumTypeName}, {GDE_LOCAL_PARAMETER_NAME}._native_ptr(), {builtinClassMethod.Hash.ToString(CultureInfo.InvariantCulture)});");
        }

        stringBuilder.AppendLine();
    }

    private static void GenerateBuiltinClassMembersInitialization(StringBuilder stringBuilder, string enumTypeName, IReadOnlyList<Singleton> builtinClassMembers)
    {
        if (builtinClassMembers == null) return;

        foreach (var builtinClassMember in builtinClassMembers)
        {
            stringBuilder
               .AppendLine($"{GDE_LOCAL_PARAMETER_NAME} = new StringName(\"{builtinClassMember.Name}\");")
               .AppendLine($"{BindingStructFieldName}.member_{builtinClassMember.Name}_setter = {MethodTableAccess}.variant_get_ptr_setter({enumTypeName}, {GDE_LOCAL_PARAMETER_NAME}._native_ptr());")
               .AppendLine($"{BindingStructFieldName}.member_{builtinClassMember.Name}_getter = {MethodTableAccess}.variant_get_ptr_getter({enumTypeName}, {GDE_LOCAL_PARAMETER_NAME}._native_ptr());");
        }

        stringBuilder.AppendLine();
    }

    private static void GenerateBuiltinClassIndexerInitialization(StringBuilder stringBuilder, string enumTypeName, bool hasIndexer)
    {
        if (!hasIndexer) return;
        stringBuilder
           .AppendLine($"{BindingStructFieldName}.indexed_setter = {MethodTableAccess}.variant_get_ptr_indexed_setter({enumTypeName});")
           .AppendLine($"{BindingStructFieldName}.indexed_getter = {MethodTableAccess}.variant_get_ptr_indexed_getter({enumTypeName});")
           .AppendLine();
    }

    private static void GenerateBuiltinClassKeyedInitialization(StringBuilder stringBuilder, string enumTypeName, bool isKeyed)
    {
        if (!isKeyed) return;
        stringBuilder
           .AppendLine($"{BindingStructFieldName}.keyed_setter = {MethodTableAccess}.variant_get_ptr_keyed_setter({enumTypeName});")
           .AppendLine($"{BindingStructFieldName}.keyed_getter = {MethodTableAccess}.variant_get_ptr_keyed_getter({enumTypeName});")
           .AppendLine($"{BindingStructFieldName}.keyed_checker = {MethodTableAccess}.variant_get_ptr_keyed_checker({enumTypeName});")
           .AppendLine();
    }

    private static void GenerateBuiltinClassOperatorsInitialization(StringBuilder stringBuilder, string enumTypeName, IReadOnlyList<Operator> builtinClassOperators)
    {
        if (builtinClassOperators == null) return;

        foreach (var @operator in builtinClassOperators)
        {
            if (@operator.RightType != null)
            {
                string right_type_variant_type;
                if (@operator.RightType == "Variant")
                {
                    right_type_variant_type = "GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_NIL";
                }
                else
                {
                    right_type_variant_type = $"GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE{@operator.RightType.PascalCaseToSnakeCase().ToUpperInvariant()}";
                }

                stringBuilder.AppendLine($"{BindingStructFieldName}.operator_{@operator.Name.OperatorIdToName()}_{@operator.RightType} = {MethodTableAccess}.variant_get_ptr_operator_evaluator(GDExtensionVariantOperator.GDEXTENSION_VARIANT_OP_{@operator.Name.OperatorIdToName().ToUpper()}, {enumTypeName}, {right_type_variant_type});");
            }
            else
            {
                stringBuilder.AppendLine($"{BindingStructFieldName}.operator_{@operator.Name.OperatorIdToName()} = {MethodTableAccess}.variant_get_ptr_operator_evaluator(GDExtensionVariantOperator.GDEXTENSION_VARIANT_OP_{@operator.Name.OperatorIdToName().ToUpperInvariant()}, {enumTypeName}, GDExtensionVariantType.GDEXTENSION_VARIANT_TYPE_NIL);");
            }
        }

        stringBuilder.AppendLine();
    }
}
