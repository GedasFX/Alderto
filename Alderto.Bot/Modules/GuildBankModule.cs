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
            _guildBankManager = guildBankManager;
        }

        //[Command("Give"), Alias("Add")]
        //public async Task Give(IGuildUser transactor, string bankName, string itemName, double quantity)
        //{
        //    if (itemName == "$")
        //    {
        //        // Special case - currency donation.
        //        await _guildBankManager.ModifyCurrencyCountAsync(Context.Guild.Id, bankName, Context.User.Id, transactor.Id, quantity);
        //    }
        //    else
        //    {
        //        await _guildBankManager.ModifyItemCountAsync(Context.Guild.Id, bankName, Context.User.Id, transactor.Id, itemName, quantity);
        //    }
        //}

        //[Command("Take"), Alias("Remove")]
        //public async Task Take(IGuildUser transactor, string bankName, string itemName, double quantity)
        //{
        //    if (itemName == "$")
        //    {
        //        // Special case - currency donation.
        //        await _guildBankManager.ModifyCurrencyCountAsync(Context.Guild.Id, bankName, Context.User.Id, transactor.Id, -quantity);
        //    }
        //    else
        //    {
        //        await _guildBankManager.ModifyItemCountAsync(Context.Guild.Id, bankName, Context.User.Id, transactor.Id, itemName, -quantity);
        //    }
        //}

        [Command("Items"), Alias("List")]
        public async Task Items(string bankName)
        {
            var bank = await _guildBankManager.GetGuildBankAsync(Context.Guild.Id, bankName, b => b
                    .Include(g => g.Contents)
                    .ThenInclude(g => g.GuildBankItem));

            var res = bank.Contents.Aggregate(seed: "", (current, item) => current + $"{item.GuildBankItem.Name} {item.GuildBankItem.Description}\n");
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
                await _itemManager.CreateItemAsync(((IGuildUser)Context.User).GuildId, itemName, itemDescription);
            }

            public async Task Remove(string itemName)
            {
                await _itemManager.RemoveItemAsync(((IGuildUser)Context.User).GuildId, itemName);
            }

            public async Task Rename(string itemName, string newName)
            {
                await _itemManager.UpdateItemAsync(((IGuildUser)Context.User).GuildId, itemName, i => i.Name = newName);
            }

            public async Task Description(string itemName, string newDescription = null)
            {
                if (newDescription != null)
                    await _itemManager.UpdateItemAsync(((IGuildUser)Context.User).GuildId, itemName, item => item.Description = newDescription);
            }
        }
    }
}