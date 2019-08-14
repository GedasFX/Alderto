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
            _manager = new GuildBankManager(context, new GuildBankTransactionsManager(context), _items);
        }

        [Fact]
        public async Task TestItemManagerCrud()
        {
            var item = await _items.GetItemAsync(1, "item");
            Assert.Null(item);

            await _items.CreateItemAsync(1, "item", "d", -0.3);

            item = await _items.GetItemAsync(1, "item");
            Assert.Equal("d", item.Description);

            await _items.UpdateItemAsync(1, "item", bankItem => bankItem.Description = "t");
            Assert.Equal("t", item.Description);

            item = await _items.GetItemAsync(1, "item");
            Assert.Equal("t", item.Description);

            await _items.RemoveItemAsync(1, "item");
            item = await _items.GetItemAsync(1, "item");
            Assert.Null(item);
        }

        [Fact]
        public async Task TestManager()
        {
            var b = await _manager.CreateGuildBankAsync(1, "main");
            Assert.NotEqual(0, b.Id);
            Assert.Equal(1u, b.GuildId);

            var item = await _items.CreateItemAsync(1, "bb", "cc");
            await _manager.ModifyItemCountAsync(1, "main", 1, 2, "bb", 6666);
            await _manager.ModifyItemCountAsync(1, "main", 1, 3, "bb", -2266.4);

            var bi = await _items.GetBankItemAsync(b.Id, item.Id);
            Assert.Equal(4399.6, bi.Quantity);

            var log = _manager.GetAllTransactions(1, 2).ToArray();

            Assert.Single(log);
            Assert.Equal(1u, log[0].AdminId); 
        }
    }
}