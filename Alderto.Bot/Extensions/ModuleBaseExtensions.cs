using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Extensions
{
    public static class ModuleBaseExtensions
    {
        public static async Task ReplyEmbedAsync<T>(this ModuleBase<T> module,
            string description = null,
            Action<EmbedBuilder> extra = null,
            EmbedColor color = EmbedColor.Info) where T : class, ICommandContext
        {
            var embed = new EmbedBuilder().WithDefault(description, extra, color).Build();
            await module.Context.Channel.SendMessageAsync(embed: embed);
        }
    }
}
