using System.Runtime.InteropServices;

internal static class NativeMethods
{
    [DllImport("hostfxr.dll")]
    public static extern int CallMain(IntPtr mainAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("hostfxr.dll")]
    public static extern IntPtr hostfxr_initialize_for_runtime_config(
        [MarshalAs(UnmanagedType.LPWStr)] string runtimeConfigPath,
        [MarshalAs(UnmanagedType.LPWStr)] string appBasePath,
        [MarshalAs(UnmanagedType.LPWStr)] string appName);

    [DllImport("hostfxr.dll")]
    public static extern IntPtr hostfxr_get_main_address(IntPtr hostHandle);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
}