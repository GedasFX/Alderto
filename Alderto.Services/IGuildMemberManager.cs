using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Discord;

namespace Alderto.Services
{
    public interface IGuildMemberManager
    {
        /// <summary>
        /// Adds a member to <see cref="IAldertoDbContext.GuildMembers"/> and, if needed,
        /// creates a guild in <see cref="IAldertoDbContext.Guilds"/> and a member in <see cref="IAldertoDbContext.Members"/>
        /// </summary>
        /// <param name="guildMember">New <see cref="GuildMember"/> to add.</param>
        Task AddGuildMemberAsync(GuildMember guildMember);

        /// <summary>
        /// Adds a guild to <see cref="IAldertoDbContext.Guilds"/>.
        /// </summary>
        /// <param name="guild">New <see cref="Guild"/> to add.</param>
        Task AddGuildAsync(Guild guild);

        /// <summary>
        /// Adds a member to <see cref="IAldertoDbContext.Members"/>.
        /// </summary>
        /// <param name="member">New <see cref="Member"/> to add.</param>
        Task AddMemberAsync(Member member);

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
        /// <param name="recruitedAt">Time user got recruited. Recruited time approx. equals joined time.</param>
        Task AddRecruitAsync(GuildMember recruitedMember, ulong recruiterMemberId, DateTimeOffset recruitedAt);

        /// <summary>
        /// Provides a collection of members recruited by the given person.
        /// </summary>
        /// <param name="member">Member to get recruits of.</param>
        /// <returns>A collection of members recruited by the given person.</returns>
        IEnumerable<GuildMember> ListRecruitsAsync(GuildMember member);

        /// <summary>
        /// Accepts the member to the guild.
        /// </summary>
        /// <param name="user">Guild user to accept</param>
        /// <param name="nickname">[Optional] Change accepted user's nickname (Nickname policy)</param>
        /// <param name="role">[Optional] Add user to accepted role (Role policy)</param>
        /// <param name="recruiterId">[Optional] Add user as a recruit of someone (Recruitment policy)</param>
        Task AcceptMemberAsync(IGuildUser user, string nickname = null, IRole role = null, ulong recruiterId = 0);
    }
}