using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Services;
using Alderto.Tests.MockedEntities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alderto.Tests
{
    public class CurrencyProviderTests
    {
        private readonly ICurrencyManager _manager;
        private readonly IAldertoDbContext _context;
        private readonly GuildMemberManager _guildMemberManager;

        public CurrencyProviderTests()
        {
            _context = new MockDbContext();
            _manager = new CurrencyManager(_context);
            _guildMemberManager = new GuildMemberManager(_context);
        }

        [Fact]
        public async Task Give()
        {
            var user = Dummies.Alice;
            await _manager.ModifyPointsAsync(await _guildMemberManager.GetGuildMemberAsync(user.GuildId, user.Id), deltaPoints: 20);
            var dbUser = await _context.GuildMembers.SingleOrDefaultAsync(m => m.GuildId == user.GuildId && m.MemberId == user.Id);

            Assert.Equal(expected: 20, dbUser.CurrencyCount);
        }

        [Fact]
        public async Task Timely()
        {
            var user = Dummies.Alice;
            var member = await _guildMemberManager.GetGuildMemberAsync(user);
            
            // At the start, the currency count should be 0.
            Assert.Equal(expected: 0, member.CurrencyCount);

            var res = await _manager.GrantTimelyRewardAsync(member, amount: 3, cooldown: 1);

            // Should be granted.
            Assert.Null(res);
            Assert.Equal(expected: 3, member.CurrencyCount);

            // Cooldown check.
            res = await _manager.GrantTimelyRewardAsync(member, amount: 3, cooldown: 1);
            Assert.NotNull(res);
            Assert.Equal(expected: 3, member.CurrencyCount);

            // Add slightly more than 1 second.
            await Task.Delay(1100);

            // Cooldown check #2.
            res = await _manager.GrantTimelyRewardAsync(member, amount: -5, cooldown: 1);
            Assert.Null(res);
            Assert.Equal(expected: -2, member.CurrencyCount);
        }
    }
}
