using DotnetAppLoader.Interop.FunctionsNetHost;
using System.Runtime.InteropServices;

namespace DotnetAppLoader.Interop
{
    public struct NativeHostData
    {
        public IntPtr pNativeApplication;
        public IntPtr Callback;
    }

    public class NativeApplication
    {
        NativeApplication _instance = new NativeApplication();

        public NativeApplication Instance => _instance;
    }

    public class NativeExports
    {
        public delegate int CallbackDelegate(int value);
        // public delegate int RequestHandlerDelegate(ref byte[] msg, int size, IntPtr grpcHandle);

        // https://github.com/dotnet/runtime/issues/78663

        [UnmanagedCallersOnly(EntryPoint = "get_application_properties")]
        public static int get_application_properties(NativeHostData nativeHostData)
        {
            Logger.Log("get_application_properties was invoked");

            var nativeHostApplication = new NativeHostApplication();
            GCHandle gch = GCHandle.Alloc(nativeHostApplication, GCHandleType.Pinned);
            IntPtr pObj = gch.AddrOfPinnedObject();
            nativeHostData.pNativeApplication = pObj;

            Logger.Log($"nativeHostApplication ptr:{pObj}");

            return 1;
        }


        [UnmanagedCallersOnly(EntryPoint = "register_callbacks")]
        public unsafe static int register_callbacks(IntPtr pInProcessApplication,
                                                delegate* unmanaged<byte**, int, IntPtr, IntPtr> requestCallback,
            IntPtr grpcHandler)
        {
            //Marshal.PtrToStructure(pInProcessApplication, typeof())
            Logger.Log("register_callbacks was invoked");

            return 1;
        }

        [UnmanagedCallersOnly(EntryPoint = "register_callbacks")]
        public unsafe static int send_streaming_message(IntPtr pInProcessApplication,
byte* streamingMessage, int streamingMessageSize)
        {


            Logger.Log("register_callbacks was invoked");



            return 1;
        }
    }
}
