using DotnetAppLoader;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Pass the worker assembly path as argunent. Ex: ./DotnetAppLoader C:/Temp/FunctionApp1.dll");
            return 1;
        }
        var workerAssemblyPath = args[0];

        using (var appLoader = AppLoader.Instance)
        {
            appLoader.RunApplication(workerAssemblyPath);
        }

        return 1;
    }
}
