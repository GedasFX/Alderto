using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Tests.MockedEntities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Alderto.Tests
{
    public class GuildBankManagerTests
    {
        private readonly IGuildBankManager _bank;
        private readonly IGuildBankItemsManager _bankItems;

        public GuildBankManagerTests()
        {
            var services = MockServices.ScopedServiceProvider;

            _bank = services.GetService<IGuildBankManager>();
            _bankItems = services.GetService<IGuildBankItemsManager>();
        }

        [Fact]
        public async Task TestItemManagerCrud()
        {
            var item = await _bankItems.GetBankItemAsync(new GuildBank(1, "bank") { Id = 1 }, "item");
            Assert.Null(item);

            var bank = await _bank.CreateGuildBankAsync(1, 1, new GuildBank(1, "bank"));

            var i = await _bankItems.CreateBankItemAsync(bank, new GuildBankItem("item") { Description = "d", Value = -0.3, Quantity = -1.6 }, 1);

            item = (await _bankItems.GetBankItemAsync(bank, i.Id))!;
            Assert.Equal("d", item.Description);

            await _bankItems.UpdateBankItemAsync(bank, i.Id, 1, bankItem => bankItem.Description = "t", 1);
            Assert.Equal("t", item.Description);

            item = (await _bankItems.GetBankItemAsync(bank, 1))!;
            Assert.Equal("t", item.Description);

            await _bankItems.RemoveBankItemAsync(bank, i.Id, 1);
            item = await _bankItems.GetBankItemAsync(bank, 1);
            Assert.Null(item);
        }

        [Fact]
        public async Task TestManager()
        {
            var b = await _bank.CreateGuildBankAsync(1, 1, new GuildBank(1, "main"));
            Assert.NotEqual(0, b.Id);
            Assert.Equal(1u, b.GuildId);

            var item = await _bankItems.CreateBankItemAsync(b, new GuildBankItem("bb") { Description = "cc" }, 1);
            await _bank.UpdateGuildBankAsync(1, "main", 1, bb =>
            {
                bb.GuildId = 2;
                bb.Name = "bb";
            });
            await _bankItems.UpdateBankItemAsync(b, item.Id, 1, bb => bb.Quantity -= 2266.4);

            var bi = (await _bankItems.GetBankItemAsync(b, item.Id))!;
            Assert.Equal(-2266.4, bi.Quantity);
        }
    }
}