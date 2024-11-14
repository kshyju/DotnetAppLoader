
namespace DotnetAppLoader
{
    internal static class EnvironmentUtils
    {
#if OS_LINUX
        [System.Runtime.InteropServices.DllImport("libc")]
        private static extern int setenv(string name, string value, int overwrite);

        [System.Runtime.InteropServices.DllImport("libc")]
        private static extern string getenv(string name);
#endif

        internal static string? GetValue(string environmentVariableName)
        {
#if OS_LINUX
            return getenv(environmentVariableName);
#else
            return Environment.GetEnvironmentVariable(environmentVariableName);
#endif
        }

        internal static void SetValue(string name, string value)
        {
#if OS_LINUX
            setenv(name, value, 1);
#else
            Environment.SetEnvironmentVariable(name, value);
#endif
        }
    }
}
