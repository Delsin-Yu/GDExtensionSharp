namespace Godot.NativeInterop;

unsafe partial class NativeFuncs
{
    private static UnmanagedCallbacks _unmanagedCallbacks;

    /// <summary>
    /// Never called under GDExtension usecase
    /// </summary>
    /// <returns><see cref="godot_bool.True"/></returns>
    internal static partial godot_bool godotsharp_dotnet_module_is_initialized()
    {
        return godot_bool.True;
    }

    /// <summary>
    /// <see cref="GodotObject.ClassDB_get_method"/> uses this binding, which the method itself is never used anymore.
    /// </summary>
    /// <exception cref="NotSupportedException">Always</exception>
    public static partial IntPtr godotsharp_method_bind_get_method(in godot_string_name p_classname, in godot_string_name p_methodname)
    {
        throw new NotSupportedException();
    }

    public static partial IntPtr godotsharp_method_bind_get_method_with_compatibility(in godot_string_name p_classname, in godot_string_name p_methodname, ulong p_hash)
    {
        return NativeBinding.classdb_get_method_bind(p_classname, p_methodname, (long)p_hash);
    }

    public static partial delegate* unmanaged<IntPtr> godotsharp_get_class_constructor(in godot_string_name p_classname)
    {
        return NativeBinding.classdb_construct_object(p_classname);
    }

    public static partial IntPtr godotsharp_engine_get_singleton(in godot_string p_name)
    {
        return NativeBinding.godotsharp_engine_get_singleton(p_name);
    }

    /// <summary>
    /// <see cref="DebuggingUtils.godot_stack_info_vector"/> is only used by <see cref="DebuggingUtils.GetCurrentStackInfo"/> or <see cref="ExceptionUtils.SendToScriptDebugger"/>; any of which is unreachable so we simply ignore any implementation related to it
    /// </summary>
    /// <exception cref="NotSupportedException">Always</exception>
    internal static partial Error godotsharp_stack_info_vector_resize(
        ref DebuggingUtils.godot_stack_info_vector p_stack_info_vector,
        int p_size)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// <see cref="DebuggingUtils.godot_stack_info_vector"/> is only used by <see cref="DebuggingUtils.GetCurrentStackInfo"/> or <see cref="ExceptionUtils.SendToScriptDebugger"/>; any of which is unreachable so we simply ignore any implementation related to it
    /// </summary>
    /// <exception cref="NotSupportedException">Always</exception>
    internal static partial void godotsharp_stack_info_vector_destroy(
        ref DebuggingUtils.godot_stack_info_vector p_stack_info_vector)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// GDExtension does not use in-editor file system, so nothing is calling this method
    /// </summary>
    /// <exception cref="NotSupportedException">Always</exception>
    internal static partial void godotsharp_internal_editor_file_system_update_file(in godot_string p_script_path)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// GDExtension does not use in-editor file system, so nothing is calling this method
    /// </summary>
    /// <exception cref="NotSupportedException">Always</exception>
    internal static partial void godotsharp_internal_script_debugger_send_error(
        in godot_string p_func,
        in godot_string p_file,
        int p_line,
        in godot_string p_err,
        in godot_string p_descr,
        godot_error_handler_type p_type,
        in DebuggingUtils.godot_stack_info_vector p_stack_info_vector)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// GDExtension do not support logging to script debugger
    /// </summary>
    /// <returns><see cref="godot_bool.False"/></returns>
    internal static partial godot_bool godotsharp_internal_script_debugger_is_active()
    {
        return godot_bool.False;
    }

    /// <summary>
    /// This method ties the managed representation of a Native Godot Type to its unmanaged counterpart
    /// </summary>
    /// <param name="gcHandleIntPtr">The allocated gc handle</param>
    /// <param name="unmanaged">This should be the opaque data in gdextension</param>
    /// <param name="nativeName">The string name for the type</param>
    /// <param name="refCounted">Is the type ref counted</param>
    internal static partial void godotsharp_internal_tie_native_managed_to_unmanaged(
        IntPtr gcHandleIntPtr,
        IntPtr unmanaged,
        in godot_string_name nativeName,
        godot_bool refCounted)
    {
        NativeBinding.godotsharp_internal_tie_native_managed_to_unmanaged(gcHandleIntPtr, unmanaged, in nativeName, refCounted);
    }
}
