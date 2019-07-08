using Discord;

namespace Alderto.Bot.Extentions
{
    public static class EmbedBuilderExtentions
    {
        public static EmbedBuilder WithDefault(this EmbedBuilder builder) => builder
            .WithColor(new Color(0x2B9738));
    }
}
