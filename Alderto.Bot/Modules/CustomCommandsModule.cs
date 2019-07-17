using System.Threading.Tasks;
using Alderto.Bot.Services;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Cc")]
    public class CustomCommandsModule : ModuleBase<SocketCommandContext>
    {
        private readonly ICustomCommandProviderService _cmdProvider;

        public CustomCommandsModule(ICustomCommandProviderService cmdProvider)
        {
            _cmdProvider = cmdProvider;
        }

        [Command]
        public async Task ExecuteAsync(params string[] args)
        {
            // TODO: Create API for Lua code.
            // TODO: Handle LuaCommandNotFoundException
            await _cmdProvider.RunCommandAsync(Context.Guild.Id, args[0], args);
        }

        [Command("Reload")]
        public async Task ReloadAsync()
        {
            await _cmdProvider.ReloadCommands(Context.Guild.Id);
        }
    }
}
