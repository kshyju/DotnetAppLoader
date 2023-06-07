// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace FunctionsNetHost
{
    internal partial class PathResolver
    {
        private static string GetWindowsDotnetRootPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "dotnet");
        }

        // Example: C:\Program Files\dotnet\host\fxr\7.0.5
        private static Dictionary<string, string> GetWindowsHostFxrPaths()
        {
            string hostFxrRoot = Path.Combine(
                GetWindowsDotnetRootPath(),
                "host",
                "fxr");

            return GetHostFxPathsForAllVersions(hostFxrRoot, "hostfxr.dll");
        }
    }
}
