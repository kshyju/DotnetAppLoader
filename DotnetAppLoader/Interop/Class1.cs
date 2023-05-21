using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAppLoader.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace FunctionsNetHost
    {
        using System;
        using System.Runtime.InteropServices;

        public delegate void RequestHandlerDelegate(ref byte buffer, int size, IntPtr handle);

        public sealed class Singleton
        {
            static readonly Singleton instance = new Singleton();

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Singleton()
            {
            }

            Singleton()
            {
            }

            public static Singleton Instance
            {
                get
                {
                    return instance;
                }
            }
        }

        public class NativeHostApplication
        {
            static readonly NativeHostApplication instance = new NativeHostApplication();

            static NativeHostApplication()
            {
            }

            NativeHostApplication()
            {
            }

            public static NativeHostApplication Instance
            {
                get
                {
                    return instance;
                }
            }

            private IntPtr handle;
            private RequestHandlerDelegate callback;

            public void HandleIncomingMessage(byte[] buffer, int size)
            {
                Logger.Log("HandleIncomingMessage");

                GCHandle bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    IntPtr bufferPtr = bufferHandle.AddrOfPinnedObject();

                    // callback(ref bufferPtr, size, handle);
                }
                finally
                {
                    bufferHandle.Free();
                }
            }

            public void SetCallbackHandles(RequestHandlerDelegate requestCallback, IntPtr grpcHandle)
            {
                Logger.Log("SetCallbackHandles");

                callback = requestCallback;
                handle = grpcHandle;
            }
        }

    }
}
