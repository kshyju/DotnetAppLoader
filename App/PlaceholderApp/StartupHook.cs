using System.Reflection;

internal class StartupHook
{
    public static void Initialize()
    {
        Console.WriteLine("[StartupHook] Hello from StartupHook.Initialize");

        try
        {
            var assemblyPath = Path.Combine("..", "..", "App", "CustomerApp", "bin", "debug", "net9.0", "App.dll");
            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"[StartupHook] Assembly to load not found at {assemblyPath}");
                return;
            }

            Assembly specializedEntryAssembly = Assembly.LoadFrom(assemblyPath);

            Assembly.SetEntryAssembly(specializedEntryAssembly);
            Console.WriteLine($"[StartupHook] Set EntryAssembly to {assemblyPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[StartupHook] Error in StartupHook.Initialize", ex);
        }
    }
}