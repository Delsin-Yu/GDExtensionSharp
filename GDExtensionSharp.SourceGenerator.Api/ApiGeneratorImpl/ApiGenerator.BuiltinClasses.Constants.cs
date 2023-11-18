// ReSharper disable InconsistentNaming
namespace GDExtensionSharp.SourceGenerator.Api;

internal static partial class ApiGenerator
{
    private const string InitializeBindings = "InitializeBindings";
    private const string InitializeBindings_CtorDtor = "InitializeBindings_CtorDtor";
    private const string GDExtensionInt = "GDExtensionInt";
    private const string GDExtensionBool = "GDExtensionBool";
    private const string GDObjectInstanceID = "GDObjectInstanceID";
    private const string uint32_t = "uint32_t";
    
    private const string VoidReturn = "void";
    private const string UntypedPointer = "void*";
    
    private const string MethodBindingModifier = "public";
    private const string Header = "delegate* unmanaged";
    
    private const string GDExtensionConstTypePtr = UntypedPointer;
    private const string GDExtensionTypePtr = UntypedPointer;
    private const string GDExtensionUninitializedTypePtr = UntypedPointer;
    private const string GDExtensionConstVariantPtr = UntypedPointer;
    
    private const string GDExtensionPtrConstructor = $"{Header}<{GDExtensionUninitializedTypePtr}, {GDExtensionConstTypePtr}*, {VoidReturn}>";
    private const string GDExtensionPtrDestructor = $"{Header}<{GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrBuiltInMethod = $"{Header}<{GDExtensionTypePtr}, {GDExtensionConstTypePtr}*, {GDExtensionTypePtr}, int, {VoidReturn}>";
    private const string GDExtensionPtrSetter = $"{Header}<{GDExtensionTypePtr}, {GDExtensionConstTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrGetter = $"{Header}<{GDExtensionConstTypePtr}, {GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrIndexedSetter = $"{Header}<{GDExtensionTypePtr}, {GDExtensionInt}, {GDExtensionConstTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrIndexedGetter = $"{Header}<{GDExtensionConstTypePtr}, {GDExtensionInt}, {GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrKeyedSetter = $"{Header}<{GDExtensionTypePtr}, {GDExtensionConstTypePtr}, {GDExtensionConstTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrKeyedGetter = $"{Header}<{GDExtensionConstTypePtr}, {GDExtensionConstTypePtr}, {GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrKeyedChecker = $"{Header}<{GDExtensionConstVariantPtr}, {GDExtensionConstVariantPtr}, {uint32_t}>";
    private const string GDExtensionPtrOperatorEvaluator = $"{Header}<{GDExtensionConstTypePtr}, {GDExtensionConstTypePtr}, {GDExtensionTypePtr}, {VoidReturn}>";
}
