namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine("Hello world - " + i);
                Thread.Sleep(1000);
            }
        }
    }
}