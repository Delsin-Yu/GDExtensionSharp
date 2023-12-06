using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDExtensionSharp;
public enum ModuleInitializationLevel
{
    MODULE_INITIALIZATION_LEVEL_CORE = GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_CORE,
    MODULE_INITIALIZATION_LEVEL_SERVERS = GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SERVERS,
    MODULE_INITIALIZATION_LEVEL_SCENE = GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SCENE,
    MODULE_INITIALIZATION_LEVEL_EDITOR = GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_EDITOR
}
