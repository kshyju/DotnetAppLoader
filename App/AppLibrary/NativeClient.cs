using System.Runtime.InteropServices;
using System;

namespace AppLibrary
{
    public class NativeClient
    {
        private GCHandle _gcHandle;
        public unsafe void Start()
        {
            Logger.LogInfo("Inside Customer Assembly AppLibrary.NativeClient.Start()");

            _gcHandle = GCHandle.Alloc(this);
            NativeMethods.RegisterCallbacks(&HandleRequest, (IntPtr)_gcHandle);
        }


        [UnmanagedCallersOnly]
        private static unsafe IntPtr HandleRequest(byte** nativeMessage, int nativeMessageSize, IntPtr grpcHandler)
        {
            var span = new ReadOnlySpan<byte>(*nativeMessage, nativeMessageSize);
            Logger.LogInfo($"Inside Customer Assembly AppLibrary.NativeClient.HandleRequest");

            return IntPtr.Zero;
        }
    }
}
