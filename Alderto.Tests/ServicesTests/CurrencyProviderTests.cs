using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Data.Extensions;
using Alderto.Tests.MockedEntities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alderto.Tests.ServicesTests
{
    public class CurrencyProviderTests
    {
        private readonly ICurrencyProvider _provider;
        private readonly IAldertoDbContext _context;

        public CurrencyProviderTests()
        {
            _context = new MockDbContext();
            _provider = new CurrencyProvider(_context);
        }

        [Fact]
        public async Task Give()
        {
            var user = Dummies.Alice;
            await _provider.ModifyPointsAsync(new[] { await _context.GetGuildMemberAsync(user.GuildId, user.Id) }, deltaPoints: 20);
            var dbUser = await _context.GuildMembers.SingleOrDefaultAsync(m => m.GuildId == user.GuildId && m.MemberId == user.Id);

            Assert.Equal(expected: 20, dbUser.CurrencyCount);
        }
    }
}
