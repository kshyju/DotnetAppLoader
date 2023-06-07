// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace FunctionsNetHost
{
    internal static partial class PathResolver
    {
        private static string GetUnixDotnetRootPath()
        {
            return Path.Combine($"{Path.DirectorySeparatorChar}usr",
                "share",
                "dotnet");
        }

        ///Example path: usr/share/dotnet/host/fxr/7.0.5
        private static Dictionary<string, string> GetUnixHostFxrPaths()
        {
            var hostFxrRoot = Path.Combine(GetUnixDotnetRootPath(),
                "host",
                "fxr");

            return GetHostFxPathsForAllVersions(hostFxrRoot, "libhostfxr.so");
        }
    }
}