using DotnetAppLoader;

class Program
{

    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Pass the worker assembly path as argument. Ex: ./FunctionsNetHost C:/Temp/SampleApp.dll");
            return 1;
        }

        Logger.LogInfo($"Args: {string.Join(" ", args)}");

        var workerAssemblyPath = args[0];

        Logger.LogInfo($"Environment.ProcessId: {Environment.ProcessId}");

        using (var appLoader = new AppLoader())
        {
            try
            {
                EnvironmentUtil.SetEnvVar("FUNCTIONS_NATIVE_FOO", "N_SetInNative");
                EnvironmentUtil.SetEnvVar("FUNCTIONS_APPLICATION_DIRECTORY", "N-UpdatedFromNative");

                appLoader.RunApplication(workerAssemblyPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling RunApplication from Main.", ex);

                // Keep it running if we want to login to container and inspect something.
                var counter = 0;
                while (counter < 10000)
                {
                    counter++;
                    Thread.Sleep(1000);
                    //if (counter % 10 == 0)
                    {
                        Console.WriteLine(counter);
                    }
                }
            }
        }

        return 1;
    }
}
