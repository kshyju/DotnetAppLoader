using DotnetAppLoader;
using FunctionsNetHost;
using System.Runtime.InteropServices;

internal sealed class AppLoader
{
    public int RunApplication(string assemblyPath, string tfm)
    {
        //var hostfxrFullPath = PathResolver.GetHostFxrPath(tfm);
        //Logger.LogInfo($"hostfxrFullPath: {hostfxrFullPath}");

        var hostfxrFullPath = NativeMethods.GetHostFxrPath();
        Logger.LogInfo($"hostFxr2: {hostfxrFullPath}");

        var hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
        if (hostfxrHandle == IntPtr.Zero)
        {
            Logger.LogInfo($"Failed to load hostfxr. hostfxrFullPath:{hostfxrFullPath}");
            return 0;
        }
        
        Logger.LogInfo($"hostfxr loaded successfully.");
        
        unsafe
        {
            var parameters = new HostFxr.hostfxr_initialize_parameters
            {
                size = sizeof(HostFxr.hostfxr_initialize_parameters)
            };

            var error = HostFxr.Initialize(1, new[] { assemblyPath }, ref parameters, out var hostContextHandle);

            if (hostContextHandle == IntPtr.Zero)
            {
                Logger.LogInfo(
                    $"Failed to initialize the .NET Core runtime. assemblyPath:{assemblyPath}");
                return -1;
            }

            if (error < 0)
            {
                return error;
            }
            Logger.LogInfo($"HostFxr.Initialize done. hostContextHandle:{hostContextHandle}");

            return HostFxr.Run(hostContextHandle);
        }
    }
}