using System;
using System.Runtime.InteropServices;
using GDExtensionSharp.NativeInterop;

namespace GDExtensionSharp
{
    public static class Dispatcher
    {
        internal static GodotTaskScheduler DefaultGodotTaskScheduler;

        internal static void InitializeDefaultGodotTaskScheduler()
        {
            DefaultGodotTaskScheduler?.Dispose();
            DefaultGodotTaskScheduler = new GodotTaskScheduler();
        }

        public static GodotSynchronizationContext SynchronizationContext => DefaultGodotTaskScheduler.Context;
    }
}
