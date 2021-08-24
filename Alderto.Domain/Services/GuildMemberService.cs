using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Domain.Services
{
    public class GuildMemberService : IGuildMemberService
    {
        private readonly AldertoDbContext _context;

        public GuildMemberService(AldertoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a member to <see cref="AldertoDbContext.GuildMembers"/> and, if needed,
        /// creates a guild in <see cref="AldertoDbContext.Guilds"/> and a member in <see cref="AldertoDbContext.Members"/>
        /// </summary>
        /// <param name="guildMember">New <see cref="GuildMember"/> to add.</param>
        public async Task AddGuildMemberAsync(GuildMember guildMember)
        {
            var guild = await _context.Guilds.FindAsync(guildMember.GuildId);
            if (guild == null)
            {
                guild = new Guild(guildMember.GuildId);
                _context.Guilds.Add(guild);
            }

            var member = await _context.Members.FindAsync(guildMember.MemberId);
            if (member == null)
            {
                member = new Member(guildMember.MemberId);
                _context.Members.Add(member);
            }

            _context.GuildMembers.Add(guildMember);

            await _context.SaveChangesAsync();
        }

        public async Task AddGuildAsync(Guild guild)
        {
            await _context.Guilds.AddAsync(guild);
            await _context.SaveChangesAsync();
        }

        public async Task AddMemberAsync(Member member)
        {
            await _context.Members.AddAsync(member);
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
        public async Task<GuildMember?> GetGuildMemberAsync(ulong guildId, ulong memberId, bool addIfNonExistent = true)
        {
            var member = await _context.GuildMembers.FindAsync(guildId, memberId);

            // Check if member exists. If yes - return it
            if (member != null)
                return member;

            // Check if AddIfNonExistent flag was unset. If unset - return null
            if (!addIfNonExistent)
                return null;

            // Member does not exist and addIfNotExistent flag was set. Add to database and return.
            member = new GuildMember(guildId, memberId);
            await AddGuildMemberAsync(member);
            return member;
        }
    }
}