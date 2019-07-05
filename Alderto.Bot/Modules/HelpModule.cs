using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Bot.Extentions;
using Alderto.Bot.Preconditions;
using Alderto.Data;
using Alderto.Data.Extentions;
using Discord;
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
            var embed = new EmbedBuilder()
                .AddDefault();
            foreach (var command in _commands.Commands)
            {
                embed.AddField(command.Name, command.Summary);
            }
            await ReplyAsync(embed: embed.Build());
        }

        [Group("Currency")]
        public class Currency
        {

        }
    }
}