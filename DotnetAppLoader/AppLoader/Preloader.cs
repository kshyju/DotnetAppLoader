using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAppLoader.AppLoader
{
    internal class Preloader
    {
        internal static void Preload()
        {

            string[] arr = new string[]
            {
                @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.18\System.Private.CoreLib.dll",
                @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.18\System.Runtime.dll",
                @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.18\System.Collections.Immutable.dll",
                @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.18\System.Collections.Concurrent.dll",
                @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.18\System.Diagnostics.Tracing.dll",
                @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.18\System.Threading.Channels.dll"

            };
            ReadRuntimeAssemblyFiles(arr);
        }
        internal static void ReadRuntimeAssemblyFiles(string[] allFiles)
        {
            try
            {
                // Read File content in 4K chunks
                int maxBuffer = 4 * 1024;
                byte[] chunk = new byte[maxBuffer];
                Random random = new Random();
                foreach (string file in allFiles)
                {
                    // Read file content to avoid disk reads during specialization. This is only to page-in bytes.
                    ReadFileInChunks(file, chunk, maxBuffer, random);
                }
                Logger.LogInfo($"Number of files read:{allFiles.Length}");
            }
            catch (Exception ex)
            {
                Logger.LogInfo($"Failed ReadRuntimeAssemblyFiles" + ex);
            }
        }

        private static void ReadFileInChunks(string file, byte[] chunk, int maxBuffer, Random random)
        {
            Logger.LogInfo($"Reading {file}");
            try
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead;
                    while ((bytesRead = fileStream.Read(chunk, 0, maxBuffer)) != 0)
                    {
                        // Read one random byte for every 4K bytes - 4K is default OS page size. This will help avoid disk read during specialization
                        // see for details on OS page buffering in Windows - https://docs.microsoft.com/en-us/windows/win32/fileio/file-buffering
                        var randomByte = Convert.ToInt32(chunk[random.Next(0, bytesRead - 1)]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo($"Failed to read file {file}. {ex}");
            }
        }
    }
}
