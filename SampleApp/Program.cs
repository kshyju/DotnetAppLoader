namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (AppContext.GetData("AZURE_FUNCTIONS_NATIVE_HOST") is not null)
            {
                Console.WriteLine($"AZURE_FUNCTIONS_NATIVE_HOST is not null :) :)  :) ");
            }
            else
            {
                Console.WriteLine($"AZURE_FUNCTIONS_NATIVE_HOST is null!!!!!!!!!");
            }

            for (var i = 1; i <= 2; i++)
            {
                Console.WriteLine("Hello world - " + i);
                Thread.Sleep(1000);
            }
        }
    }
}