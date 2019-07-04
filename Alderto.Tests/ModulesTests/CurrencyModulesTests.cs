using System;
using System.Threading.Tasks;
using Alderto.Bot.Modules;
using Alderto.Data;
using Discord.WebSocket;
using Moq;
using Xunit;

namespace Alderto.Tests.ModulesTests
{
    public class CurrencyModulesTests
    {
        private readonly CurrencyModule _currencyModule;

        public CurrencyModulesTests()
        {
            var currencyModule = new Mock<CurrencyModule>(MockBehavior.Strict, new SqliteDbContext());
            currencyModule.SetupGet(m => m.Context)
                .Returns(Dummies.SocketCommandContext);
            
            _currencyModule = currencyModule.Object;
        }

        [Fact]
        public async Task Give()
        {
            await _currencyModule.GiveAsync(20, Dummies.Alice, Dummies.Bob);
        }
    }
}
