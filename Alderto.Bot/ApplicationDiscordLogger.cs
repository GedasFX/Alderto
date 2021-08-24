using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank.Events;
using Alderto.Domain.Services;
using Discord;
using MediatR;

namespace Alderto.Bot
{
    public class ApplicationDiscordLogger :
        INotificationHandler<BankCreatedEvent>,
        INotificationHandler<BankUpdatedEvent>,
        INotificationHandler<BankDeletedEvent>,
        INotificationHandler<BankItemCreatedEvent>,
        INotificationHandler<BankItemUpdatedEvent>,
        INotificationHandler<BankItemDeletedEvent>
    {
        private readonly IDiscordClient _client;
        private readonly IGuildSetupService _guildSetupService;

        public ApplicationDiscordLogger(IDiscordClient client, IGuildSetupService guildSetupService)
        {
            _client = client;
            _guildSetupService = guildSetupService;
        }

        public Task Handle(BankCreatedEvent notification, CancellationToken cancellationToken)
        {
            return SendMessage(notification.Request.GuildId, notification.Request.MemberId,
                $"Created bank **{notification.Bank.Name}**");
        }

        public Task Handle(BankUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return SendMessage(notification.Request.GuildId, notification.Request.MemberId,
                $"Renamed a bank to **{notification.Bank.Name}**");
        }

        public Task Handle(BankDeletedEvent notification, CancellationToken cancellationToken)
        {
            return SendMessage(notification.Request.GuildId, notification.Request.MemberId,
                $"Deleted bank **{notification.Bank.Name}** and all of its contents");
        }


        public Task Handle(BankItemCreatedEvent notification, CancellationToken cancellationToken)
        {
            return SendMessage(notification.Request.GuildId, notification.Request.MemberId,
                $"Added item **{notification.BankItem.Name}** to bank **{notification.BankItem.GuildBank?.Name ?? notification.BankItem.Id.ToString()}**",
                notification.BankItem,
                new[]
                {
                    nameof(notification.BankItem.Name), nameof(notification.BankItem.Description),
                    nameof(notification.BankItem.Value), nameof(notification.BankItem.Quantity)
                },
                embed =>
                {
                    if (notification.BankItem.ImageUrl != null)
                        embed.WithThumbnailUrl(notification.BankItem.ImageUrl);
                });
        }

        public Task Handle(BankItemUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return SendMessage(notification.Request.GuildId, notification.Request.MemberId,
                $"Updated item **{notification.BankItem.Name}** in bank **{notification.BankItem.GuildBank?.Name ?? notification.BankItem.Id.ToString()}**",
                notification.BankItem,
                new[]
                {
                    nameof(notification.BankItem.Name), nameof(notification.BankItem.Description),
                    nameof(notification.BankItem.Value), nameof(notification.BankItem.Quantity)
                },
                embed =>
                {
                    if (notification.BankItem.ImageUrl != null)
                        embed.WithThumbnailUrl(notification.BankItem.ImageUrl);
                });
        }

        public Task Handle(BankItemDeletedEvent notification, CancellationToken cancellationToken)
        {
            return SendMessage(notification.Request.GuildId, notification.Request.MemberId,
                $"Removed item **{notification.BankItem.Name}** from bank **{notification.BankItem.GuildBank?.Name ?? notification.BankItem.Id.ToString()}**",
                notification.BankItem,
                new[]
                {
                    nameof(notification.BankItem.Name), nameof(notification.BankItem.Description),
                    nameof(notification.BankItem.Value), nameof(notification.BankItem.Quantity)
                },
                embed =>
                {
                    if (notification.BankItem.ImageUrl != null)
                        embed.WithThumbnailUrl(notification.BankItem.ImageUrl);
                });
        }

        private async Task SendMessage(ulong guildId, ulong userId, string message, object? entity = null,
            IEnumerable<string>? properties = null, Action<EmbedBuilder>? embedOptions = null)
        {
            var setup = await _guildSetupService.GetGuildSetupAsync(guildId);
            var logChannelId = setup.Configuration.LogChannelId;

            if (logChannelId == null)
                return;

            if (await _client.GetChannelAsync((ulong) logChannelId) is not IMessageChannel channel)
                return;

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(await _client.GetUserAsync(userId))
                .WithDescription(message)
                .WithCurrentTimestamp();

            if (entity != null && properties != null)
            {
                var type = entity.GetType();
                foreach (var propertyName in properties)
                {
                    var property = type.GetProperty(propertyName);
                    if (property == null)
                        continue;

                    var value = property.GetValue(entity)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                        embedBuilder.AddField(propertyName, value);
                }
            }

            embedOptions?.Invoke(embedBuilder);

            // Do not await.
            _ = channel.SendMessageAsync(embed: embedBuilder.Build());
        }
    }
}
