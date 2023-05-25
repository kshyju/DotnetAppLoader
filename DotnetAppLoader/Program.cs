using DotnetAppLoader;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Pass the worker assembly path as argument. Ex: ./DotnetAppLoader C:/Temp/FunctionApp1.dll");
            return 1;
        }
        var workerAssemblyPath = args[0];

        using (var appLoader = AppLoader.Instance)
        {
            appLoader.RunApplication(workerAssemblyPath);

            int counter = 0;
            while (counter < 10000)
            {
                Thread.Sleep(1000);
                if (counter % 10 == 0)
                {
                    Console.WriteLine(counter++);
                }
            }
        }

        return 1;
    }
}
