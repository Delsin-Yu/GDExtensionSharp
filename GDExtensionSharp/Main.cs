using System.Runtime.InteropServices;
using GDExtensionSharp.Bindings.Header;

namespace GDExtensionSharp;

public static class Main
{
    private static MethodTable _methodTable;
    
    [UnmanagedCallersOnly(EntryPoint = "gdextension_csharp_init")]
    private static unsafe GDExtensionBool Initialize
        (
            delegate* unmanaged <byte*, delegate* unmanaged <void>> p_get_proc_address,
            delegate* unmanaged <void> p_library,
            GDExtensionInitialization* r_initialization
        )
    {
        _methodTable = new(p_get_proc_address);
        return 1;
    }
}
