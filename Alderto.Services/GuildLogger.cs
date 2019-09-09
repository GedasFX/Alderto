using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Discord;
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

        public async Task LogBankCreateAsync(ulong guildId, ulong adminId, GuildBank newBank)
        {
            // Do not log if there is nowhere to log.
            if (newBank.LogChannelId == null)
                return;

            var guild = _client.GetGuild(guildId);
            var admin = guild.GetUser(adminId);

            var channel = (ISocketMessageChannel)guild.GetChannel((ulong)newBank.LogChannelId);

            var logMessage = new EmbedBuilder()
                .WithAuthor(admin)
                .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}");

            logMessage.AddField("Name", $"{newBank.Name}")
                .AddField("Log Channel", $"<#{newBank.LogChannelId}>");

            logMessage
                .WithDescription("The following bank was created:");

            await channel.SendMessageAsync(embed: logMessage.Build()).ConfigureAwait(false);
        }

        public async Task LogBankUpdateAsync(ulong guildId, ulong adminId, GuildBank oldBank, GuildBank newBank)
        {
            // Do not log if there is nowhere to log.
            if (newBank.LogChannelId == null && oldBank.LogChannelId == null)
                return;

            var guild = _client.GetGuild(guildId);
            var admin = guild.GetUser(adminId);

            // Special case: Log channel change in old channel and log other changes in the updated channel.
            // Ensure that old bank has a log channel.
            // Ensure that the log channel ids differ.
            if (oldBank.LogChannelId != null && oldBank.LogChannelId != newBank.LogChannelId)
            {
                var c = (ISocketMessageChannel)guild.GetChannel((ulong)oldBank.LogChannelId);
                var comment = newBank.LogChannelId == null
                    ? $"Log channel for bank {oldBank.Name} was removed."
                    : $"Log channel for bank {oldBank.Name} was changed to <#{newBank.LogChannelId}>.";
                await c.SendMessageAsync(embed: new EmbedBuilder()
                    .WithAuthor(admin)
                    .WithFooter($"Req. by {admin.Username}#{admin.Discriminator}")
                    .WithDescription(comment).Build()).ConfigureAwait(false);
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
                logMessage.AddField("Name", $"{oldBank.Name} -> {newBank.Name}");

            logMessage
                .WithDescription("The following changes were applied:");

            await channel.SendMessageAsync(embed: logMessage.Build()).ConfigureAwait(false);
        }


        //public async Task LogAsync(int bankId, ulong adminId, ulong transactorId, double amountDelta, int itemId = 0, string comment = null,
        //    bool saveChanges = false)
        //{
        //    var message = new GuildBankTransaction
        //    {
        //        BankId = bankId,
        //        AdminId = adminId,
        //        MemberId = transactorId,
        //        Amount = amountDelta,
        //        ItemId = itemId != 0 ? (int?)itemId : null,
        //        Comment = comment,
        //        TransactionDate = DateTimeOffset.UtcNow
        //    };
    }
}