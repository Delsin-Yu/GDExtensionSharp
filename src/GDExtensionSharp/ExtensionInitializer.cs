using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unsafe Callback = delegate* unmanaged[Cdecl]<GDExtensionSharp.ModuleInitializationLevel, void>;
namespace GDExtensionSharp;
public unsafe class ExtensionInitializer
{
    private GDExtensionInterfaceGetProcAddress p_get_proc_address;
    private GDExtensionClassLibraryPtr p_library;
    private GDExtensionInitialization* r_initialization;

    public ExtensionInitializer(GDExtensionInterfaceGetProcAddress pGetProcAddress, GDExtensionClassLibraryPtr pLibrary, void* rInitialization)
    {
        p_get_proc_address = pGetProcAddress;
        p_library = pLibrary;
        r_initialization = (GDExtensionInitialization*)rInitialization;
    }

    public byte Initialize()
    {
        return GDExtensionSharpBinding.Initialize(p_get_proc_address, p_library, r_initialization);
    }

    public void RegisterInitializer(Callback callback)
    {
        GDExtensionSharpBinding.InitCallback = callback;
    }

    public void RegisterTerminator(Callback callback)
    {
        GDExtensionSharpBinding.TerminateCallback = callback;
    }
    public void SetMinimumLibraryInitializationLevel(ModuleInitializationLevel level)
    {
        GDExtensionSharpBinding.MinimumInitializationLevel = (GDExtensionInitializationLevel)level;
    }
}
