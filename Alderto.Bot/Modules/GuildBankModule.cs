using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Modules
{
    [Group, Alias("GuildBank", "GB")]
    public class GuildBankModule : ModuleBase<SocketCommandContext>
    {

        private readonly IGuildBankManager _guildBankManager;

        public GuildBankModule(IGuildBankManager guildBankManager)
        {
            _guildBankManager = guildBankManager.Configure(Context.Guild.Id, Context.User.Id);
        }

        [Command("Transactions"), Alias("Log")]
        public async Task Transactions(IGuildUser user = null, string bankName = null)
        {
            if (user == null)
                user = (IGuildUser)Context.User;

            var gbTrans = bankName != null
                ? _guildBankManager.GetAllTransactions(user.Id, gb => gb.Where(b => b.Name == bankName))
                : _guildBankManager.GetAllTransactions(user.Id);
            var transactions = gbTrans as ICollection<GuildBankTransaction> ?? gbTrans.ToArray();

            if (transactions.Count == 0)
                await this.ReplyErrorEmbedAsync($"{user.Mention} has not made any transactions.");
            else
                await this.ReplySuccessEmbedAsync($"{user.Mention} has made the following transactions:", builder =>
                {
                    foreach (var transaction in transactions)
                    {
                        builder.AddField($"{transaction.Id}: {transaction.TransactionDate}", $"**{transaction.Comment}**");
                    }
                });
        }

        [Command("Give"), Alias("Add")]
        public async Task Give(IGuildUser transactor, string bankName, string itemName, double quantity)
        {
            if (itemName == "$")
            {
                // Special case - currency donation.
                await _guildBankManager.ModifyCurrencyCountAsync(bankName, transactor.Id, quantity);
            }
            else
            {
                await _guildBankManager.ModifyItemCountAsync(bankName, transactor.Id, itemName, quantity);
            }
        }

        [Command("Take"), Alias("Remove")]
        public async Task Take(IGuildUser transactor, string bankName, string itemName, double quantity)
        {
            if (itemName == "$")
            {
                // Special case - currency donation.
                await _guildBankManager.ModifyCurrencyCountAsync(bankName, transactor.Id, -quantity);
            }
            else
            {
                await _guildBankManager.ModifyItemCountAsync(bankName, transactor.Id, itemName, -quantity);
            }
        }

        [Command("Items"), Alias("List")]
        public async Task Items(string bankName)
        {
            var bank = await _guildBankManager.GetGuildBankAsync(bankName, b => b
                    .Include(g => g.GuildBankContents)
                    .ThenInclude(g => g.GuildBankItem));

            var res = bank.GuildBankContents.Aggregate(seed: "", (current, item) => current + $"{item.GuildBankItem.Name} {item.GuildBankItem.Description}\n");
            await this.ReplySuccessEmbedAsync(res);
        }

        [Group("Items")]
        public class GuildItemsModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildBankItemManager _itemManager;

            public GuildItemsModule(IGuildBankItemManager itemManager)
            {
                _itemManager = itemManager;
            }

            public async Task Add(string itemName, string itemDescription = null)
            {
                await _itemManager.CreateAsync(((IGuildUser)Context.User).GuildId, new GuildBankItem { Name = itemName, Description = itemDescription });
            }

            public async Task Remove(string itemName)
            {
                await _itemManager.RemoveAsync(((IGuildUser)Context.User).GuildId, itemName);
            }

            public async Task Rename(string itemName, string newName)
            {
                await _itemManager.UpdateAsync(((IGuildUser)Context.User).GuildId, itemName, i => i.Name = newName);
            }

            public async Task Description(string itemName, string newDescription = null)
            {
                if (newDescription != null)
                    await _itemManager.UpdateAsync(((IGuildUser)Context.User).GuildId, itemName, item => item.Description = newDescription);
            }
        }
    }
}