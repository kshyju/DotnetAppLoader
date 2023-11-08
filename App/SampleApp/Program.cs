using AppLibrary;
using System.Collections;

namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.LogInfo($"Inside SampleApp Main");
            var os = Environment.OSVersion;
            Logger.LogInfo($"OS: {os}");

            new NativeClient().Start();
        }
    }
}