// ReSharper disable InconsistentNaming
namespace GDExtensionSharp.SourceGenerator.Api.ApiGeneratorImpl;

internal static partial class ApiGenerator
{
    private const string InitializeBindings = "InitializeBindings";
    private const string InitializeBindings_CtorDtor = "InitializeBindings_CtorDtor";
    // private const string GDExtensionInt = "GDExtensionInt";
    // private const string GDExtensionBool = "GDExtensionBool";
    // private const string GDObjectInstanceID = "GDObjectInstanceID";
    // private const string uint32_t = "uint32_t";
    //
    // private const string VoidReturn = "void";
    private const string UntypedPointer = "void*";
    
    private const string MethodBindingModifier = "public";
    // private const string Header = "delegate* unmanaged";
    
    // private const string GDExtensionConstTypePtr = UntypedPointer;
    // private const string GDExtensionTypePtr = UntypedPointer;
    // private const string GDExtensionUninitializedTypePtr = UntypedPointer;
    // private const string GDExtensionConstVariantPtr = UntypedPointer;
    
    private const string GDExtensionPtrConstructor = "GDExtensionPtrConstructor";//$"{Header}<{GDExtensionUninitializedTypePtr}, {GDExtensionConstTypePtr}*, {VoidReturn}>";
    private const string GDExtensionPtrDestructor = "GDExtensionPtrDestructor";//$"{Header}<{GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrBuiltInMethod = "GDExtensionPtrBuiltInMethod";//$"{Header}<{GDExtensionTypePtr}, {GDExtensionConstTypePtr}*, {GDExtensionTypePtr}, int, {VoidReturn}>";
    private const string GDExtensionPtrSetter = "GDExtensionPtrSetter";//$"{Header}<{GDExtensionTypePtr}, {GDExtensionConstTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrGetter = "GDExtensionPtrGetter";//$"{Header}<{GDExtensionConstTypePtr}, {GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrIndexedSetter = "GDExtensionPtrIndexedSetter";//$"{Header}<{GDExtensionTypePtr}, {GDExtensionInt}, {GDExtensionConstTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrIndexedGetter = "GDExtensionPtrIndexedGetter";//$"{Header}<{GDExtensionConstTypePtr}, {GDExtensionInt}, {GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrKeyedSetter = "GDExtensionPtrKeyedSetter";//$"{Header}<{GDExtensionTypePtr}, {GDExtensionConstTypePtr}, {GDExtensionConstTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrKeyedGetter = "GDExtensionPtrKeyedGetter";//$"{Header}<{GDExtensionConstTypePtr}, {GDExtensionConstTypePtr}, {GDExtensionTypePtr}, {VoidReturn}>";
    private const string GDExtensionPtrKeyedChecker = "GDExtensionPtrKeyedChecker";//$"{Header}<{GDExtensionConstVariantPtr}, {GDExtensionConstVariantPtr}, {uint32_t}>";
    private const string GDExtensionPtrOperatorEvaluator = "GDExtensionPtrOperatorEvaluator";//$"{Header}<{GDExtensionConstTypePtr}, {GDExtensionConstTypePtr}, {GDExtensionTypePtr}, {VoidReturn}>";
}
