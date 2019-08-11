namespace Alderto.Bot
{
    internal class Program
    {
        private static void Main()
            => new Startup().RunAsync().Wait();
    }
}
