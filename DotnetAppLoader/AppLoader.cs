using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace DotnetAppLoader
{
    internal static class AppLoader
    {
        public static int RunApplication(string assemblyPath)
        {
            // If having problems with the managed host, enable the following:
            Environment.SetEnvironmentVariable("COREHOST_TRACE", "1");
            Environment.SetEnvironmentVariable("COREHOST_TRACEFILE", "./CoreHostTraceLog.txt");

            Console.WriteLine($"assemblyPath to load:{assemblyPath}");

            var hostfxrFullPath = HostFxr.GetPath();
            Console.WriteLine($"hostfxrFullPath:{hostfxrFullPath}");
            var dotnetBasePath = HostFxr.GetDotnetRootPath();
            Console.WriteLine($"dotnetBasePath:{dotnetBasePath}");

            IntPtr hostfxrHandle = IntPtr.Zero;
            
            try
            {
                hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
                if (hostfxrHandle == IntPtr.Zero)
                {
                    Console.WriteLine($"Failed to load {hostfxrFullPath}");
                    return -1;
                }

                Console.WriteLine($"Hostfxr library loaded.");

                var hostPath = Environment.CurrentDirectory;
                Console.WriteLine($"hostPath:{hostPath}");

                unsafe
                {
                    fixed (char* hostPathPointer = hostPath)
                    fixed (char* dotnetRootPointer = dotnetBasePath)
                    {
                        var parameters = new HostFxr.hostfxr_initialize_parameters
                        {
                            size = sizeof(HostFxr.hostfxr_initialize_parameters),
                            host_path = hostPathPointer,
                            dotnet_root = dotnetRootPointer
                        };

                        var error = HostFxr.Initialize(1, new string[] { assemblyPath }, ref parameters, out var host_context_handle);

                        if (host_context_handle == IntPtr.Zero)
                        {
                            Console.WriteLine("Failed to initialize the .NET Core runtime.");
                            return -1;
                        }

                        if (error < 0)
                        {
                           return error;
                        }

                        return HostFxr.Run(host_context_handle);
                    }
                }

            }
            finally
            {
                if (hostfxrHandle != IntPtr.Zero)
                {
                    NativeLibrary.Free(hostfxrHandle);
                    Console.WriteLine($"Freed hostfxr library handle");
                }
            }            
        }
    }
}
