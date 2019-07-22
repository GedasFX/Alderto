using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
using Alderto.Bot.Services;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    public class CurrencyModule : ModuleBase<SocketCommandContext>
    {
        private readonly IGuildUserManager _guildUserManager;
        private readonly IGuildPreferencesManager _guildPreferences;
        private readonly ICurrencyManager _currencyManager;

        public CurrencyModule(IGuildUserManager guildUserManager, IGuildPreferencesManager guildPreferences, ICurrencyManager currencyManager)
        {
            _guildUserManager = guildUserManager;
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
                var member = await _guildUserManager.GetGuildMemberAsync(user);
                await _currencyManager.ModifyPointsAsync(member, qty);

                // Format a nice output.
                reply.AddField($"No. {no++}:", $"{user.Mention}: {member.CurrencyCount - qty} -> {member.CurrencyCount} {currencySymbol}");
            }

            return reply.Build();
        }

        [Command("$")]
        [Summary("Checks the amount of points a given user has.")]
        public async Task CheckAsync(
            [Summary("Person to check. If no user was provided, checks personal points.")] IGuildUser user = null)
        {
            if (user == null)
                user = (IGuildUser)Context.Message.Author;

            var currencySymbol = (await _guildPreferences.GetPreferencesAsync(user.GuildId)).CurrencySymbol;
            var dbUser = await _guildUserManager.GetGuildMemberAsync(user);

            await this.ReplyEmbedAsync($"{user.Mention} has {dbUser.CurrencyCount} {currencySymbol}");
        }

        [Command("Timely"), Alias("Tub", "ClaimTub")]
        [Summary("Grants a timely currency reward.")]
        public async Task Timely()
        {
            var user = (IGuildUser)Context.User;
            var dbUser = await _guildUserManager.GetGuildMemberAsync(user.GuildId, user.Id);

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
    }
}
