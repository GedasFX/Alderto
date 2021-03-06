﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Extensions
{
    public static class ModuleBaseExtensions
    {
        /// <summary>
        /// Sends a formatted reply to the source channel.
        /// </summary>
        /// <typeparam name="T">Class <see cref="T"/> implements <see cref="ICommandContext"/></typeparam>
        /// <param name="module">Command module</param>
        /// <param name="description"><see cref="EmbedBuilder.WithDescription"/></param>
        /// <param name="color"><see cref="EmbedColor"/> to be displayed to the right side of the embed. Defaults to <see cref="EmbedColor.Info"/></param>
        /// <param name="author"><see cref="EmbedBuilder.WithAuthor"/></param>
        /// <param name="extra">Additional actions to apply to the builder.</param>
        /// <returns></returns>
        public static async Task ReplyEmbedAsync<T>(this ModuleBase<T> module,
            string? description = null,
            Color color = default,
            IUser? author = null,
            Action<EmbedBuilder>? extra = null) where T : class, ICommandContext
        {
            var embed = new EmbedBuilder().WithDefault(description, color, author ?? module.Context.Message.Author, extra).Build();
            await module.Context.Channel.SendMessageAsync(embed: embed);
        }

        /// <summary>
        /// Sends a formatted reply to the source channel. Specifies <see cref="EmbedColor"/> to be <see cref="EmbedColor.Success"/>
        /// </summary>
        /// <typeparam name="T">Class <see cref="T"/> implements <see cref="ICommandContext"/></typeparam>
        /// <param name="module">Command module</param>
        /// <param name="description"><see cref="EmbedBuilder.WithDescription"/></param>
        /// <param name="extra">Additional actions to apply to the builder.</param>
        /// <returns></returns>
        public static Task ReplySuccessEmbedAsync<T>(this ModuleBase<T> module,
            string? description = null,
            Action<EmbedBuilder>? extra = null) where T : class, ICommandContext
        {
            return ReplyEmbedAsync(module, description, EmbedColor.Success, extra: extra);
        }

        /// <summary>
        /// Sends a formatted reply to the source channel. Specifies <see cref="EmbedColor"/> to be <see cref="EmbedColor.Error"/>
        /// </summary>
        /// <typeparam name="T">Class <see cref="T"/> implements <see cref="ICommandContext"/></typeparam>
        /// <param name="module">Command module</param>
        /// <param name="description"><see cref="EmbedBuilder.WithDescription"/></param>
        /// <param name="extra">Additional actions to apply to the builder.</param>
        /// <returns></returns>
        public static Task ReplyErrorEmbedAsync<T>(this ModuleBase<T> module,
            string? description = null,
            Action<EmbedBuilder>? extra = null) where T : class, ICommandContext
        {
            return ReplyEmbedAsync(module, description, EmbedColor.Error, extra: extra);
        }
    }
}
