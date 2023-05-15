﻿using System.Runtime.InteropServices;

namespace DotnetAppLoader
{
    static partial class HostFxr
    {
        public unsafe struct hostfxr_initialize_parameters
        {
            public nint size;
            public char* host_path;
            public char* dotnet_root;
        };

        [LibraryImport("hostfxr", EntryPoint = "hostfxr_initialize_for_dotnet_command_line")]
        public unsafe static partial int Initialize(
                int argc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] argv,
                ref hostfxr_initialize_parameters parameters,
                out IntPtr host_context_handle
            );

        [LibraryImport("hostfxr", EntryPoint = "hostfxr_run_app")]
        public static partial int Run(IntPtr host_context_handle);

    }
}
