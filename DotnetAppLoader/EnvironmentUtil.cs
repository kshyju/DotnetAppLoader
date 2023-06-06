#if LINUX
using System.Runtime.InteropServices;
#endif

namespace DotnetAppLoader
{
    internal static class EnvironmentUtil
    {
#if LINUX
    [DllImport("libc")]
    private static extern int setenv(string name, string value, int overwrite);
#endif

        internal static void SetEnvVar(string name, string value)
        {
#if LINUX
            setenv(name, value, 1);
#else
            Environment.SetEnvironmentVariable(name, value);
#endif
        }
    }
}
