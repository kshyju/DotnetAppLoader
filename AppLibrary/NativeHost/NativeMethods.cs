using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AppLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeHost
    {
        public IntPtr pNativeApplication;
    }



    internal static unsafe partial class NativeMethods
    {
        static NativeMethods()
        {
            var downloadDirectory = @"/mnt/c/Temp/app/dotnetapploader";
           // AddDllDirectory(downloadDirectory);
          //  Logger.LogInfo($"AddDllDirectory called with:{downloadDirectory}");

            string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            string libraryPath = Path.Combine(downloadDirectory, "FunctionsNetHost");

            // Load the library
            IntPtr libraryHandle = NativeLibrary.Load(libraryPath);
            Logger.LogInfo($"libraryHandle:{libraryHandle}");
            NativeLibrary.SetDllImportResolver(typeof(Initializer).Assembly, ImportResolver);
        }
        // In windows platform, we need to set "FunctionsNetHost.exe";
        private const string NativeWorkerDll = "FunctionsNetHost";

        public static NativeHost GetNativeHostData()
        {
            var currentLdLibraryPath = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
            Console.WriteLine($" [SampleApp] LD_LIBRARY_PATH value: {currentLdLibraryPath}");

            Console.WriteLine(" [SampleApp] About to call get_application_properties from SampleApp.");

            _ = get_application_properties(out var hostData);

            return hostData;
        }

        [DllImport(NativeWorkerDll, CharSet = CharSet.Auto)]
        private static extern int get_application_properties(out NativeHost hostData);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int AddDllDirectory(string NewDirectory);

        private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;
            if (libraryName == NativeWorkerDll)
            {
                Logger.LogInfo($"ImportResolver: libraryName:{libraryName}");

                var path = @"/mnt/c/Temp/app/dotnetapploader/FunctionsNetHost";
               // Logger.LogInfo($"ImportResolver: path:{path}");
                // Try using the system library 'libmylibrary.so.5'
                NativeLibrary.TryLoad(NativeWorkerDll, out libHandle);
            }

            return libHandle;
        }
    }
}