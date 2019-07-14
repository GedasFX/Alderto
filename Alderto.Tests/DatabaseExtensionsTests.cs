using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Extensions;
using Alderto.Tests.MockedEntities;
using Xunit;

namespace Alderto.Tests
{
    public class DatabaseExtensionsTests
    {
        private readonly IAldertoDbContext _context;

        public DatabaseExtensionsTests()
        {
            _context = new MockDbContext();
        }

        [Fact]
        public async Task GetMember()
        {
            // Fail to get the entity when he doesn't exist.
            var member = await _context.GetGuildMemberAsync(guildId: 1, memberId: 1);
            Assert.Null(member);

            // This time make sure to add the member.
            member = await _context.GetGuildMemberAsync(guildId: 1, memberId: 1, addIfNonExistent: true);
            Assert.NotNull(member);
      
            // Check if internal extenstion .SingleOrDefaultAsync() does not crash
            member = await _context.GetGuildMemberAsync(guildId: 1, memberId: 1, addIfNonExistent: true);
            Assert.NotNull(member);

            // Once more to see if it wasn't added a second time.
            member = await _context.GetGuildMemberAsync(guildId: 1, memberId: 1, addIfNonExistent: true);
            Assert.NotNull(member);
        }
    }
}
