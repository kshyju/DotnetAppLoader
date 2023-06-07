using System;
using System.IO;
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
            NativeLibrary.SetDllImportResolver(typeof(Initializer).Assembly, ImportResolver);
        }
        // In windows platform, we need to set "FunctionsNetHost.exe";
        private const string NativeWorkerDll = "FunctionsNetHost";

        public static NativeHost GetNativeHostData()
        {
            _ = get_application_properties(out var hostData);

            return hostData;
        }

        [DllImport(NativeWorkerDll, CharSet = CharSet.Auto)]
        private static extern int get_application_properties(out NativeHost hostData);


        private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;
            if (libraryName == NativeWorkerDll)
            {
#if NET7_0_OR_GREATER

                var h = NativeLibrary.GetMainProgramHandle();
                return h;
#else
                throw new PlatformNotSupportedException("Interop communication with native layer is not supported in current platform. Consider upgrading your project to net7.0 or later.");
#endif

            }

            return libHandle;
        }
    }
}