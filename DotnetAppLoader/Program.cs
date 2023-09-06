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

        return 1;
    }
}
