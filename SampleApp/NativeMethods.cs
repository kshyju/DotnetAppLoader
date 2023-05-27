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
        // In windows platform, we need to set "FunctionsNetHost.exe";
        private const string NativeWorkerDll = "FunctionsNetHost";

        public static NativeHost GetNativeHostData()
        {
            var currentLdLibraryPath = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
            Console.WriteLine($" [SampleApp] LD_LIBRARY_PATH value: {currentLdLibraryPath}");

            Console.WriteLine(" [SampleApp] About to call get_application_properties from SampleApp.");

            _ = get_application_properties(out var hostData);

            return hostData;
        }

        [DllImport(NativeWorkerDll, CharSet = CharSet.Auto)]
        private static extern int get_application_properties(out NativeHost hostData);
    }
}
