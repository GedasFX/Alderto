using System;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
using Alderto.Bot.Services;
using Alderto.Data;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Modules
{
    [Group("Recruitment"), Alias("Recruit", "Recruited", "Rec")]
    public class RecruitmentModule : ModuleBase<SocketCommandContext>
    {
        private readonly IGuildUserManager _guildUserManager;
        private readonly IAldertoDbContext _context;

        public RecruitmentModule(IGuildUserManager guildUserManager, IAldertoDbContext context)
        {
            _guildUserManager = guildUserManager;
            _context = context;
        }

        [Command("Recruited"), Alias("Add")]
        [RequireRole("Admin")]
        public async Task Recruited(IGuildUser recruiter, params IGuildUser[] recruited)
        {
            var recruiterId = recruiter.Id;
            foreach (var member in recruited)
            {
                var dbUser = await _guildUserManager.GetGuildMemberAsync(recruiter.GuildId, member.Id);
                await _guildUserManager.AddRecruitAsync(dbUser, recruiterId, DateTimeOffset.Now);
            }

            await this.ReplySuccessEmbedAsync($"Successfully registered {recruited.Length} user(s) as recruits of {recruiter.Mention}.");
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
                    builder.AddField(recruit.JoinedAt.ToString(), $"<@{recruit.MemberId}>");
                }

                builder.WithAuthor(member);
            });
        }

        [Command("By")]
        public async Task ByAsync(IGuildUser member = null)
        {
            if (member == null)
                member = (IGuildUser)Context.User;

            var dbMember = await _guildUserManager.GetGuildMemberAsync(member.GuildId, member.Id);
            if (dbMember?.RecruiterMemberId == null)
            {
                await this.ReplyEmbedAsync($"{member.Mention} was not recruited by anyone.");
                return;
            }

            var recruiter = await _guildUserManager.GetGuildMemberAsync(member.GuildId, (ulong)dbMember.RecruiterMemberId);
            await this.ReplyEmbedAsync($"{member.Mention} was recruited by <@{recruiter.MemberId}>.");
        }
    }
}
