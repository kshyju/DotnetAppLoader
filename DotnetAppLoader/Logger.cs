namespace DotnetAppLoader
{
    internal static class Logger
    {
        public static void LogInfo(string message) => Console.WriteLine($"[DotnetAppLoader] {message}");

        internal static void LogTrace(string v) => LogInfo(v);
    }
}
