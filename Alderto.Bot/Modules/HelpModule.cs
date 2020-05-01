using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        [Summary("Shows help menu")]
        public async Task Help()
        {
            await this.ReplyEmbedAsync("For command reference visit https://alderto.com/documentation");
        }
    }
}