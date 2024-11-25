
using System.Reflection;

namespace FunctionsNetHost
{
    public class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("[Placeholder] Hello from FunctionsNetHost Placeholder app main method.");
            var entryAssembly = Assembly.GetEntryAssembly()!;

            Console.WriteLine($"[Placeholder] EntryAssembly: {entryAssembly.FullName}");

            return AppDomain.CurrentDomain.ExecuteAssemblyByName(entryAssembly.GetName());

            //return 0;
        }
    }
}