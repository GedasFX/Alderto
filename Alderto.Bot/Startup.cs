using System;
using System.IO;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLua;

namespace Alderto.Bot
{
    public class Startup
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;

        public Startup()
        {
            var luacode = new Lua();
            for (int i = 0; i < 10000000; i++)
                luacode.DoString(@"
function _" + "dawdawd" + @" (val1, val2)
	if val1 > val2 then
        CurrencyCommands.Yeet()
		return val1 + 1
	else
		return val2 - 1
	end
end
");
            return;
            _config = BuildConfig();

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });
        }

        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Add database
                .AddDbContext<IAldertoDbContext, SqliteDbContext>()

                // Add discord socket client
                .AddSingleton(_client)

                // Add command handling services
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()

                // Add Lua command handler
                .AddSingleton<Lua>()

                // Add logger service
                .AddLogging(lb => { lb.AddConsole(); })
                .AddSingleton<LoggingService>()

                // Add configuration
                .AddSingleton(_config)

                // Build
                .BuildServiceProvider();
        }

        public async Task RunAsync()
        {
            return;
            var services = ConfigureServices();

            var s = _config["commands"];
            Console.WriteLine(s);

            // Enable logging
            await services.GetService<LoggingService>().InstallLogger();

            // Start bot
            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();

            // Install Command handler
            await services.GetRequiredService<CommandHandlingService>().InstallCommandsAsync();

            // Lock main thread to run indefinetly
            await Task.Delay(-1);
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile("config.json.debug")
#else
                .AddJsonFile("config.json")
#endif
                .AddJsonFile("commands.json")
                .Build();
        }
    }
}
