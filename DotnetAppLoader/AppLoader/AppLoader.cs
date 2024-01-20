using DotnetAppLoader;
using DotnetAppLoader.Diagnostics;
using FunctionsNetHost;
using System.Diagnostics;
using System.Runtime.InteropServices;

internal sealed class AppLoader : IDisposable
{
    private IntPtr _hostfxrHandle = IntPtr.Zero;
    private IntPtr _hostContextHandle = IntPtr.Zero;
    private bool _disposed;

    public int RunApplication(string assemblyPath)
    {
        Logger.LogInfo($"{assemblyPath} file.Exists:{File.Exists(assemblyPath)}");

        unsafe
        {
            var parameters = new NetHost.get_hostfxr_parameters
            {
                size = sizeof(NetHost.get_hostfxr_parameters),
                assembly_path = (char*)Marshal.StringToHGlobalUni(assemblyPath).ToPointer()
            };

            Stopwatch sw = Stopwatch.StartNew();
            var hostfxrFullPath = NetHost.GetHostFxrPath(&parameters);
            sw.Stop();
            Logger.LogInfo($"get_hostfxr_path took {sw.ElapsedMilliseconds}ms");
            Logger.LogInfo($"get_hostfxr_path: {hostfxrFullPath}");

            sw.Restart();

            AppLoaderEventSource.Log.HostFxrLoadStart(hostfxrFullPath);
            _hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
            if (_hostfxrHandle == IntPtr.Zero)
            {
                Logger.LogInfo($"Failed to load hostfxr. hostfxrFullPath:{hostfxrFullPath}");
            }
            AppLoaderEventSource.Log.HostFxrLoadStop();
            
            sw.Stop();
            Logger.LogInfo($"NativeLibrary.Load took {sw.ElapsedMilliseconds}ms");
            Logger.LogInfo($"hostfxr loaded successfully.");
            Logger.LogInfo($"About to call HostFxr.Initialize.");

            AppLoaderEventSource.Log.HostFxrInitializeForDotnetCommandLineStart(assemblyPath);
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
            AppLoaderEventSource.Log.HostFxrInitializeForDotnetCommandLineStop();

            Logger.LogInfo($"About to call HostFxr.Run");
            AppLoaderEventSource.Log.HostFxrRunAppStart();
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