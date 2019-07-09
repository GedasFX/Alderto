using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Extentions;
using Alderto.Tests.MockedEntities;
using Xunit;

namespace Alderto.Tests
{
    public class DatabaseExtentionsTests
    {
        private readonly IAldertoDbContext _context;

        public DatabaseExtentionsTests()
        {
            _context = new MockDbContext();
        }

        [Fact]
        public async Task GetMember()
        {
            // Fail to get the entity when he doesnt exist.
            var member = await _context.GetMemberAsync(guildId: 1, memberId: 1);
            Assert.Null(member);

            // This time make sure to add the member.
            member = await _context.GetMemberAsync(guildId: 1, memberId: 1, addIfNonExistant: true);
            Assert.NotNull(member);
      
            // Check if internal extention .SingleOrDefaultAsync() does not crash
            member = await _context.GetMemberAsync(guildId: 1, memberId: 1, addIfNonExistant: true);
            Assert.NotNull(member);

            // Once more to see if it wasn't added a second time.
            member = await _context.GetMemberAsync(guildId: 1, memberId: 1, addIfNonExistant: true);
            Assert.NotNull(member);
        }
    }
}
