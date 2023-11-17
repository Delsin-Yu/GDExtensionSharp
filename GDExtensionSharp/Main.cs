using System.Runtime.InteropServices;
using GDExtensionSharp.Bindings.Header;
using GDExtensionSharp.Core;

namespace GDExtensionSharp;

public static unsafe class Main
{
    private static MethodTable _methodTable;

    private const uint GODOT_VERSION_MAJOR = 4;
    private const uint GODOT_VERSION_MINOR = 2;
    private const uint GODOT_VERSION_PATCH = 0;

    [UnmanagedCallersOnly(EntryPoint = "gdextension_csharp_init")]
    private static GDExtensionBool Initialize
        (
            delegate* unmanaged <byte*, delegate* unmanaged <void>> p_get_proc_address,
            delegate* unmanaged <void> p_library,
            GDExtensionInitialization* r_initialization
        )
    {
        _methodTable = new(p_get_proc_address);

        GDExtensionGodotVersion version;
        _methodTable.get_godot_version(&version);

        // ReSharper disable ConditionIsAlwaysTrueOrFalse
        bool isCompatible;
        if (version.major != GODOT_VERSION_MAJOR) isCompatible = version.major > GODOT_VERSION_MAJOR;
        else if (version.minor != GODOT_VERSION_MINOR) isCompatible = version.minor > GODOT_VERSION_MINOR;
        else isCompatible = version.patch >= GODOT_VERSION_PATCH;
        // ReSharper restore ConditionIsAlwaysTrueOrFalse

        if (!isCompatible)
        {
            throw new NotSupportedException($"Unable to load the GDExtensionSharp built for Godot {GODOT_VERSION_MAJOR}.{GODOT_VERSION_MINOR}.{GODOT_VERSION_PATCH}, current version is ({version.major}.{version.minor}.{version.patch})");
        }

        r_initialization->initialize = &InitializeLevel; 
        r_initialization->deinitialize = &DeInitializeLevel; 
        
        return 1;
    }
    
    [UnmanagedCallersOnly]
    private static void InitializeLevel(void* userdata, GDExtensionInitializationLevel p_level)
    {
        // ClassDB.CurrentLevel = p_level;
        //
        // OnInit(p_level);
        //
        // ClassDB.Initialize(p_level);
    }

    [UnmanagedCallersOnly]
    private static void DeInitializeLevel(void* userdata, GDExtensionInitializationLevel p_level)
    {
        // ClassDB.CurrentLevel = p_level;
        //
        // OnTerminate(p_level);
        //
        // EditorPlugins::deinitialize(p_level);
        // ClassDB::deinitialize(p_level);
    }

    private static void OnInit(GDExtensionInitializationLevel p_level)
    {
        // Source Generate all classes?
    }

    private static void OnTerminate(GDExtensionInitializationLevel p_level)
    {
        
    }
}
