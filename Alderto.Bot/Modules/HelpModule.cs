using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;

        public HelpModule(CommandService commands)
        {
            _commands = commands;
        }

        [Command]
        [Summary("Shows help menu")]
        public async Task Help()
        {
            await this.ReplyEmbedAsync(extra: builder =>
            {
                foreach (var command in _commands.Commands)
                {
                    builder.AddField(command.Name, command.Summary);
                }
            });
        }
    }
}