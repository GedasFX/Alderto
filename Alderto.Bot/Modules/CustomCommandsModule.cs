using System.Threading.Tasks;
using Alderto.Bot.Services;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Cc")]
    public class CustomCommandsModule : ModuleBase<SocketCommandContext>
    {
        private readonly CustomCommandsProviderService _cmdProvider;

        public CustomCommandsModule(CustomCommandsProviderService cmdProvider)
        {
            _cmdProvider = cmdProvider;
        }

        [Command]
        public async Task ExecuteAsync([Remainder] string args)
        {
            await _cmdProvider.RunCommandAsync(Context.Guild.Id, args.Split(' ')[0], args);
        }

        [Command("Reload")]
        public async Task ReloadAsync()
        {
            await _cmdProvider.ReloadCommands(Context.Guild.Id);
        }
    }
}
