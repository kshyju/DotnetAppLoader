using System.Runtime.InteropServices;

namespace SampleApp
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeHost
    {
        public IntPtr pNativeApplication;
    }

    internal static unsafe partial class NativeMethods
    {
        public static NativeHost GetNativeHostData()
        {
            IntPtr mainExecutableHandle = NativeLibrary.GetMainProgramHandle();
            Logger.LogInfo($"MainProgramHandle: {mainExecutableHandle}");

            var getPropPtr = NativeLibrary.GetExport(mainExecutableHandle, "get_application_properties");
            Logger.LogInfo($"get_application_properties address: {getPropPtr}");

            // Create a delegate for the "get_application_properties" method
            var getPropMethod = Marshal.GetDelegateForFunctionPointer<GetPropDelegate>(getPropPtr);

            var hostData = new NativeHost
            {
                pNativeApplication = IntPtr.Zero // Set the required value for PNativeApplication
            };

            // Call the native method
            int result = getPropMethod(hostData);

            Logger.LogInfo("Result: " + result);
            Logger.LogInfo("pNativeApplication address: " + hostData.pNativeApplication);

            return hostData;
        }
    }

    // Declare the delegate for the "get_application_properties" method
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int GetPropDelegate(NativeHost hostData);
}
