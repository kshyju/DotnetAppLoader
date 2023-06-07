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

            Initializer.Init();

            //var allEnvVars = Environment.GetEnvironmentVariables();
            //Print(allEnvVars);
        }

        private static void Print(IDictionary dictionary)
        {
            Logger.LogInfo($"Printing all EnvironmentVariables starts with FUNCTIONS_");

            foreach (DictionaryEntry item in dictionary)
            {
                if (item.Key.ToString().StartsWith("FUNCTIONS_") == false)
                {
                    continue;
                }

                Logger.LogInfo($"ENV VARIABLE {item.Key}:{item.Value}");
            }
        }
    }
}