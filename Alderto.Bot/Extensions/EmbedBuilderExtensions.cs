using Discord;

namespace Alderto.Bot.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder WithDefault(this EmbedBuilder builder, string description = null)
        {
            builder.WithColor(new Color(0x2B9738));
            if (description != null)
                builder.WithDescription(description);
            return builder;
        }
    }
}
