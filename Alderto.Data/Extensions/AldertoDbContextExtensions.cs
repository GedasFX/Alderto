using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Data.Extensions
{
    public static class AldertoDbContextExtensions
    {
        /// <summary>
        /// Adds a member to <see cref="IAldertoDbContext.GuildMembers"/> and, if needed,
        /// creates a guild in <see cref="IAldertoDbContext.Guilds"/> and a member in <see cref="IAldertoDbContext.Members"/>
        /// </summary>
        /// <param name="context">DbContext containing Members and Guilds.</param>
        /// <param name="guildMember">New <see cref="GuildMember"/> to add.</param>
        /// <returns>Member tracked by the database.</returns>
        public static async Task<GuildMember> AddMemberAsync(this IAldertoDbContext context, GuildMember guildMember)
        {
            var guild = await context.Guilds.FindAsync(guildMember.GuildId);

            // Check if guild exists, and if it doesn't - add one.
            // Otherwise the database will spew foreign key violation problems.
            if (guild == null)
            {
                guild = new Guild(guildMember.GuildId);
                await context.Guilds.AddAsync(guild);
            }

            var member = await context.Members.FindAsync(guildMember.MemberId);
            if (member == null)
            {
                member = new Member(guildMember.MemberId);
                await context.Members.AddAsync(member);
            }

            var entity = await context.GuildMembers.AddAsync(guildMember);

            await context.SaveChangesAsync();

            return entity.Entity;
        }

        /// <summary>
        /// Gets the <see cref="GuildMember"/> from the database context. Can return null if member doesn't exist and <see cref="addIfNonExistent"/> is set to false.
        /// Non-null <see cref="GuildMember"/>s returned are tracked by the DbContext.
        /// </summary>
        /// <param name="context">Database</param>
        /// <param name="guildId">Discord Guild Id</param>
        /// <param name="memberId">Discord Member Id</param>
        /// <param name="addIfNonExistent">Set to false if you for whatever reason do not wish to add the member to the database.</param>
        /// <returns>DbContext tracked <see cref="GuildMember"/>, or null, if <see cref="addIfNonExistent"/> was set to false.</returns>
        public static async Task<GuildMember> GetGuildMemberAsync(this IAldertoDbContext context, ulong guildId, ulong memberId, bool addIfNonExistent = true)
        {
            var member = await context.GuildMembers.FindAsync(memberId, guildId);

            // Check if member exists. If yes - return it
            if (member != null)
                return member;

            // Check if AddIfNonExistent flag was unset. If unset - return null
            if (!addIfNonExistent)
                return null;

            // Member does not exist and addIfNotExistent flag was set. Add to database and return.
            return await AddMemberAsync(context, new GuildMember(guildId, memberId));
        }
    }
}
