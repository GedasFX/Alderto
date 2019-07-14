using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extentions;
using Alderto.Data;
using Alderto.Data.Extensions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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

        [Command("Recruited")]
        public async Task Recruited(IGuildUser recruiter, params IGuildUser[] recruited)
        {
            var recruiterId = recruiter.Id;
            foreach (var member in recruited)
            {
                var dbUser = await _context.GetGuildMemberAsync(recruiter.GuildId, member.Id, addIfNonExistent: true);
                dbUser.RecruiterMemberId = recruiterId;
            }

            await _context.SaveChangesAsync();
        }

        [Command("List")]
        public void List(IGuildUser member)
        {
            var recruits = _context.GuildMembers
                .Include(g => g.Member)
                .Where(g => g.GuildId == member.GuildId && g.RecruiterMemberId == member.Id);

            var res = new EmbedBuilder()
                .WithDefault()
                .WithAuthor(member);

            foreach (var recruit in recruits)
            {
                res.AddField($"{recruit.Member.Username}#{recruit.Member.Discriminator}", value: null);
            }

        }

        [Command("By")]
        public async Task ByAsync(IGuildUser member)
        {
            var dbUser = await _context.GetGuildMemberAsync(member.GuildId, member.Id);
            //dbUser.RecruitedByGuildMember;
        }
    }
}
