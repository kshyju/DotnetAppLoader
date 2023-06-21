using DotnetAppLoader;
using FunctionsNetHost;
using System.Runtime.InteropServices;

internal sealed class AppLoader : IDisposable
{
    private static readonly AppLoader _instance = new();
    private IntPtr _hostfxrHandle = IntPtr.Zero;
    private bool _disposed;

    internal AppLoader()
    {
        LoadHostfxrLibrary();
    }

    private void LoadHostfxrLibrary()
    {
        // If having problems with the managed host, enable the following:
        //Environment.SetEnvironmentVariable("COREHOST_TRACE", "1");
        // In Unix environment, you need to run the below command in the terminal to set the environment variable.
        // export COREHOST_TRACE=1

        var hostfxrFullPath = NetHost.GetHostFxrPath();
        Logger.LogInfo($"hostfxrFullPath: {hostfxrFullPath}");

        _hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
        if (_hostfxrHandle == IntPtr.Zero)
        {
            Logger.LogInfo($"Failed to load hostfxr. hostfxrFullPath:{hostfxrFullPath}");
            return;
        }
        Logger.LogInfo($"hostfxr loaded successfully");
    }

    public int RunApplication(string assemblyPath)
    {
        Logger.LogInfo($"{assemblyPath} file.Exists:{File.Exists(assemblyPath)}");

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
                        
            return HostFxr.Run(hostContextHandle);
        }
    }

    private void PrePageAssemblies()
    {
        var assemblies = new string[] { "" };

    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (!disposing)
            {
                return;
            }

            if (_hostfxrHandle != IntPtr.Zero)
            {
                NativeLibrary.Free(_hostfxrHandle);
                _hostfxrHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}