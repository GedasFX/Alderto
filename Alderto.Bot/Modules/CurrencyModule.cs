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
        private const string currencySymbol = ":Bucketpin:";
        private const int timelyAmount = 3;
        private const int minTimeElapsedMS = 1000 * 60 * 60 * 6;
        private const bool allowNegativePoints = true;

        private readonly IAldertoDbContext _context;

        public CurrencyModule(IAldertoDbContext context)
        {
            _context = context;
        }

        private Task<IUserMessage> ReplyAsyncSimpleEmbed(string message)
        {
            return ReplyAsync(embed: new EmbedBuilder().WithDefault(message).Build());
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
                await ReplyAsyncSimpleEmbed("No changes made: Given quantity must by > 0");
                return;
            }

            if (users.Length == 0)
            {
                await ReplyAsyncSimpleEmbed("No changes made: At least one user must be specified.");
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
                await ReplyAsyncSimpleEmbed("No changes made: Given quantity must by > 0");
                return;
            }

            if (users.Length == 0)
            {
                await ReplyAsyncSimpleEmbed("No changes made: At least one user must be specified.");
                return;
            }

            var reply = await ModifyAsyncExec(-qty, users);
            await ReplyAsync(embed: reply);
        }

        public async Task<Embed> ModifyAsyncExec(int qty, IEnumerable<IGuildUser> guildUsers)
        {
            var reply = new EmbedBuilder()
                .WithDefault()
                .WithDescription("**The following changes have been made:**");

            foreach (var user in guildUsers)
            {
                // Get the user
                var dbUser = await _context.GetGuildMemberAsync(user.GuildId, user.Id, addIfNonExistent: true);
                var oldCurrencyCount = dbUser.CurrencyCount;

                if (qty > 0 && oldCurrencyCount > 0 && oldCurrencyCount + qty < 0)
                {
                    // overflow, set to max value instead.
                    dbUser.CurrencyCount = Int32.MaxValue;
                }
                else if (qty < 0 && oldCurrencyCount < 0 && oldCurrencyCount + qty > 0)
                {
                    //underflow, set to min value instead
                    dbUser.CurrencyCount = Int32.MinValue;
                }
                else
                {
                    // Add currency to the user
                    dbUser.CurrencyCount += qty;
                }

                if (!allowNegativePoints && dbUser.CurrencyCount < 0)
                {
                    dbUser.CurrencyCount = 0;
                }

                // Format a nice output
                reply.AddField($"{user.Mention} [{user.Username}#{user.Discriminator}]",
                    $"{oldCurrencyCount} -> {dbUser.CurrencyCount} {currencySymbol}");
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

            await ReplyAsyncSimpleEmbed($"```{user.Nickname ?? user.Username} [{user.Username}#{user.Discriminator}] has " +
                $"{dbUser.CurrencyCount} {currencySymbol}{(dbUser.CurrencyCount == 1 || dbUser.CurrencyCount == -1 ? "" : "s")}.```");
        }

        [Command("Timely"), Alias("Tub", "ClaimTub")]
        public async Task Timely()
        {
            var user = (IGuildUser)Context.User;
            var dbUser = await _context.GetGuildMemberAsync(user.GuildId, user.Id, addIfNonExistent: true);

            var timeRemaining = dbUser.CurrencyLastClaimed.AddMilliseconds(minTimeElapsedMS) - DateTimeOffset.Now;


            if (timeRemaining.Ticks > 0)
            {
                // Deny points as time delay hasn't ran out.
                await ReplyAsyncSimpleEmbed($"You will be able to claim more {currencySymbol}s in **{timeRemaining}**.");
                return;
            }

            // Give out points.
            dbUser.CurrencyLastClaimed = DateTimeOffset.Now;
            dbUser.CurrencyCount += timelyAmount;
            await _context.SaveChangesAsync();

            await ReplyAsyncSimpleEmbed($"{user.Mention} was given {currencySymbol}{(timelyAmount == 1 || timelyAmount == -1 ? "" : "s")} {currencySymbol}. " +
                    $"New total: **{dbUser.CurrencyCount}**.");
        }
    }
}
