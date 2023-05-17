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
            // In Unix enviornment, you need to run the below command in the terminal to set the environment variable.
            // export COREHOST_TRACE=1


            Logger.Log($"AssemblyPath to load:{assemblyPath}");

            var hostfxrFullPath = HostFxr.GetPath();
            Logger.Log($"hostfxrFullPath:{hostfxrFullPath}");


            IntPtr hostfxrHandle = IntPtr.Zero;

            try
            {
                hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
                if (hostfxrHandle == IntPtr.Zero)
                {
                    Logger.Log($"Failed to load {hostfxrFullPath}");
                    return -1;
                }

                Logger.Log($"Hostfxr library loaded successfully.");

                var hostPath = Environment.CurrentDirectory;
                Logger.Log($"hostPath: {hostPath}");
                var dotnetBasePath = @"/usr/share/dotnet"; //HostFxr.GetDotnetRootPath();
                Logger.Log($"dotnetBasePath: {dotnetBasePath}");

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

                        var error = HostFxr.Initialize(2, new string[] { "DotnetAppLoader", assemblyPath }, ref parameters, out var host_context_handle);

                        if (host_context_handle == IntPtr.Zero)
                        {
                            Logger.Log($"Failed to initialize the .NET Core runtime. host_context_handle:{host_context_handle}");
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
                    Logger.Log($"Freed hostfxr library handle");
                }
            }
        }
    }
}
