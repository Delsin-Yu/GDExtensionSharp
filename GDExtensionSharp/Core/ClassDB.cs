using GDExtensionSharp.Bindings.Header;

namespace GDExtensionSharp.Core;

internal static class ClassDB
{
    public unsafe class ClassInfo 
    {
        // private readonly string _name;
        // private readonly string _parent_name;
        // private readonly GDExtensionInitializationLevel _level = GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SCENE;
        // private readonly Dictionary<string, MethodBind *> _method_map;
        // private readonly HashSet<string> _signal_names;
        // private readonly Dictionary<string, > _virtual_methods;
        // private readonly HashSet<string> _property_names;
        // private readonly HashSet<string> _constant_names;
        // // Pointer to the parent custom class, if any. Will be null if the parent class is a Godot class.
        // private readonly ClassInfo _parent_ptr;
    };
    
    
    internal static GDExtensionInitializationLevel CurrentLevel { get; set; }

    private static Dictionary<string, ClassInfo> _classes = new();
    
    internal static void Initialize(GDExtensionInitializationLevel p_level)
    {
        
    }
}
