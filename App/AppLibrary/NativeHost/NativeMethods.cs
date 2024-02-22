using System;
using System.Runtime.InteropServices;

namespace AppLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeHost
    {
        public IntPtr pNativeApplication;
    }

    internal static unsafe class NativeMethods
    {
        private const string NativeWorkerDll = "FunctionsNetHost.exe";

        static NativeMethods()
        {
            NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, ImportResolver);
        }

        public static void RegisterCallbacks(
            delegate* unmanaged<byte**, int, IntPtr, IntPtr> requestCallback,
            IntPtr grpcHandler)
        {
            _ = register_callbacks(IntPtr.Zero, requestCallback, grpcHandler);
        }


        [DllImport(NativeWorkerDll)]
        private static extern int send_streaming_message(IntPtr pInProcessApplication, byte* streamingMessage, int streamingMessageSize);

        [DllImport(NativeWorkerDll)]
        private static extern unsafe int register_callbacks(IntPtr pInProcessApplication,
            delegate* unmanaged<byte**, int, IntPtr, IntPtr> requestCallback,
            IntPtr grpcHandler);

        /// <summary>
        /// Custom import resolve callback.
        /// When trying to resolve "FunctionsNetHost", we return the handle using GetMainProgramHandle API in this callback.
        /// </summary>
        private static IntPtr ImportResolver(string libraryName, System.Reflection.Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == NativeWorkerDll)
            {
#if NET6_0
                if (OperatingSystem.IsLinux())
                {
                    return NativeLibraryLinux.GetMainProgramHandle();
                }
#elif NET7_0_OR_GREATER
                return NativeLibrary.GetMainProgramHandle();
#else
                throw new PlatformNotSupportedException("Interop communication with FunctionsNetHost is not supported in the current platform. Consider upgrading your project to .NET 7.0 or later.");
#endif
            }

            // Return 0 so that built-in resolving code will be executed.
            return IntPtr.Zero;
        }
    }

}