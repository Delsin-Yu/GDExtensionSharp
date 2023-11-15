using System.Runtime.InteropServices;

namespace GDExtensionSharp;

public static class Main
{
    private static MethodTable _methodTable;
    
    [UnmanagedCallersOnly(EntryPoint = "gd_extension_csharp_init")]
    private static unsafe GDExtensionBool Initialize
        (
            delegate* unmanaged <char*, delegate* unmanaged <void>> p_get_proc_address,
            delegate* unmanaged <void> p_library,
            GDExtensionInitialization* r_initialization
        )
    {
        _methodTable = new(p_get_proc_address);
        return 0;
    }
}
