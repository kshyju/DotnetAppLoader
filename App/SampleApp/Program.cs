using AppLibrary;

namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.LogInfo($"Inside Customer assembly SampleApp Main");
            new NativeClient().Start();
        }
    }
}