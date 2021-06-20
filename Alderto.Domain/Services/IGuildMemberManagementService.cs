using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Domain.Services
{
    public interface IGuildMemberManagementService
    {
        /// <summary>
        /// Adds a member to <see cref="AldertoDbContext.GuildMembers"/> and, if needed,
        /// creates a guild in <see cref="AldertoDbContext.Guilds"/> and a member in <see cref="AldertoDbContext.Members"/>
        /// </summary>
        /// <param name="guildMember">New <see cref="GuildMember"/> to add.</param>
        Task AddGuildMemberAsync(GuildMember guildMember);

        /// <summary>
        /// Adds a guild to <see cref="AldertoDbContext.Guilds"/>.
        /// </summary>
        /// <param name="guild">New <see cref="Guild"/> to add.</param>
        Task AddGuildAsync(Guild guild);

        /// <summary>
        /// Adds a member to <see cref="AldertoDbContext.Members"/>.
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
        Task<GuildMember?> GetGuildMemberAsync(ulong guildId, ulong memberId, bool addIfNonExistent = true);
    }
}