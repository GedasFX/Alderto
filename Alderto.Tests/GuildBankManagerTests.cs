using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Services.GuildBankManagers;
using Alderto.Tests.MockedEntities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Alderto.Tests
{
    public class GuildBankManagerTests
    {
        private readonly IGuildBankManager _manager;
        private readonly IGuildBankContentsManager _items;

        public GuildBankManagerTests()
        {
            var services = MockServices.ScopedServiceProvider;

            var context = services.GetService<AldertoDbContext>();
            var transactions = new GuildLogger(new Discord.WebSocket.DiscordSocketClient());
            _manager = new GuildBankManager(context, transactions);
            _items = new GuildBankContentsManager(context, transactions);

        }

        [Fact]
        public async Task TestItemManagerCrud()
        {
            var item = await _items.GetBankItemAsync(1, "item");
            Assert.Null(item);

            var bank = await _manager.CreateGuildBankAsync(1, 1, "bank");

            var i = await _items.CreateBankItemAsync(bank, new GuildBankItem { Name = "item", Description = "d", Value = -0.3, Quantity = -1.6 }, 1);

            item = await _items.GetBankItemAsync(i.Id);
            Assert.Equal("d", item.Description);

            await _items.UpdateBankItemAsync(i.Id, 1, bankItem => bankItem.Description = "t", 1);
            Assert.Equal("t", item.Description);

            item = await _items.GetBankItemAsync(1, "item");
            Assert.Equal("t", item.Description);

            await _items.RemoveBankItemAsync(i.Id, 1);
            item = await _items.GetBankItemAsync(1, "item");
            Assert.Null(item);
        }

        [Fact]
        public async Task TestManager()
        {
            var b = await _manager.CreateGuildBankAsync(1, 1, "main");
            Assert.NotEqual(0, b.Id);
            Assert.Equal(1u, b.GuildId);

            var item = await _items.CreateBankItemAsync(b, new GuildBankItem { Name = "bb", Description = "cc" }, 1);
            await _manager.UpdateGuildBankAsync(1, "main", 1, bb =>
            {
                bb.GuildId = 2;
                bb.Name = "bb";
            });
            await _items.UpdateBankItemAsync(item.Id, 1, bb => bb.Quantity -= 2266.4);

            var bi = await _items.GetBankItemAsync(item.Id);
            Assert.Equal(-2266.4, bi.Quantity);
        }
    }
}