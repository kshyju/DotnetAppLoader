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

        Logger.LogInfo($"workerAssemblyPath: {workerAssemblyPath}");

        using (var appLoader = new AppLoader())
        {
            try
            {
                appLoader.RunApplication(workerAssemblyPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling RunApplication from Main.", ex);
            }
        }
        Console.ReadKey();
        return 1;
    }
}
