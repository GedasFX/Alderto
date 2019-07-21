using System;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Discord;

namespace Alderto.Bot.Services
{
    public interface IGuildUserManager
    {
        /// <summary>
        /// Adds a member to <see cref="IAldertoDbContext.GuildMembers"/> and, if needed,
        /// creates a guild in <see cref="IAldertoDbContext.Guilds"/> and a member in <see cref="IAldertoDbContext.Members"/>
        /// </summary>
        /// <param name="guildMember">New <see cref="GuildMember"/> to add.</param>
        Task AddMemberAsync(GuildMember guildMember);

        /// <summary>
        /// Gets the <see cref="GuildMember"/> from the database context. Can return null if member doesn't exist and <see cref="addIfNonExistent"/> is set to false.
        /// Non-null <see cref="GuildMember"/>s returned are tracked by the DbContext.
        /// </summary>
        /// <param name="guildId">Discord Guild Id</param>
        /// <param name="memberId">Discord Member Id</param>
        /// <param name="addIfNonExistent">Set to false if you for whatever reason do not wish to add the member to the database.</param>
        /// <returns>DbContext tracked <see cref="GuildMember"/>, or null, if <see cref="addIfNonExistent"/> was set to false.</returns>
        Task<GuildMember> GetGuildMemberAsync(ulong guildId, ulong memberId, bool addIfNonExistent = true);

        /// <summary>
        /// Gets the <see cref="GuildMember"/> from the database context. Can return null if member doesn't exist and <see cref="addIfNonExistent"/> is set to false.
        /// Non-null <see cref="GuildMember"/>s returned are tracked by the DbContext.
        /// </summary>
        /// <param name="user">Discord user.</param>
        /// <param name="addIfNonExistent">Set to false if you for whatever reason do not wish to add the member to the database.</param>
        /// <returns>DbContext tracked <see cref="GuildMember"/>, or null, if <see cref="addIfNonExistent"/> was set to false.</returns>
        Task<GuildMember> GetGuildMemberAsync(IGuildUser user, bool addIfNonExistent = true);

        /// <summary>
        /// Sets the <see cref="GuildMember.RecruiterMemberId"/> value to <see cref="recruiterMemberId"/>.
        /// Ensures the <see cref="GuildMember.JoinedAt"/> is set to the correct value.
        /// </summary>
        /// <param name="recruitedMember">Recruited member.</param>
        /// <param name="recruiterMemberId">Discord user id of recruiter.</param>
        /// <param name="recruitedAt">Time user joined. If recruited, Recruited time = Joined time.</param>
        /// <returns></returns>
        Task AddRecruitAsync(GuildMember recruitedMember, ulong recruiterMemberId, DateTimeOffset recruitedAt);
    }
}