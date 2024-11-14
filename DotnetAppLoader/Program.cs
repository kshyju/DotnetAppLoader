using DotnetAppLoader;

class Program
{

    static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Pass the worker assembly path as argument. Ex: ./FunctionsNetHost C:/Temp/SampleApp.dll");
            return 1;
        }

        Logger.LogInfo($"Args: {string.Join(" ", args)}");

        var workerAssemblyPath = args[0];

        Logger.LogInfo($"workerAssemblyPath: {workerAssemblyPath}");
        await Task.Delay(10);

        using (var appLoader = new AppLoader())
        {
            try
            {
                SetEnvironmentVariableLater();
                appLoader.RunApplication(workerAssemblyPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling RunApplication from Main.", ex);
            }
        }

        return 1;
    }

    static async Task SetEnvironmentVariableLater()
    {
        Logger.LogInfo("Waiting 5 seconds before setting environment variable");
        await Task.Delay(TimeSpan.FromSeconds(5));
        var envVarName = "AOTFoo";
        var envVarValue = "Hello world 1";
        EnvironmentUtils.SetValue(envVarName, envVarValue);
        Logger.LogInfo($"Set environment variable {envVarName} to {envVarValue}");
    }
}
