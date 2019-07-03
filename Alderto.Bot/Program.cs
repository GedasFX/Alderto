using Discord.WebSocket;

namespace Alderto.Bot
{
    class Program
    {
        static void Main(string[] args)
            => new Startup().Run()
                .GetAwaiter().GetResult();
    }
}
