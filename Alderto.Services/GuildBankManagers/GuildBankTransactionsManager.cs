using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Discord;
using Discord.WebSocket;

namespace Alderto.Services.GuildBankManagers
{
    public class GuildBankTransactionsManager : IGuildBankTransactionsManager
    {
        private readonly DiscordSocketClient _client;

        public GuildBankTransactionsManager(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task LogCurrencyChangeAsync(GuildBank bank, ulong adminId, ulong transactorId,
            double prevAmount, double newAmount, string comment = null)
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
                .AddField("Bank Name", bank.Name)
                .AddField("Changes", $"Money: {prevAmount} -> {newAmount}");
            
            if (comment != null)
                logMessage.AddField(name: "Comment", comment);

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