using System.Threading.Tasks;
using Alderto.Bot.Modules;
using Alderto.Data;
using Discord;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alderto.Tests.ModulesTests
{
    public class CurrencyModulesTests
    {
        private readonly CurrencyModule _module;
        private readonly IAldertoDbContext _database;

        public CurrencyModulesTests()
        {
            _database = Dummies.Database;
            _module = new CurrencyModule(_database);
        }

        [Fact]
        public async Task Give()
        {
            var user = Dummies.Alice;

            await _module.ModifyAsyncExec(20, new[] { Dummies.Alice, Dummies.Alice });
            var dbUser = await _database.Members.SingleOrDefaultAsync(m => m.GuildId == user.GuildId && m.MemberId == user.Id);
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
