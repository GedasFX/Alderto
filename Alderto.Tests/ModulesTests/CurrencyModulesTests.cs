using System.Threading.Tasks;
using Alderto.Bot.Modules;
using Alderto.Data;
using Alderto.Tests.MockedEntities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alderto.Tests.ModulesTests
{
    public class CustomCommandsProviderServiceTests
    {
        private readonly CurrencyModule _module;
        private readonly IAldertoDbContext _context;

        public CustomCommandsProviderServiceTests()
        {
            _context = new MockDbContext();
            _module = new CurrencyModule(_context);
        }

        [Fact]
        public async Task Give()
        {
            var user = Dummies.Alice;
            await _module.ModifyAsyncExec(20, new[] { Dummies.Alice, Dummies.Alice });
            var dbUser = await _context.Members.SingleOrDefaultAsync(m => m.GuildId == user.GuildId && m.MemberId == user.Id);
            Assert.Equal(40, dbUser.CurrencyCount);
        }

        [Fact]
        public async Task PreconditionsForGiveAndTake()
        {
            // Would crash if not returned at the start of method.
            await _module.GiveAsync(-5);
            await _module.TakeAsync(-5);
        }
    }
}
