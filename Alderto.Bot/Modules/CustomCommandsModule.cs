using System.Threading.Tasks;
using Alderto.Bot.Lua;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Cc")]
    public class CustomCommandsModule : ModuleBase<SocketCommandContext>
    {
        private readonly ICustomCommandProvider _cmdProvider;

        public CustomCommandsModule(ICustomCommandProvider cmdProvider)
        {
            _cmdProvider = cmdProvider;
        }

        [Command]
        [Summary("Executes a custom command. Set up commands in the WebUI.")]
        public async Task ExecuteAsync(params string[] args)
        {
            // TODO: Create API for Lua code.
            // TODO: Handle LuaCommandNotFoundException
            // ReSharper disable once CoVariantArrayConversion
            await _cmdProvider.RunCommandAsync(Context.Guild.Id, args[0], args);
        }

        [Command("Reload")]
        [Summary("Reloads custom commands into memory. May fix broken commands.")]
        public async Task ReloadAsync()
        {
            await _cmdProvider.ReloadCommands(Context.Guild.Id);
        }
    }
}
