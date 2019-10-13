using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Microsoft.Extensions.DependencyInjection;
using Alderto.Services;
using Alderto.Tests.Extensions;
using Alderto.Tests.MockedEntities;
using Alderto.Web.Models.Bank;
using Discord;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild.Bank.Tests
{
    public class BankItemsControllerTests
    {
        private readonly BankItemsController _controller;

        public BankItemsControllerTests()
        {
            var services = MockServices.ScopedServiceProvider;

            _controller = new BankItemsController(
                services.GetService<IGuildBankItemsManager>(),
                services.GetService<IGuildBankManager>(),
                services.GetService<IDiscordClient>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
                }
            };

            var context = services.GetService<AldertoDbContext>();
            context.GuildBanks.Add(new GuildBank(1, "TestBank"));
            context.SaveChanges();
        }

        [Fact()]
        public async Task CrudTests()
        {
            _controller.User.SetId(1);
            var res = await _controller.ListBankItems(1, 1);
            var items = (IEnumerable<ApiGuildBankItem>)((ObjectResult)res).Value;

            Assert.Empty(items);

            res = (ObjectResult)await _controller.CreateBankItem(1, 1, new GuildBankItem("Item1"));
            var item = (ApiGuildBankItem)((ObjectResult)res).Value;

            Assert.Equal("Item1", item.Name);
            Assert.Null(item.Description);
            Assert.Equal(1, item.GuildBankId);
            Assert.Equal(1, item.Id);

            // Test invariance of bankId.
            res = await _controller.EditBankItem(1, 1, 1,
                new GuildBankItem("Item1")
                {
                    Id = 999,
                    GuildBankId = 0,
                    Description = "Description1"
                });
            Assert.IsType<OkResult>(res);

            res = await _controller.GetBankItem(1, 1, 1);
            item = (ApiGuildBankItem)((ObjectResult)res).Value;

            Assert.Equal("Item1", item.Name);
            Assert.Equal("Description1", item.Description);
            Assert.Equal(1, item.GuildBankId);
            Assert.Equal(1, item.Id);


            res = await _controller.ListBankItems(1, 1);
            items = (IEnumerable<ApiGuildBankItem>)((ObjectResult)res).Value;

            Assert.NotEmpty(items);


            res = await _controller.RemoveBankItem(1, 1, 1);
            Assert.IsType<NoContentResult>(res);


            res = await _controller.ListBankItems(1, 1);
            items = (IEnumerable<ApiGuildBankItem>)((ObjectResult)res).Value;

            Assert.Empty(items);
        }
    }
}