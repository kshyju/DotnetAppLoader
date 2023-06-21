using AppLibrary;
using Microsoft.Extensions.Configuration;
using System.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.LogInfo($"Inside SampleApp Main");

            Initializer.Init();

            //var allEnvVars = Environment.GetEnvironmentVariables();
            //Print(allEnvVars);

            //Console.ReadLine();
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