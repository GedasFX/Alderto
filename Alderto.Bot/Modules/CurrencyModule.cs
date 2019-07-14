using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
using Alderto.Data;
using Alderto.Data.Extensions;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    public class CurrencyModule : ModuleBase<SocketCommandContext>
    {
        private const string CurrencySymbol = ":Bucketpin:";
        private const int TimelyAmount = 3;
        private const int MinTimeElapsedMs = 1000 * 60 * 60 * 6;
        private const bool AllowNegativePoints = true;

        private readonly IAldertoDbContext _context;

        public CurrencyModule(IAldertoDbContext context)
        {
            _context = context;
        }

        [Command("Give")]
        [Summary("Gives an amount of points to the listed users")]
        [RequireRole("Admin")]
        public async Task GiveAsync([Summary("Amount of points to give")] int qty,
            [Summary("Users to give points to")] params IGuildUser[] users)
        {
            // This is for giving, not taking
            if (qty <= 0)
            {
                await this.ReplyEmbedAsync(description: "No changes made: Given quantity must be > 0.", color: EmbedColor.Error);
                return;
            }

            if (users.Length == 0)
            {
                await this.ReplyEmbedAsync(description: "No changes made: At least one user must be specified.", color: EmbedColor.Error);
                return;
            }

            var reply = await ModifyAsyncExec(qty, users);

            await ReplyAsync(embed: reply);
        }

        [Command("Take")]
        [Summary("Takes an amount of points from the listed users")]
        [RequireRole("Admin")]
        public async Task TakeAsync([Summary("Amount of points to take")] int qty,
            [Summary("Users to take points from")] params IGuildUser[] users)
        {
            // This is for taking, not giving
            if (qty <= 0)
            {
                await this.ReplyEmbedAsync(description: "No changes made: Given quantity must be > 0.", color: EmbedColor.Error);
                return;
            }

            if (users.Length == 0)
            {
                await this.ReplyEmbedAsync(description: "No changes made: At least one user must be specified.", color: EmbedColor.Error);
                return;
            }

            var reply = await ModifyAsyncExec(-qty, users);
            await ReplyAsync(embed: reply);
        }

        public async Task<Embed> ModifyAsyncExec(int qty, IEnumerable<IGuildUser> guildUsers)
        {
            var reply = new EmbedBuilder()
                .WithDefault(description: "**The following changes have been made:**", embedColor: EmbedColor.Success);

            foreach (var user in guildUsers)
            {
                // Get the user
                var dbUser = await _context.GetGuildMemberAsync(user.GuildId, user.Id);
                var oldCurrencyCount = dbUser.CurrencyCount;

                if (qty > 0 && oldCurrencyCount > 0 && oldCurrencyCount + qty < 0)
                {
                    // overflow, set to max value instead.
                    dbUser.CurrencyCount = int.MaxValue;
                }
                else if (qty < 0 && oldCurrencyCount < 0 && oldCurrencyCount + qty > 0)
                {
                    //underflow, set to min value instead
                    dbUser.CurrencyCount = int.MinValue;
                }
                else
                {
                    // Add currency to the user
                    dbUser.CurrencyCount += qty;
                }

                if (!AllowNegativePoints && dbUser.CurrencyCount < 0)
                {
                    dbUser.CurrencyCount = 0;
                }

                // Format a nice output.
                reply.AddField($"{oldCurrencyCount} -> {dbUser.CurrencyCount} {CurrencySymbol}", $"{user.Mention}");
            }

            await _context.SaveChangesAsync();

            return reply.Build();
        }

        [Command("$")]
        [Summary("Checks the amount of points a given user has.")]
        public async Task CheckAsync([Summary("Person to check. If none provided, checks personal points.")] IGuildUser user = null)
        {
            if (user == null)
                user = (IGuildUser)Context.Message.Author;

            var dbUser = await _context.GetGuildMemberAsync(user.GuildId, user.Id);

            await this.ReplyEmbedAsync($"{user.Mention} has " +
                $"{dbUser.CurrencyCount} {CurrencySymbol}{(dbUser.CurrencyCount == 1 || dbUser.CurrencyCount == -1 ? "" : "s")}.");
        }

        [Command("Timely"), Alias("Tub", "ClaimTub")]
        public async Task Timely()
        {
            var user = (IGuildUser)Context.User;
            var dbUser = await _context.GetGuildMemberAsync(user.GuildId, user.Id);

            var timeRemaining = dbUser.CurrencyLastClaimed.AddMilliseconds(MinTimeElapsedMs) - DateTimeOffset.Now;


            if (timeRemaining.Ticks > 0)
            {
                // Deny points as time delay hasn't ran out.
                await this.ReplyEmbedAsync($"You will be able to claim more {CurrencySymbol}s in **{timeRemaining}**.", color: EmbedColor.Error);
                return;
            }

            // Give out points.
            dbUser.CurrencyLastClaimed = DateTimeOffset.Now;
            dbUser.CurrencyCount += TimelyAmount;
            await _context.SaveChangesAsync();

            await this.ReplyEmbedAsync($"{user.Mention} was given {CurrencySymbol}{(TimelyAmount == 1 || TimelyAmount == -1 ? "" : "s")} {CurrencySymbol}. " +
                    $"New total: **{dbUser.CurrencyCount}**.", color: EmbedColor.Success);
        }
    }
}
