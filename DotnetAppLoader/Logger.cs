namespace DotnetAppLoader
{
    internal static class Logger
    {
        public static void LogInfo(string message) => Console.WriteLine($"[DotnetAppLoader] {message}");

        internal static void LogDebug(string v) => LogInfo(v);
    }
}
