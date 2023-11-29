using GDExtensionSharp;

namespace Godot.NativeInterop;

[GenerateUnmanagedBinding]
internal static unsafe partial class NativeBinding
{
    private static byte* AsByte(in godot_string value)
    {
        return (byte*)value.GetUnsafeAddress();
    }

    [GDExtensionMethod("classdb_get_method_bind")]
    internal static partial IntPtr classdb_get_method_bind(in godot_string_name p_classname, in godot_string_name p_methodname, long p_hash);

    [GDExtensionMethod("classdb_construct_object")]
    internal static partial delegate* unmanaged<IntPtr> classdb_construct_object(in godot_string_name p_classname);

    [GDExtensionMethod("global_get_singleton")]
    internal static partial IntPtr godotsharp_engine_get_singleton(in godot_string p_name);

    internal static void godotsharp_internal_tie_native_managed_to_unmanaged(
        IntPtr gcHandleIntPtr,
        IntPtr unmanaged,
        in godot_string_name nativeName,
        godot_bool refCounted)
    {
        if (refCounted is godot_bool.True)
        {
        }
    }
}
