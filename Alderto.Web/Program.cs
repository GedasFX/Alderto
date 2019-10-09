using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Alderto.Web
{
    public static class Program
    {
        public static void Main([NotNull] string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }


        [return:NotNull]
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
