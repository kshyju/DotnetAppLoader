using DotnetAppLoader;

class Program
{

    static async Task<int> Main(string[] args)
    {
        var placeHolderAssemblyPath = Path.Combine("..", "..", "App", "PlaceholderApp", "bin", "debug", "net9.0", "PlaceholderApp.dll");
        if (!File.Exists(placeHolderAssemblyPath))
        {
            Logger.LogInfo($"Placeholder app assembly to load not found at {placeHolderAssemblyPath}");
        }

        Logger.LogInfo($"placeholder app assembly path: {placeHolderAssemblyPath}");
        await Task.Delay(10);

        using (var appLoader = new AppLoader())
        {
            try
            {
                appLoader.RunApplication(placeHolderAssemblyPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling RunApplication from Main.", ex);
            }
        }

        return 1;
    }
}
