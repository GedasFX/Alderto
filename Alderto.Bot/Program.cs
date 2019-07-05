namespace Alderto.Bot
{
    internal class Program
    {
        private static void Main(string[] args)
            => new Startup().RunAsync().Wait();
    }
}
