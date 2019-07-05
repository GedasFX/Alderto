using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Bot.Extentions;
using Alderto.Bot.Preconditions;
using Alderto.Data;
using Alderto.Data.Extentions;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    public class CurrencyModule : ModuleBase<SocketCommandContext>
    {
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
                return;

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
                return;

            var reply = await ModifyAsyncExec(-qty, users);

            await ReplyAsync(embed: reply);
        }

        public async Task<Embed> ModifyAsyncExec(int qty, IEnumerable<IGuildUser> guildUsers)
        {
            var reply = new EmbedBuilder()
                .AddDefault()
                .WithDescription("**The following changes have been made:**");

            foreach (var user in guildUsers)
            {
                // Get the user
                var dbUser = await _context.GetMemberAsync(guildId: user.GuildId, memberId: user.Id, addIfNonExistant: true);

                // Add currency to the user
                dbUser.CurrencyCount += qty;

                // Format a nice output
                reply.AddField($"{user.Mention} [{user.Username}#{user.Discriminator}]",    
                    $"{dbUser.CurrencyCount - qty} -> {dbUser.CurrencyCount} :gp:");
            }

            await _context.SaveChangesAsync();

            return reply.Build();
        }

        [Command("$")]
        [Summary("Ghecks the amount of points a given user has.")]
        public async Task CheckAsync([Summary("Person to check. If none provided, checks personal points.")] IGuildUser user = null)
        {
            if (user == null)
                user = (IGuildUser)Context.Message.Author;

            var dbUser = await _context.GetMemberAsync(guildId: user.GuildId, memberId: user.Id);

            await ReplyAsync($"```{user.Nickname ?? user.Username} [{user.Username}#{user.Discriminator}] has {dbUser.CurrencyCount} point(s).```");
        }
    }
}
