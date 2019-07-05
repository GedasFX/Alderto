using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Extentions
{
    public static class ModuleBaseExtentions
    {
        public static async Task ReplyEmbedAsync<T>(this ModuleBase<T> module, string description) where T : class, ICommandContext
        {
            var builder = new EmbedBuilder()
                .WithDescription(description)
                .WithColor(new Color(0x2B9738));
            var embed = builder.Build();
            await module.Context.Channel.SendMessageAsync(text: null, embed: embed)
                .ConfigureAwait(false);
        }
    }
}
