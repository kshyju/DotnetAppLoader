using System;
using System.Runtime.InteropServices;

namespace AppLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeHost
    {
        public IntPtr pNativeApplication;
    }

    internal unsafe partial class NativeMethods
    {
        public NativeHost GetNativeHostData()
        {
            var hostData = new NativeHost
            {
                pNativeApplication = IntPtr.Zero // Set the required value for PNativeApplication
            };

#if NET7_0
            IntPtr mainExecutableHandle = NativeLibrary.GetMainProgramHandle();
            Logger.LogInfo($"MainProgramHandle: {mainExecutableHandle}");

            var getPropPtr = NativeLibrary.GetExport(mainExecutableHandle, "get_application_properties");
            Logger.LogInfo($"get_application_properties address: {getPropPtr}");

            // Create a delegate for the "get_application_properties" method
            var getPropMethod = Marshal.GetDelegateForFunctionPointer<GetPropDelegate>(getPropPtr);



            // Call the native method
            int result = getPropMethod(hostData);
             return hostData;
#else
            // Code for other cases (optional)
            throw new PlatformNotSupportedException("Interop communication with native layer is not supported in current platform. Consider upgrading your project to net7.0 or later.");
#endif
        }
    }

    // Declare the delegate for the "get_application_properties" method
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int GetPropDelegate(NativeHost hostData);
}
