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

            Logger.LogInfo($"Environment.ProcessId: {Environment.ProcessId}");

            var userEnvVars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
            PrintEnv(userEnvVars, "User");

            var processEnvVars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            PrintEnv(processEnvVars, "Process");

            var machineEnvVars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            PrintEnv(machineEnvVars, "Machine");

           
            Logger.LogInfo($"After calling  Initializer.Init");
        }

        static void PrintEnv(IDictionary dictionary, string target)
        {
            foreach (DictionaryEntry var in dictionary)
            {
                if (var.Key.ToString().StartsWith("FUNCTIONS_"))
                {
                    Logger.LogInfo($" {target} - ENVIRONMENT VARIABLE : {var.Key}: {var.Value}");
                }
            }
        }
    }
}