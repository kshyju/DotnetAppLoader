using System.IO;
using System.Runtime.InteropServices;

namespace DotnetAppLoader
{

    internal static partial class HostFxrPathResolver
    {
        internal static string GetDotnetRootPath()
        {
            var path = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = GetWindowsDotnetRootPath();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = GetUnixDotnetRootPath();
            }

            return path;
        }

        internal static string GetHostFxrPath()
        {
            var path = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = GetWindowsHostFxrPath();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = GetUnixHostFxrPath();
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            return path;
        }       


        private static string GetLatestVersion(string hostFxrVersionsDirPath)
        {
            // Exclude the preview version for now. Will revisit.
            var versionDirectories = Directory.GetDirectories(hostFxrVersionsDirPath)
                .Where(f => !f.Contains("-preview")).ToArray();

            Array.Sort(versionDirectories);
            var latestVersion = Path.GetFileName(versionDirectories[^1]);
            
            return latestVersion;
        }
    }
}
