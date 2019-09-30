using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Services;
using Alderto.Tests.MockedEntities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Alderto.Tests
{
    public class UserProviderTests
    {
        private readonly IGuildMemberManager _manager;

        public UserProviderTests()
        {
            _manager = MockServices.ScopedServiceProvider.GetService<IGuildMemberManager>();
        }

        [Fact]
        public async Task GetMember()
        {
            // Fail to get the entity when he doesn't exist.
            var member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1, addIfNonExistent: false);
            Assert.Null(member);

            // This time make sure to add the member.
            member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1);
            Assert.NotNull(member);

            // Check if internal extenstion .SingleOrDefaultAsync() does not crash
            member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1);
            Assert.NotNull(member);

            // Once more to see if it wasn't added a second time.
            member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1);
            Assert.NotNull(member);
        }
    }
}
