using System;

namespace AppLibrary
{
    public static class Logger
    {
        public static void LogInfo(string message) => Console.WriteLine($"  [CustomerApp]  {message}");
    }
}
