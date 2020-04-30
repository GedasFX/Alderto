using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Services;
using Alderto.Services.Exceptions;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Modules
{
    [Group, Alias("GuildBank", "GB", "GuildBanks", "Banks", "Bank")]
    public class GuildBankModule : ModuleBase<SocketCommandContext>
    {
        private readonly IGuildBankManager _guildBankManager;

        public GuildBankModule(IGuildBankManager guildBankManager)
        {
            _guildBankManager = guildBankManager;
        }

        [Command("list")]
        public async Task List()
        {
            var banks = await _guildBankManager.GetGuildBanksAsync(Context.Guild.Id);
            await this.ReplyEmbedAsync(banks.Aggregate(new StringBuilder(), (c, i) => c.Append($"**{i.Name,32}**\n")).ToString());
        }

        [Command("items")]
        public async Task Items(string bankName)
        {
            var bank = await _guildBankManager.GetGuildBankAsync(Context.Guild.Id, bankName,
                b => b.Include(g => g.Contents));
            if (bank == null)
                throw new BankNotFoundException();

            var res = bank.Contents.Aggregate(new StringBuilder(), (current, item) =>
                current.Append($"**{item.Name}**\n{(string.IsNullOrEmpty(item.Description) ? "N/A" : item.Description)}\n*qty*: {item.Quantity} @ {item.Value} ea.\n\n"));
            await this.ReplyEmbedAsync(res.ToString());
        }
    }
}