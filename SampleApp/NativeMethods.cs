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
            Console.WriteLine(" [SampleApp] About to call get_application_properties from SampleApp.");
            _ = get_application_properties(out var hostData);

            return hostData;
        }

        public static void RegisterCallbacks(NativeSafeHandle nativeApplication,
            delegate* unmanaged<byte**, int, nint, nint> requestCallback,
            nint grpcHandler)
        {
            Console.WriteLine(" [SampleApp] NativeMethods.RegisterCallbacks");
            _ = register_callbacks(nativeApplication, requestCallback, grpcHandler);
        }


        [DllImport(NativeWorkerDll, CharSet = CharSet.Auto)]
        private static extern int get_application_properties(out NativeHost hostData);

        [DllImport(NativeWorkerDll, CharSet = CharSet.Auto)]
        private static extern int send_streaming_message(NativeSafeHandle pInProcessApplication, byte[] streamingMessage, int streamingMessageSize);

        [DllImport(NativeWorkerDll, CharSet = CharSet.Auto)]
        private static extern unsafe int register_callbacks(NativeSafeHandle pInProcessApplication,
            delegate* unmanaged<byte**, int, nint, nint> requestCallback,
            nint grpcHandler);
    }
}
