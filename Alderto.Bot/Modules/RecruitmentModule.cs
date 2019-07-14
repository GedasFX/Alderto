using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Data;
using Alderto.Data.Extensions;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Modules
{
    [Group("Recruitment"), Alias("Recruit", "Recruited", "Rec")]
    public class RecruitmentModule : ModuleBase<SocketCommandContext>
    {
        private readonly IAldertoDbContext _context;

        public RecruitmentModule(IAldertoDbContext context)
        {
            _context = context;
        }

        [Command("Recruited"), Alias("Add")]
        public async Task Recruited(IGuildUser recruiter, params IGuildUser[] recruited)
        {
            var recruiterId = recruiter.Id;
            foreach (var member in recruited)
            {
                var dbUser = await _context.GetGuildMemberAsync(recruiter.GuildId, member.Id, addIfNonExistent: true);
                dbUser.RecruiterMemberId = recruiterId;

                // Ensure that joinedAt is registered. Is used for listing user recruits.
                dbUser.JoinedAt = member.JoinedAt;
            }

            await _context.SaveChangesAsync();

            await ReplyAsync(embed: new EmbedBuilder().WithDefault().WithDescription($"Successfully registered {recruited.Length} user(s) as recruits of {recruiter.GetFullName()}").Build());
        }

        [Command("List")]
        public async Task ListAsync(IGuildUser member = null)
        {
            if (member == null)
                member = (IGuildUser)Context.User;
            var recruits = _context.GuildMembers
                .Include(g => g.Member)
                .Where(g => g.GuildId == member.GuildId && g.RecruiterMemberId == member.Id);

            var res = new EmbedBuilder()
                .WithDefault()
                .WithAuthor(member);

            foreach (var recruit in recruits)
            {
                res.AddField(recruit.JoinedAt.ToString(), value: $"<@{recruit.MemberId}>");
            }

            await ReplyAsync(embed: res.Build());
        }

        [Command("By")]
        public async Task ByAsync(IGuildUser member = null)
        {
            if (member == null)
                member = (IGuildUser)Context.User;

            var dbUser = await _context.GetGuildMemberAsync(member.GuildId, member.Id, addIfNonExistent: true);
            var recruiter = await _context.GuildMembers.Include(g => g.Member).SingleOrDefaultAsync(g => g.GuildId == member.GuildId && dbUser.RecruiterMemberId == g.MemberId);

            var embed = new EmbedBuilder().WithDefault();
            if (recruiter == null)
                await ReplyAsync(embed: embed.WithDescription($"{member.GetFullName()} was not recruited by anyone.").Build());
            else
                await ReplyAsync(embed: embed.WithDescription($"{member.Mention} was recruited by <@{recruiter.MemberId}>.").Build());
        }
    }
}
