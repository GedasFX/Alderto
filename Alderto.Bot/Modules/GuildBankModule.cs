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
            var bank = await _guildBankManager.GetGuildBankAsync(Context.Guild.Id, bankName,
                b => b.Include(g => g.Contents));

            var res = bank.Contents.Aggregate(seed: "", (current, item) => current + $"{item.Name} {item.Description}\n");
            await this.ReplySuccessEmbedAsync(res);
        }
    }
}