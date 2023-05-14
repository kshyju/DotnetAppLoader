namespace DotnetAppLoader
{
    internal class AppLoader
    {
        public void Load(string assemblyPath, string tfm)
        {
            var baseDirectory = Path.GetDirectoryName(assemblyPath)!;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assemblyPath);
            var runtimeConfigPath = Path.Combine(baseDirectory, $"{fileNameWithoutExtension}.runtimeconfig.json");

            // temporarily hardcoded path to hostfxr dll.
            string runtimePath = @"C:\Program Files\dotnet\host\fxr\7.0.5";
            string hostfxrFileName = IntPtr.Size == 8 ? "hostfxr.dll" : "hostfxr32.dll";
            string hostfxrFullPath = Path.Combine(runtimePath, hostfxrFileName);


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
                Console.WriteLine($"Hostfxr library loaded.");

                // Initialize the .NET Core runtime.
                IntPtr handle = NativeMethods.hostfxr_initialize_for_runtime_config(runtimeConfigPath, baseDirectory, appName: null);
                if (handle == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to initialize the .NET Core runtime.");
                    return;
                }
                Console.WriteLine("Initialized the .NET Core runtime.");

                // Load the assembly.
                assemblyHandle = NativeMethods.LoadLibrary(assemblyPath);
                if (assemblyHandle == IntPtr.Zero)
                {
                    Console.WriteLine($"Failed to load {assemblyPath}");
                    return;
                }
                Console.WriteLine($"Loaded {assemblyPath}");

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
                    NativeMethods.FreeLibrary(hostfxrHandle);
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
}
