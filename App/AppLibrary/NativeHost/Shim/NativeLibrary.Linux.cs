using System;
using System.Runtime.InteropServices;

namespace AppLibrary
{
    // NativeLibrary.GetMainProgramHandle is available only from NET7 onwards.

    internal class NativeLibraryLinux
    {
        private const int RTLD_LAZY = 0x00001;

        [DllImport("libdl.so", CharSet = CharSet.Auto)]
        private static extern nint dlerror();

        [DllImport("libdl.so", CharSet = CharSet.Auto)]
        private static extern int dlclose(nint handle);

        [DllImport("libdl.so", CharSet = CharSet.Auto)]
        private static extern nint dlopen(string filename, int flags);

        internal static nint GetMainProgramHandle()
        {
#pragma warning disable CS8625 // Passing null will return main program handle.
            var handle = dlopen(filename: null, RTLD_LAZY);
#pragma warning restore CS8625
            Logger.LogInfo($"GetMainProgramHandle LINUX HANDLE: {handle}");

            if (handle == IntPtr.Zero)
            {
                var error = Marshal.PtrToStringAnsi(dlerror());
                Logger.LogInfo($"Failed to open library: {error}");
            }

            var result = dlclose(handle);
            if (result != 0)
            {
                var error = Marshal.PtrToStringAnsi(dlerror());
                Logger.LogInfo($"Failed to close library: {error}");
            }

            return handle;
        }
    }
}
