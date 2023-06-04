using AppLibrary;
using System.Collections;

namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.LogInfo($"Inside SampleApp Main");

            // If you want to inspect something from the environment before our code crashes, set this env variable
            // so that it runs this loop before calling CreateClient()
            var dontCrashEnvVarValue = Environment.GetEnvironmentVariable("DONTCRASH");

            if (dontCrashEnvVarValue != null)
            {
                for (var i = 1; i <= 5000; i++)
                {
                    Logger.LogInfo(" - " + i);
                    Thread.Sleep(1000);
                }
            }

            Logger.LogInfo($"Before calling  Initializer.Init");

            Initializer.Init();

            Logger.LogInfo($"After calling  Initializer.Init");
        }
    }
}