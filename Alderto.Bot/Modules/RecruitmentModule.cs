using System;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
using Alderto.Services;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Recruitment"), Alias("Recruit", "Recruited", "Rec")]
    public class RecruitmentModule : ModuleBase<SocketCommandContext>
    {
        private readonly IGuildMemberManager _guildMemberManager;

        public RecruitmentModule(IGuildMemberManager guildMemberManager)
        {
            _guildMemberManager = guildMemberManager;
        }

        [Command("Recruited"), Alias("Add")]
        [Summary("Adds members as recruit of a member.")]
        [RequireRole("Admin")]
        public async Task Recruited(
            [Summary("Recruiter")] IGuildUser recruiter,
            [Summary("Members recruited")] params IGuildUser[] recruited)
        {
            var recruiterId = recruiter.Id;
            foreach (var member in recruited)
            {
                var dbUser = await _guildMemberManager.GetGuildMemberAsync(recruiter.GuildId, member.Id);
                await _guildMemberManager.AddRecruitAsync(dbUser, recruiterId, DateTimeOffset.UtcNow);
            }

            await this.ReplySuccessEmbedAsync($"Successfully registered {recruited.Length} user(s) as recruits of {recruiter.Mention}.");
        }

        [Command("List")]
        [Summary("Lists all member recruited by the person.")]
        public async Task List(
            [Summary("Recruiter. Not specifying a user will list your own recruits.")] IGuildUser user = null)
        {
            if (user == null)
                user = (IGuildUser)Context.User;

            var recruits = _guildMemberManager.ListRecruitsAsync(await _guildMemberManager.GetGuildMemberAsync(user));

            await this.ReplyEmbedAsync(extra: builder =>
            {
                foreach (var recruit in recruits)
                {
                    builder.AddField(recruit.JoinedAt.ToString(), $"<@{recruit.MemberId}>");
                }

                builder.WithAuthor(user);
            });
        }

        [Command("By")]
        [Summary("Displays the member this person was recruited by.")]
        public async Task By(
            [Summary("Recruit. Not specifying a user will display your recruiter.")] IGuildUser member = null)
        {
            if (member == null)
                member = (IGuildUser)Context.User;

            var dbMember = await _guildMemberManager.GetGuildMemberAsync(member.GuildId, member.Id);
            if (dbMember?.RecruiterMemberId == null)
            {
                await this.ReplyEmbedAsync($"{member.Mention} was not recruited by anyone.");
                return;
            }

            var recruiter = await _guildMemberManager.GetGuildMemberAsync(member.GuildId, (ulong)dbMember.RecruiterMemberId);
            await this.ReplyEmbedAsync($"{member.Mention} was recruited by <@{recruiter.MemberId}>.");
        }
    }
}
