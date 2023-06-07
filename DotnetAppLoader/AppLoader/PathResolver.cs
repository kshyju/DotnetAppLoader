// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotnetAppLoader;

namespace FunctionsNetHost
{
    internal static partial class PathResolver
    {
        private static Dictionary<string, string>? _hostFxrPathDictionary;

        private static void Print(Dictionary<string, string> dict)
        {
            Logger.LogInfo($"dict.Count: {dict.Count}");
            foreach (var kvp in dict)
            {
                Logger.LogInfo($" {kvp.Key}: {kvp.Value}");
            }
        }

        internal static string GetHostFxrPath(string targetFrameworkVersion)
        {
            Logger.LogInfo($"targetFrameworkVersion:{targetFrameworkVersion}");
            if (_hostFxrPathDictionary is null)
            {
#if LINUX
                _hostFxrPathDictionary = GetUnixHostFxrPaths();
#else
                _hostFxrPathDictionary = GetWindowsHostFxrPaths();
#endif
            }

            Print(_hostFxrPathDictionary);

            var majorVersion = GetMajorVersion(targetFrameworkVersion);
            if (_hostFxrPathDictionary.TryGetValue(majorVersion, out var hostFxrFullPath))
            {
                return hostFxrFullPath;
            }

            throw new PlatformNotSupportedException($"No hostfxr found for the tfm:{targetFrameworkVersion}");
        }
        
        private static string GetMajorVersion(string versionString)
        {
            string[] parts = versionString.Split('.');
            if (parts.Length > 0)
            {
                return parts[0];
            }

            throw new InvalidOperationException(
                $"Expected a version string in {{major}}.{{minor}} format. Received:{versionString}");
        }
        
        private static Dictionary<string, string> GetHostFxPathsForAllVersions(string hostFxrRoot, string hostfxrDll)
        {
            var directoryPaths = Directory.GetDirectories(hostFxrRoot, "*", SearchOption.TopDirectoryOnly);
            Logger.LogInfo($"hostFxrRoot:{hostFxrRoot}, Child directory count:{directoryPaths.Length}");

            var latest = GetMajorVersionPaths(directoryPaths);
            
            // Update the value of each dictionary entry to append the dll name.
            foreach (var versionEntry in latest)
            {
                var versionDir = versionEntry.Value;
                var versionDllPath = Path.Combine(versionDir, hostfxrDll);
                latest[versionEntry.Key] = Path.GetFullPath(versionDllPath);
            }

            return latest;
        }

        private static Dictionary<string, string> GetMajorVersionPaths(string[] directoryPaths)
        {
            var dict = new Dictionary<string, string>();

            foreach (var path in directoryPaths)
            {
                var version = GetVersionFromPath(path);
                var majorVersion = GetMajorVersion(version);

                if (dict.TryGetValue(majorVersion, out var currentPath))
                {
                    var currentVersion = GetVersionFromPath(currentPath);
                    if (IsHigherVersion(version, currentVersion))
                    {
                        dict[majorVersion] = path;
                    }
                }
                else
                {
                    dict.Add(majorVersion, path);
                }
            }

            return dict;
        }


        private static bool IsHigherVersion(string version1, string version2)
        {
            // Rule 2: If both versions can be parsed to Version instances, use that for comparison
            if (Version.TryParse(version1, out var v1) && Version.TryParse(version2, out var v2))
            {
                return v1 > v2;
            }

            bool hasPreview1 = version1.IndexOf("preview", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasPreview2 = version2.IndexOf("preview", StringComparison.OrdinalIgnoreCase) >= 0;

            // Rule 1: If either version has "preview", return the one without "preview"
            if (hasPreview1 && !hasPreview2)
            {
                return false;
            }
            else if (!hasPreview1 && hasPreview2)
            {
                return true;
            }

            // Default comparison: Use string comparison
            return string.Compare(version1, version2, StringComparison.OrdinalIgnoreCase) > 0;
        }

        private static string GetVersionFromPath(string path)
        {
            return Path.GetFileName(path);
        }
    }
}