using System;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace Alderto.Services
{
    public class GuildLogger : IGuildLogger
    {
        private readonly DiscordSocketClient _client;

        public GuildLogger(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task LogBankItemChangeAsync(GuildBank bank, GuildBankItem changedItem, ulong adminId, ulong transactorId, string comment)
        {
            // Do not log if there is nowhere to log.
            if (bank.LogChannelId == null)
                return;

            var guild = _client.GetGuild(bank.GuildId);
            var channel = (ISocketMessageChannel)guild.GetChannel((ulong)bank.LogChannelId);

            var admin = guild.GetUser(adminId);
            var transactor = guild.GetUser(transactorId);

            var logMessage = new EmbedBuilder()
                .WithAuthor(transactor)
                .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}");

            logMessage
                .AddField("Bank", bank.Name, true)
                .AddField("Item", changedItem.Name, true);

            logMessage.WithDescription(comment);

            await channel.SendMessageAsync(embed: logMessage.Build()).ConfigureAwait(false);
        }

        public async Task LogBankItemDeleteAsync(GuildBank bank, GuildBankItem deletedItem, ulong adminId)
        {
            // Do not log if there is nowhere to log.
            if (bank.LogChannelId == null)
                return;

            var guild = _client.GetGuild(bank.GuildId);
            var admin = guild.GetUser(adminId);

            var channel = (ISocketMessageChannel)guild.GetChannel((ulong)bank.LogChannelId);

            var logMessage = new EmbedBuilder()
                .WithAuthor(admin)
                .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}");

            logMessage.WithDescription("The following bank item was deleted:");
            logMessage
                .AddField("Name", deletedItem.Name, true)
                .AddField("Description", deletedItem.Description, true)
                .AddField("Quantity", deletedItem.Quantity, true)
                .AddField("Value", deletedItem.Value, true);

            try
            {
                logMessage.WithThumbnailUrl(deletedItem.ImageUrl);
            }
            catch (ArgumentException)
            {
                // URL is not well formed. Ignore error, will not display image as it wont work in the first place.
            }

            await channel.SendMessageAsync(embed: logMessage.Build());
        }

        public async Task LogBankCreateAsync(GuildBank bank, ulong adminId)
        {
            // Do not log if there is nowhere to log.
            if (bank.LogChannelId == null)
                return;

            var guild = _client.GetGuild(bank.GuildId);
            var admin = guild.GetUser(adminId);

            var channel = (ISocketMessageChannel)guild.GetChannel((ulong)bank.LogChannelId);

            var logMessage = new EmbedBuilder()
                .WithAuthor(admin)
                .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}");

            logMessage.WithDescription("The following bank was created:");
            logMessage
                .AddField("Name", $"{bank.Name}", true)
                .AddField("Log Channel", $"<#{bank.LogChannelId}>", true);

            await channel.SendMessageAsync(embed: logMessage.Build());
        }

        public async Task LogBankUpdateAsync(GuildBank oldBank, GuildBank newBank, ulong adminId)
        {
            // Do not log if there is nowhere to log.
            if (newBank.LogChannelId == null && oldBank.LogChannelId == null)
                return;

            var guild = _client.GetGuild(oldBank.GuildId);
            var admin = guild.GetUser(adminId);

            // Special case: Log channel change in old channel and log other changes in the updated channel.
            // Ensure that old bank has a log channel.
            // Ensure that the log channel ids differ.
            if (oldBank.LogChannelId != null && oldBank.LogChannelId != newBank.LogChannelId)
            {
                var c = (ISocketMessageChannel)guild.GetChannel((ulong)oldBank.LogChannelId);
                var comment = newBank.LogChannelId == null
                    ? $"Log channel for bank **{oldBank.Name}** was removed."
                    : $"Log channel for bank **{oldBank.Name}** was changed to <#{newBank.LogChannelId}>.";
                try
                {
                    await c.SendMessageAsync(embed: new EmbedBuilder()
                        .WithAuthor(admin)
                        .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}")
                        .WithDescription(comment).Build());
                }
                catch (HttpException) { /* Ignore error. Bot most likely does not have access to previous channel anymore. No point disallowing log channel change. */ }
            }

            // Ensure that the updated bank has a log channel.
            if (newBank.LogChannelId == null)
                return;

            var channel = (ISocketMessageChannel)guild.GetChannel((ulong)newBank.LogChannelId);

            var logMessage = new EmbedBuilder()
                .WithAuthor(admin)
                .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}");

            // Check every property change
            if (oldBank.Name != newBank.Name)
            {
                logMessage.WithDescription("The following changes were applied:");
                logMessage.AddField("Name", $"{oldBank.Name} -> {newBank.Name}", true);
            }

            await channel.SendMessageAsync(embed: logMessage.Build());
        }

        public async Task LogBankDeleteAsync(GuildBank bank, ulong adminId)
        {
            // Do not log if there is nowhere to log.
            if (bank.LogChannelId == null)
                return;

            var guild = _client.GetGuild(bank.GuildId);
            var admin = guild.GetUser(adminId);

            var channel = (ISocketMessageChannel)guild.GetChannel((ulong)bank.LogChannelId);

            var logMessage = new EmbedBuilder()
                .WithAuthor(admin)
                .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}");

            logMessage.WithDescription("The following bank was deleted:");

            logMessage
                .AddField("Name", $"{bank.Name}", true)
                .AddField("Log Channel", $"<#{bank.LogChannelId}>", true);

            await channel.SendMessageAsync(embed: logMessage.Build());

            // Offload logging of item deletions to another thread.
            await Task.Factory.StartNew(async () =>
            {
                foreach (var item in bank.Contents)
                {
                    await LogBankItemDeleteAsync(bank, item, adminId);
                }
            }).ConfigureAwait(false);
        }
    }
}