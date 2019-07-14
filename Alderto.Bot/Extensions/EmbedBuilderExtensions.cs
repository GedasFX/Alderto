using System;
using Discord;

namespace Alderto.Bot.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder WithDefault(this EmbedBuilder builder,
            string description = null,
            Action<EmbedBuilder> extra = null,
            EmbedColor embedColor = EmbedColor.Info)
        {
            builder.WithColor(new Color((uint)embedColor));
            if (description != null)
                builder.WithDescription(description);

            // Apply additional changes.
            extra?.Invoke(builder);

            return builder;
        }
    }

    public enum EmbedColor
    {
        Info = 0x5b0de, Success = 0x5cb85c, Warning = 0xf0ad4e, Error = 0xd9534f
    }
}
