using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Alderto.Tests.MockedEntities;
using Xunit;

namespace Alderto.Tests
{
    public class GuildBankManagerTests
    {
        private readonly GuildBankManager _manager;
        private readonly GuildBankItemManager _items;

        public GuildBankManagerTests()
        {
            var context = new MockDbContext();
            _items = new GuildBankItemManager(context);
            _manager = new GuildBankManager(context, new GuildBankTransactionsManager(new Discord.WebSocket.DiscordSocketClient()), _items);
        }

        [Fact]
        public async Task TestItemManagerCrud()
        {
            var item = await _items.GetBankItemAsync(1, "item");
            Assert.Null(item);

            var i = await _items.CreateBankItemAsync(1, new GuildBankItem { Name = "item", Description = "d", Value = -0.3, Quantity = -1.6 });

            item = await _items.GetBankItemAsync(i.Id);
            Assert.Equal("d", item.Description);

            await _items.UpdateBankItemAsync(i.Id, bankItem => bankItem.Description = "t");
            Assert.Equal("t", item.Description);

            item = await _items.GetBankItemAsync(1, "item");
            Assert.Equal("t", item.Description);

            await _items.RemoveBankItemAsync(i.Id);
            item = await _items.GetBankItemAsync(1, "item");
            Assert.Null(item);
        }

        [Fact]
        public async Task TestManager()
        {
            var b = await _manager.CreateGuildBankAsync(1, "main");
            Assert.NotEqual(0, b.Id);
            Assert.Equal(1u, b.GuildId);

            var item = await _items.CreateBankItemAsync(1, new GuildBankItem { Name = "bb", Description = "cc" });
            await _manager.ModifyItemCountAsync(1, "main", 1, 2, "bb", 6666);
            await _manager.ModifyItemCountAsync(1, "main", 1, 3, "bb", -2266.4);

            var bi = await _items.GetBankItemAsync(item.Id);
            Assert.Equal(4399.6, bi.Quantity);
        }
    }
}