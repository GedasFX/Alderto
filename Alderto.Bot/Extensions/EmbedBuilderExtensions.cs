using System;
using Discord;

namespace Alderto.Bot.Extensions
{
    public static class EmbedBuilderExtensions
    {
        /// <summary>
        /// Adds frequently used elements <see cref="EmbedBuilder.WithDescription"/>, <see cref="EmbedBuilder.WithColor"/> to the embed builder.
        /// </summary>
        /// <param name="builder">Builder to configure.</param>
        /// <param name="description"><see cref="EmbedBuilder.WithDescription"/></param>
        /// <param name="embedColor"><see cref="Color"/> to be displayed to the right side of the embed.
        /// Use class <see cref="EmbedColor"/> properties, or a custom color. Defaults to <see cref="EmbedColor.Info"/></param>
        /// <param name="author"><see cref="EmbedBuilder.WithAuthor"/></param>
        /// <param name="extra">Additional actions to apply to the builder.</param>
        /// <returns><see cref="builder"/></returns>
        public static EmbedBuilder WithDefault(this EmbedBuilder builder,
            string description = null,
            Color embedColor = default,
            IUser author = null,
            Action<EmbedBuilder> extra = null)
        {
            builder.WithColor(embedColor == default ? EmbedColor.Info : embedColor);

            if (description != null)
                builder.WithDescription(description);
            
            if (author != null)
                builder.WithFooter($"Req. by {author.Username}#{author.Discriminator}");

            // Apply additional changes.
            extra?.Invoke(builder);

            return builder;
        }
    }

    /// <summary>
    /// Commonly used colors for embeds.
    /// </summary>
    public class EmbedColor
    {
        /// <summary>
        /// Raw value: 0x5b0de
        /// </summary>
        public static Color Info { get; } = new Color(0x5b0de);

        /// <summary>
        /// Raw value: 0x5cb85c
        /// </summary>
        public static Color Success { get; } = new Color(0x5cb85c);

        /// <summary>
        /// Raw value: 0xf0ad4e
        /// </summary>
        public static Color Warning { get; } = new Color(0xf0ad4e);

        /// <summary>
        /// Raw value: 0xd9534f
        /// </summary>
        public static Color Error { get; } = new Color(0xd9534f);
    }
}
