using DotnetAppLoader;

class Program
{
    static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine(@"Pass the worker assembly path as argument. Ex: ./FunctionsNetHost D:\src\DotnetAppLoader\App\SampleApp\bin\Debug\net9.0\SampleApp.dll");
            return 1;
        }

        Logger.LogInfo($"Args: {string.Join(" ", args)}");

        var workerAssemblyPath = args[0];

        var diagnosticPort = args.Length > 1 ? args[1] : string.Empty;

        Logger.LogInfo($"workerAssemblyPath: {workerAssemblyPath}");
        await Task.Delay(10);

        using (var appLoader = new AppLoader())
        {
            try
            {
                EnvironmentUtils.SetValue("AOT_FOO", "Bar");

                if (!string.IsNullOrEmpty(diagnosticPort))
                {
                    Environment.SetEnvironmentVariable("DOTNET_DiagnosticPorts", diagnosticPort);
                    Logger.LogInfo($"Diagnostic port env variable set to: {diagnosticPort}");
                }

                appLoader.RunApplication(workerAssemblyPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling RunApplication from Main.", ex);
            }
        }

        return 1;
    }
}
