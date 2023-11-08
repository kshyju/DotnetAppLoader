using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AppLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeHost
    {
        public IntPtr pNativeApplication;
    }

    internal static unsafe partial class NativeMethods
    {
        static NativeMethods()
        {
            NativeLibrary.SetDllImportResolver(typeof(NativeClient).Assembly, ImportResolver);
        }

        private const string NativeWorkerDll = "FunctionsNetHost";

        public static void RegisterCallbacks(
                    delegate* unmanaged<byte**, int, IntPtr, IntPtr> requestCallback,
                    IntPtr grpcHandler)
        {
            _ = register_callbacks(IntPtr.Zero, requestCallback, grpcHandler);
        }

        [DllImport(NativeWorkerDll)]
        private static extern unsafe int register_callbacks(IntPtr pInProcessApplication,
            delegate* unmanaged<byte**, int, IntPtr, IntPtr> requestCallback,
            IntPtr grpcHandler);

        [DllImport(NativeWorkerDll, CharSet = CharSet.Auto)]
        private static extern int get_application_properties(out NativeHost hostData);


        private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == NativeWorkerDll)
            {
#if NET6_0
                if (OperatingSystem.IsWindows())
                {
                    Logger.LogInfo("NET6 0 and OperatingSystem.IsWindows");
                    return NativeLibraryWindows.GetMainProgramHandle();
                }
                else if (OperatingSystem.IsLinux())
                {
                    Logger.LogInfo("NET6 0 and OperatingSystem.IsLinux");
                    return NativeLibraryLinux.GetMainProgramHandle();
                }
                else
                {
                    throw new PlatformNotSupportedException("Not supported in this platform. Consider upgrading your project to .NET 7.0 or later.");
                }
#elif NET7_0_OR_GREATER
                Logger.LogInfo("NET7_0_OR_GREATER");
                return NativeLibrary.GetMainProgramHandle();
#else
        throw new PlatformNotSupportedException("Interop communication with native layer is not supported in the current platform. Consider upgrading your project to .NET 7.0 or later.");
#endif
            }

            return IntPtr.Zero;
        }
    }
}