using DotnetAppLoader;

class Program
{

    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Pass the .NET (customer )assembly path as argument. Ex: ./FunctionsNetHost C:/Temp/SampleApp.dll");
            return 1;
        }

        Logger.LogInfo($"Raw Command line args: {string.Join(" ", args)}");

        var customerAssemblyPath = args[0];

        using (var appLoader = new AppLoader())
        {
            try
            {
                appLoader.RunApplication(customerAssemblyPath);
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
