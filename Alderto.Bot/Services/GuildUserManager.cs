using System;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Services
{
    public class GuildUserManager : IGuildUserManager
    {
        private readonly IAldertoDbContext _context;

        public GuildUserManager(IAldertoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a member to <see cref="IAldertoDbContext.GuildMembers"/> and, if needed,
        /// creates a guild in <see cref="IAldertoDbContext.Guilds"/> and a member in <see cref="IAldertoDbContext.Members"/>
        /// </summary>
        /// <param name="guildMember">New <see cref="GuildMember"/> to add.</param>
        public async Task AddMemberAsync(GuildMember guildMember)
        {
            var guild = await _context.Guilds.FindAsync(guildMember.GuildId);

            // Check if guild exists, and if it doesn't - add one.
            // Otherwise the database will spew foreign key violation problems.
            if (guild == null)
            {
                guild = new Guild(guildMember.GuildId);
                await _context.Guilds.AddAsync(guild);
            }

            var member = await _context.Members.FindAsync(guildMember.MemberId);
            if (member == null)
            {
                member = new Member(guildMember.MemberId);
                await _context.Members.AddAsync(member);
            }

            await _context.GuildMembers.AddAsync(guildMember);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the <see cref="GuildMember"/> from the database context. Can return null if member doesn't exist and <see cref="addIfNonExistent"/> is set to false.
        /// Non-null <see cref="GuildMember"/>s returned are tracked by the DbContext.
        /// </summary>
        /// <param name="guildId">Discord Guild Id</param>
        /// <param name="memberId">Discord Member Id</param>
        /// <param name="addIfNonExistent">Set to false if you for whatever reason do not wish to add the member to the database.</param>
        /// <returns>DbContext tracked <see cref="GuildMember"/>, or null, if <see cref="addIfNonExistent"/> was set to false.</returns>
        public async Task<GuildMember> GetGuildMemberAsync(ulong guildId, ulong memberId, bool addIfNonExistent = true)
        {
            var member = await _context.GuildMembers.SingleOrDefaultAsync(g => g.GuildId == guildId && g.MemberId == memberId);

            // Check if member exists. If yes - return it
            if (member != null)
                return member;

            // Check if AddIfNonExistent flag was unset. If unset - return null
            if (!addIfNonExistent)
                return null;

            // Member does not exist and addIfNotExistent flag was set. Add to database and return.
            member = new GuildMember(guildId, memberId);
            await AddMemberAsync(member);
            return member;
        }

        public Task<GuildMember> GetGuildMemberAsync(IGuildUser user, bool addIfNonExistent = true)
        {
            return GetGuildMemberAsync(user.GuildId, user.Id, addIfNonExistent);
        }

        public async Task AddRecruitAsync(GuildMember recruitedMember, ulong recruiterMemberId, DateTimeOffset recruitedAt)
        {
            _context.Attach(recruitedMember);

            recruitedMember.RecruiterMemberId = recruiterMemberId;
            recruitedMember.JoinedAt = recruitedAt;

            await _context.SaveChangesAsync();
        }
    }
}