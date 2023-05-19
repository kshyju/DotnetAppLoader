using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAppLoader.Interop
{
    public struct NativeHostData
    {
        public IntPtr pNativeApplication;
        //public CallbackDelegate Callback;
        public IntPtr Callback;
    }

    public delegate int CallbackDelegate(int value);

    public class NativeExports
    {
        public delegate bool FPtr(int value);

        // https://github.com/dotnet/runtime/issues/78663

        [UnmanagedCallersOnly(EntryPoint = "get_application_properties")]
        public static int get_application_properties(IntPtr pNativeHostDataPtr)
        {
            Logger.Log("get_application_properties was invoked");
            NativeHostData nativeHostData = Marshal.PtrToStructure<NativeHostData>(pNativeHostDataPtr);

            // Convert the callback IntPtr to the actual delegate type
            CallbackDelegate callbackDelegate = Marshal.GetDelegateForFunctionPointer<CallbackDelegate>(nativeHostData.Callback);

            // Invoke the callback delegate if it's not null
            if (callbackDelegate != null)
            {
                int result = callbackDelegate.Invoke(42); // Example usage
                Console.WriteLine($"Callback result: {result}");
            }

            return 1;
        }

        // public delegate int RequestHandlerDelegate(ref byte[] msg, int size, IntPtr grpcHandle);


        [UnmanagedCallersOnly(EntryPoint = "register_callbacks")]
        public static int register_callbacks(IntPtr pInProcessApplication,
            IntPtr requestCallback,
            //   FPtr a,
            IntPtr grpcHandler)
        {
            Logger.Log("register_callbacks was invoked");

            return 1;
        }
    }
}
