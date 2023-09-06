﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotnetAppLoader;
using System.Runtime.InteropServices;


/// <summary>
/// Manages loading hostfxr & worker assembly.
/// </summary>
internal sealed class AppLoader : IDisposable
{
    private IntPtr _hostfxrHandle = IntPtr.Zero;
    private IntPtr _hostContextHandle = IntPtr.Zero;
    private bool _disposed;

    internal AppLoader()
    {
        LoadHostfxrLibrary();
    }

    private void LoadHostfxrLibrary()
    {
        // If having problems with the managed host, enable the following:
        // Environment.SetEnvironmentVariable("COREHOST_TRACE", "1");
        // In Unix environment, you need to run the below command in the terminal to set the environment variable.
        // export COREHOST_TRACE=1

        var hostfxrFullPath = NetHost.GetHostFxrPath();
        hostfxrFullPath = @"C:\Program Files\dotnet\host\fxr\6.0.16\hostfxr.dll";
        hostfxrFullPath = @"C:\Program Files (x86)\dotnet\host\fxr\7.0.10\hostfxr.dll";
        hostfxrFullPath = @"C:\Program Files (x86)\dotnet\host\fxr\8.0.0-preview.7.23375.6\hostfxr.dll";
        hostfxrFullPath = @"C:\Program Files\dotnet\host\fxr\8.0.0-preview.7.23375.6\hostfxr.dll";

        Logger.LogTrace($"Hostfxr path :{hostfxrFullPath}");

        _hostfxrHandle = NativeLibrary.Load(hostfxrFullPath);
        if (_hostfxrHandle == IntPtr.Zero)
        {
            Logger.LogTrace($"Failed to load hostfxr. hostfxr path:{hostfxrFullPath}");
            return;
        }

        Logger.LogTrace($"hostfxr library loaded successfully.");
    }

    internal int RunApplication(string? assemblyPath)
    {
        ArgumentNullException.ThrowIfNull(assemblyPath, nameof(assemblyPath));

        Logger.LogTrace($"RunApplication called with assemblyPath:{assemblyPath}");

        unsafe
        {
            var parameters = new HostFxr.hostfxr_initialize_parameters
            {
                size = sizeof(HostFxr.hostfxr_initialize_parameters)
            };

            var error = HostFxr.Initialize(1, new[] { assemblyPath }, ref parameters, out _hostContextHandle);

            if (_hostContextHandle == IntPtr.Zero)
            {
                Logger.LogTrace(
                    $"Failed to initialize the .NET Core runtime. Assembly path:{assemblyPath}");
                return -1;
            }

            if (error < 0)
            {
                return error;
            }

            Logger.LogTrace($"hostfxr initialized with {assemblyPath}");
            HostFxr.SetAppContextData(_hostContextHandle, "AZURE_FUNCTIONS_NATIVE_HOST", "1");

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
                Logger.LogTrace($"Freed hostfxr library handle");
                _hostfxrHandle = IntPtr.Zero;
            }

            if (_hostContextHandle != IntPtr.Zero)
            {
                NativeLibrary.Free(_hostContextHandle);
                Logger.LogTrace($"Freed hostcontext handle");
                _hostContextHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}
