using Discord;

namespace Alderto.Bot.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder WithDefault(this EmbedBuilder builder) => builder
            .WithColor(new Color(0x2B9738));
    }
}
