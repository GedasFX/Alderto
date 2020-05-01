using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
using Alderto.Services;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    public class CurrencyModule : ModuleBase<SocketCommandContext>
    {
        private readonly IGuildMemberManager _guildMemberManager;
        private readonly IGuildPreferencesProvider _guildPreferences;
        private readonly ICurrencyManager _currencyManager;

        public CurrencyModule(
            IGuildMemberManager guildMemberManager,
            IGuildPreferencesProvider guildPreferences,
            ICurrencyManager currencyManager)
        {
            _guildMemberManager = guildMemberManager;
            _guildPreferences = guildPreferences;
            _currencyManager = currencyManager;
        }

        [Command("Give")]
        [Summary("Gives an amount of points to the listed users")]
        [RequireRole("Admin")]
        public async Task GiveAsync(
            [Summary("Amount of points to give")] int qty,
            [Summary("Users to give points to")] params IGuildUser[] users)
        {
            // This is for giving, not taking
            if (qty <= 0)
            {
                await this.ReplyErrorEmbedAsync("Quantity must be greater than 0.");
                return;
            }

            if (users.Length == 0)
            {
                await this.ReplyErrorEmbedAsync("At least one user must be specified.");
                return;
            }

            var reply = await ModifyAsyncExec(qty, users);

            await ReplyAsync(embed: reply);
        }

        [Command("Take")]
        [Summary("Takes an amount of points from the listed users")]
        [RequireRole("Admin")]
        public async Task TakeAsync(
            [Summary("Amount of points to take")] int qty,
            [Summary("Users to take points from")] params IGuildUser[] users)
        {
            // This is for taking, not giving
            if (qty <= 0)
            {
                await this.ReplyErrorEmbedAsync("Quantity must be greater than 0.");
                return;
            }

            if (users.Length == 0)
            {
                await this.ReplyErrorEmbedAsync("At least one user must be specified.");
                return;
            }

            var reply = await ModifyAsyncExec(-qty, users);

            await ReplyAsync(embed: reply);
        }

        private async Task<Embed> ModifyAsyncExec(int qty, IEnumerable<IGuildUser> guildUsers)
        {
            var author = (IGuildUser)Context.Message.Author;
            var currencySymbol = (await _guildPreferences.GetPreferencesAsync(author.GuildId)).CurrencySymbol;
            var reply = new EmbedBuilder()
                .WithDefault(embedColor: EmbedColor.Success, author: author);

            var no = 1;
            foreach (var user in guildUsers)
            {
                var member = (await _guildMemberManager.GetGuildMemberAsync(user))!;
                await _currencyManager.ModifyPointsAsync(member, qty);

                // Format a nice output.
                reply.AddField($"No. {no++}:", $"{user.Mention}: {member.CurrencyCount - qty} -> {member.CurrencyCount} {currencySymbol}");
            }

            return reply.Build();
        }

        [Command("$")]
        [Summary("Checks the amount of points a given user has.")]
        public async Task CheckAsync(
            [Summary("Person to check. If no user was provided, checks personal points.")] IGuildUser? user = null)
        {
            if (user == null)
                user = (IGuildUser)Context.Message.Author;

            var currencySymbol = (await _guildPreferences.GetPreferencesAsync(user.GuildId)).CurrencySymbol;
            var dbUser = (await _guildMemberManager.GetGuildMemberAsync(user))!;

            await this.ReplyEmbedAsync($"{user.Mention} has {dbUser.CurrencyCount} {currencySymbol}");
        }

        [Command("Timely"), Alias("Moon")]
        [Summary("Grants a timely currency reward.")]
        public async Task Timely()
        {
            var user = (IGuildUser)Context.User;
            var dbUser = (await _guildMemberManager.GetGuildMemberAsync(user.GuildId, user.Id))!;

            var preferences = await _guildPreferences.GetPreferencesAsync(user.GuildId);
            var timelyCooldown = preferences.TimelyCooldown;
            var currencySymbol = preferences.CurrencySymbol;
            var timelyAmount = preferences.TimelyRewardQuantity;

            var timeRemaining = await _currencyManager.GrantTimelyRewardAsync(dbUser, timelyAmount, timelyCooldown);

            // If null - points were given out. Otherwise its time remaining until next claim.
            if (timeRemaining != null)
            {
                await this.ReplyErrorEmbedAsync($"{user.Mention} will be able to claim more {currencySymbol} in **{timeRemaining}**.");
                return;
            }

            // Points were given out.
            await this.ReplySuccessEmbedAsync(($"{user.Mention} was given {timelyAmount} {currencySymbol}. New total: **{dbUser.CurrencyCount}**."));
        }

        [Command("top")]
        public async Task Top(int page = 1)
        {
            if (page < 1)
            {
                await this.ReplyErrorEmbedAsync("Selected page must be positive.");
                return;
            }

            var topN = await _currencyManager.GetRichestUsersAsync(Context.Guild.Id, 50, (page - 1) * 50);

            var res = topN.Aggregate(new StringBuilder(), (c, n) => c.Append($"<@{n.MemberId}>: {n.CurrencyCount}\n"));
            await this.ReplyEmbedAsync(res.ToString());
        }
    }
}
