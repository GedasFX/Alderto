using Microsoft.AspNetCore.Hosting;

namespace Alderto
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            _ = new Bot.Startup().RunAsync();
            Web.Program.CreateWebHostBuilder(args).Build().Run();
        }
    }
}
