using System;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        // some hardcoded values for now.
        string runtimePath = @"C:\Program Files\dotnet\host\fxr\7.0.5";
        string hostfxrPath = IntPtr.Size == 8 ? "hostfxr.dll" : "hostfxr32.dll";
        string hostfxrFullPath = Path.Combine(runtimePath, hostfxrPath);
        var assemblyPath = @"C:\Dev\OSS\DotnetAppLoader\SampleApp\bin\Debug\net7.0\SampleApp.dll";

        IntPtr hostfxrHandle = IntPtr.Zero;
        IntPtr assemblyHandle = IntPtr.Zero;
        try
        {
            hostfxrHandle = NativeMethods.LoadLibrary(hostfxrFullPath);
            if (hostfxrHandle == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to load {hostfxrFullPath}");
                return;
            }

            // Initialize the .NET Core runtime.
            var runtimeConfigPath = @"SampleApp.runtimeconfig.json";
            var baseDir = @"C:\Dev\OSS\DotnetAppLoader\SampleApp\bin\Debug\net7.0";
            IntPtr handle = NativeMethods.hostfxr_initialize_for_runtime_config(runtimeConfigPath, baseDir, null);
            if (handle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to initialize the .NET Core runtime.");
                return;
            }

            // Load the assembly.
            assemblyHandle = NativeMethods.LoadLibrary(assemblyPath);
            if (assemblyHandle == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to load {assemblyPath}");
                return;
            }

            if (NativeMethods.GetProcAddress(assemblyHandle, "Main") == IntPtr.Zero)
            {
                Console.WriteLine("Failed to get function pointer to Main method.");
                return;
            }

            // Get the function pointer to the main method.
            // IntPtr mainPtr = NativeMethods.hostfxr_get_main_address(hostfxrHandle);
            IntPtr mainPtr = NativeMethods.GetProcAddress(assemblyHandle, "Main");
            if (mainPtr == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to get function pointer to Main method.");
                NativeMethods.FreeLibrary(assemblyHandle);
                return;
            }

            Console.WriteLine($"Invoking the main method");
            IntPtr result = NativeMethods.CallMain(mainPtr);
            Console.WriteLine($"Result of main method: {result}");
        }
        finally
        {
            if (hostfxrHandle != IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(hostfxrHandle); // Free the hostfxr library handle when finished
                Console.WriteLine($"Freed hostfxr library handle");
            }
            if (assemblyHandle != IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(assemblyHandle); 
                Console.WriteLine($"Freed assemblyHandle");
            }
        }
    }
}
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