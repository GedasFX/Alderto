using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Extensions
{
    public static class ModuleBaseExtensions
    {
        public static async Task ReplyEmbedAsync<T>(this ModuleBase<T> module, string description) where T : class, ICommandContext
        {
            var builder = new EmbedBuilder()
                .WithDefault()
                .WithDescription(description);
            var embed = builder.Build();
            await module.Context.Channel.SendMessageAsync(text: null, embed: embed)
                .ConfigureAwait(false);
        }
    }
}
