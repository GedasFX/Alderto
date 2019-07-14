using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
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
        [RequireRole("Admin")]
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

            await this.ReplyEmbedAsync($"Successfully registered {recruited.Length} user(s) as recruits of {recruiter.Mention}.", color: EmbedColor.Success);
        }

        [Command("List")]
        public async Task ListAsync(IGuildUser member = null)
        {
            if (member == null)
                member = (IGuildUser)Context.User;
            var recruits = _context.GuildMembers
                .Include(g => g.Member)
                .Where(g => g.GuildId == member.GuildId && g.RecruiterMemberId == member.Id);

            await this.ReplyEmbedAsync(extra: builder =>
            {
                foreach (var recruit in recruits)
                {
                    builder.AddField(recruit.JoinedAt.ToString(), value: $"<@{recruit.MemberId}>");
                }

                builder.WithAuthor(member);
            });
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
                await this.ReplyEmbedAsync($"{member.Mention} was not recruited by anyone.");
            else
                await this.ReplyEmbedAsync($"{member.Mention} was recruited by <@{recruiter.MemberId}>.");
        }
    }
}
