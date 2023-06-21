using DotnetAppLoader;
using DotnetAppLoader.AppLoader;

class Program
{
    internal static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Pass the worker assembly path as argument. Ex: ./FunctionsNetHost C:/Temp/SampleApp.dll");
            return 1;
        }

        var workerAssemblyPath = args[0];

        using (var appLoader = new AppLoader())
        {
            try
            {

                Preloader.Preload();

                await Task.Delay(2000);
                Logger.LogInfo($"Aboug to call RunApplication for assembly:{workerAssemblyPath}");
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
