using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using unsafe Callback = delegate* unmanaged[Cdecl]<GDExtensionSharp.ModuleInitializationLevel, void>;
namespace GDExtensionSharp;

internal static unsafe class GDExtensionSharpBinding
{
    public static MethodTable MethodTable;
    public static Callback InitCallback;
    public static Callback TerminateCallback;
    public static GDExtensionInitializationLevel MinimumInitializationLevel;

    private const uint GODOT_VERSION_MAJOR = 4;
    private const uint GODOT_VERSION_MINOR = 2;
    private const uint GODOT_VERSION_PATCH = 0;

    public static GDExtensionBool Initialize(GDExtensionInterfaceGetProcAddress p_get_proc_address, GDExtensionClassLibraryPtr p_library, GDExtensionInitialization* r_initialization)
    {
        MethodTable = new(p_get_proc_address);

        GDExtensionGodotVersion version;
        MethodTable.get_godot_version(&version);

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
        r_initialization->minimum_initialization_level = MinimumInitializationLevel;

        return 1;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void InitializeLevel(void* userdata, GDExtensionInitializationLevel p_level)
    {
        
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static void DeInitializeLevel(void* userdata, GDExtensionInitializationLevel p_level)
    {
    }

    private static void OnInit(GDExtensionInitializationLevel p_level)
    {
        // Source Generate all classes?
    }

    private static void OnTerminate(GDExtensionInitializationLevel p_level)
    {
    }
}
