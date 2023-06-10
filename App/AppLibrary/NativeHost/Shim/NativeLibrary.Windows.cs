using System.Runtime.InteropServices;

namespace AppLibrary
{
    // NativeLibrary.GetMainProgramHandle is available only from NET7 onwards.

    internal class NativeLibraryWindows
    {
        // Define the GetModuleHandle method from the kernel32 library
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern nint GetModuleHandle(string lpModuleName);

        internal static nint GetMainProgramHandle()
        {
#pragma warning disable CS8625 // Passing null will return main program handle.
            return GetModuleHandle(lpModuleName: null);
#pragma warning restore CS8625
        }
    }
}
