using DotnetAppLoader;

class Program
{

    static async Task<int> Main(string[] args)
    {
        //var assemblyPath = Path.Combine("..", "..", "App", "PlaceholderApp", "bin", "debug", "net9.0", "PlaceholderApp.dll");
        // Run the customer app assembly directly (Without using Assembly.SetEntryAssembly in startuphook)
        var assemblyPath = Path.Combine("..", "..", "App", "CustomerApp", "bin", "debug", "net9.0", "App.dll");
        if (!File.Exists(assemblyPath))
        {
            Logger.LogInfo($"Assembly to load not found at {assemblyPath}");
        }

        Logger.LogInfo($"Assembly path: {assemblyPath}");
        await Task.Delay(10);

        using (var appLoader = new AppLoader())
        {
            try
            {
                appLoader.RunApplication(assemblyPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling RunApplication from Main.", ex);
            }
        }

        return 1;
    }
}
