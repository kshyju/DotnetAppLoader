using System.Runtime.InteropServices;

namespace DotnetAppLoader
{
    public class NativeMethods
    {
        //  [DllImport("hostfxr", CharSet = CharSet.Ansi, ExactSpelling = true)]
        [DllImport("nethost", CharSet = CharSet.Auto)]
        private static extern int get_hostfxr_path(
        [Out] char[] buffer,
        [In, Out] ref int buffer_size,
        IntPtr reserved);

        public static string GetHostFxrPath()
        {
            char[] buffer = new char[500];
            int buffer_size = buffer.Length;

            // Call the get_hostfxr_path function
            int rc = get_hostfxr_path(buffer, ref buffer_size, IntPtr.Zero);

            if (rc != 0)
            {
                throw new InvalidOperationException("Failed to get the hostfxr path.");
            }

            return new string(buffer, 0, buffer_size - 1);
        }
    }

    static partial class HostFxr
    {
        public unsafe struct hostfxr_initialize_parameters
        {
            public nint size;
            public char* host_path;
            public char* dotnet_root;
        };

        //[LibraryImport("hostfxr", EntryPoint = "get_hostfxr_path")]
        //public unsafe static partial int Initialize()

        [LibraryImport("hostfxr", EntryPoint = "hostfxr_initialize_for_dotnet_command_line")]
        public unsafe static partial int Initialize(
                int argc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = 
#if LINUX
    UnmanagedType.LPStr
#else
     UnmanagedType.LPWStr
#endif
            )] string[] argv,
                ref hostfxr_initialize_parameters parameters,
                out IntPtr host_context_handle
            );

        [LibraryImport("hostfxr", EntryPoint = "hostfxr_run_app")]
        public static partial int Run(IntPtr host_context_handle);

        [LibraryImport("hostfxr", EntryPoint = "hostfxr_set_runtime_property_value")]
        public static partial int SetAppContextData(IntPtr host_context_handle, [MarshalAs(
#if LINUX
    UnmanagedType.LPStr
#else
     UnmanagedType.LPWStr
#endif
            )] string name, [MarshalAs(
#if LINUX
    UnmanagedType.LPStr
#else
     UnmanagedType.LPWStr
#endif
            )] string value);

    }
}
