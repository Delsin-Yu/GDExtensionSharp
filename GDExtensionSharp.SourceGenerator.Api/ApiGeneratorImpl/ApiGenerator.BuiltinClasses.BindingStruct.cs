using System.Globalization;
using System.Text;

namespace GDExtensionSharp.SourceGenerator.Api;

internal static partial class ApiGenerator
{
    private static string GenerateBuiltinClassBindingStruct(StringBuilder stringBuilder, BuiltinClass builtinClass)
    {
        GenerateBuiltinClassConstructorBindings(stringBuilder, builtinClass.Constructors);
        GenerateBuiltinClassDestructorBindings(stringBuilder, builtinClass.HasDestructor);
        GenerateBuiltinClassMethodBindings(stringBuilder, builtinClass.Methods);
        GenerateBuiltinClassMemberBindings(stringBuilder, builtinClass.Members);
        GenerateBuiltinClassIndexerBindings(stringBuilder, builtinClass.IndexingReturnType != null);
        GenerateBuiltinClassKeyedBindings(stringBuilder, builtinClass.IsKeyed);
        GenerateBuiltinClassOperatorBindings(stringBuilder, builtinClass.Operators);
        return stringBuilder.ToStringAndClear();
    }

    private static void GenerateBuiltinClassConstructorBindings(StringBuilder stringBuilder, IReadOnlyList<Constructor> builtinClassConstructors)
    {
        if (builtinClassConstructors == null) return;
        foreach (var builtinClassConstructor in builtinClassConstructors)
        {
            stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrConstructor} constructor_{builtinClassConstructor.Index.ToString(CultureInfo.InvariantCulture)};");
        }

        stringBuilder.AppendLine();
    }

    private static void GenerateBuiltinClassDestructorBindings(StringBuilder stringBuilder, bool builtinClassHasDestructor)
    {
        if (!builtinClassHasDestructor) return;
        stringBuilder
           .AppendLine($"{MethodBindingModifier} {GDExtensionPtrDestructor} destructor;")
           .AppendLine();
    }

    private static void GenerateBuiltinClassMethodBindings(StringBuilder stringBuilder, IReadOnlyList<BuiltinClassMethod> builtinClassMethods)
    {
        if (builtinClassMethods == null) return;
        foreach (var builtinClassMethod in builtinClassMethods)
        {
            stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrBuiltInMethod} method_{builtinClassMethod.Name};");
        }

        stringBuilder.AppendLine();
    }

    private static void GenerateBuiltinClassMemberBindings(StringBuilder stringBuilder, IReadOnlyList<Singleton> builtinClassMembers)
    {
        if (builtinClassMembers == null) return;
        foreach (var builtinClassMember in builtinClassMembers)
        {
            stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrSetter} member_{builtinClassMember.Name}_setter;");
            stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrGetter} member_{builtinClassMember.Name}_getter;");
        }

        stringBuilder.AppendLine();
    }

    private static void GenerateBuiltinClassIndexerBindings(StringBuilder stringBuilder, bool hasIndexer)
    {
        if (!hasIndexer) return;
        stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrIndexedSetter} indexed_setter;");
        stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrIndexedGetter} indexed_getter;");
        stringBuilder.AppendLine();
    }

    private static void GenerateBuiltinClassKeyedBindings(StringBuilder stringBuilder, bool isKeyed)
    {
        if (!isKeyed) return;
        stringBuilder.AppendLine($"{GDExtensionPtrKeyedSetter} keyed_setter;");
        stringBuilder.AppendLine($"{GDExtensionPtrKeyedGetter} keyed_getter;");
        stringBuilder.AppendLine($"{GDExtensionPtrKeyedChecker} keyed_checker;");
        stringBuilder.AppendLine();
    }

    private static void GenerateBuiltinClassOperatorBindings(StringBuilder stringBuilder, IReadOnlyList<Operator> builtinClassOperators)
    {
        if (builtinClassOperators == null) return;
        foreach (var @operator in builtinClassOperators)
        {
            if (@operator.RightType != null)
            {
                stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrOperatorEvaluator} operator_{@operator.Name.OperatorIdToName()}_{@operator.RightType};");
            }
            else
            {
                stringBuilder.AppendLine($"{MethodBindingModifier} {GDExtensionPtrOperatorEvaluator} operator_{@operator.Name.OperatorIdToName()};");
            }
        }

        stringBuilder.AppendLine();
    }
}
