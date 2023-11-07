using DotnetAppLoader;
using FunctionsNetHost;
using System.Runtime.InteropServices;

internal sealed class AppLoader : IDisposable
{
    private static readonly AppLoader _instance = new();
    private IntPtr _hostfxrHandle = IntPtr.Zero;
    private IntPtr _hostContextHandle = IntPtr.Zero;
    private bool _disposed;

    internal AppLoader()
    {
       // LoadHostfxrLibrary();
    }

    //private void LoadHostfxrLibrary()
    //{
    //    // If having problems with the managed host, enable the following:
    //    //Environment.SetEnvironmentVariable("COREHOST_TRACE", "1");
    //    // In Unix environment, you need to run the below command in the terminal to set the environment variable.
    //    // export COREHOST_TRACE=1

    //    //var hostfxrFullPath = NetHost.GetHostFxrPath();
    //    //Logger.LogInfo($"hostfxrFullPath: {hostfxrFullPath}");

    //    //_hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
    //    //if (_hostfxrHandle == IntPtr.Zero)
    //    //{
    //    //    Logger.LogInfo($"Failed to load hostfxr. hostfxrFullPath:{hostfxrFullPath}");
    //    //    return;
    //    //}
    //    //Logger.LogInfo($"hostfxr loaded successfully");
    //}

    public int RunApplication(string assemblyPath)
    {
        Logger.LogInfo($"{assemblyPath} file.Exists:{File.Exists(assemblyPath)}");

        unsafe
        {
            var parameters = new NetHost.get_hostfxr_parameters
            {
                size = sizeof(NetHost.get_hostfxr_parameters),
                assembly_path = (char*)Marshal.StringToHGlobalAnsi(assemblyPath).ToPointer()
            };

            var hostfxrFullPath = NetHost.GetHostFxrPath(parameters);
            Logger.LogInfo($"hostfxrFullPath: {hostfxrFullPath}");

            _hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
            if (_hostfxrHandle == IntPtr.Zero)
            {
                Logger.LogInfo($"Failed to load hostfxr. hostfxrFullPath:{hostfxrFullPath}");
            }

            Logger.LogInfo($"hostfxr loaded successfully1");
            Logger.LogInfo($"About to call HostFxr.Initialize1");

            var error = HostFxr.Initialize(1, new[] { assemblyPath }, IntPtr.Zero, out _hostContextHandle);

            if (_hostContextHandle == IntPtr.Zero)
            {
                Logger.LogInfo(
                    $"Failed to initialize the .NET Core runtime. assemblyPath:{assemblyPath}");
                return -1;
            }

            if (error < 0)
            {
                return error;
            }

            Logger.LogInfo($"About to call HostFxr.Run");
            return HostFxr.Run(_hostContextHandle);
        }
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
                Logger.LogInfo($"Freed hostfxr library handle");
                _hostfxrHandle = IntPtr.Zero;
            }

            if (_hostContextHandle != IntPtr.Zero)
            {
                HostFxr.Close(_hostContextHandle);
                Logger.LogInfo($"Closed hostcontext handle");
                _hostContextHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}