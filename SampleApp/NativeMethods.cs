using System.Runtime.InteropServices;

namespace SampleApp
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
#else
            // Code for other cases (optional)
            Console.WriteLine("This code executes in all other cases.");
#endif



            return hostData;
        }
    }

    // Declare the delegate for the "get_application_properties" method
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int GetPropDelegate(NativeHost hostData);
}
