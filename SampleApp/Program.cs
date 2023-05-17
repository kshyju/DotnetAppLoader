namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (var i = 1; i <= 5; i++)
            {
                Console.WriteLine("Hello world - " + i);
                Thread.Sleep(1000);
            }
        }
    }
}