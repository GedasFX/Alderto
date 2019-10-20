using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Tests.Extensions;
using Alderto.Tests.MockedEntities;
using Alderto.Web.Models.Bank;
using Discord;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Web.Controllers.Guild.Bank.Tests
{
    public class BanksControllerTests
    {
        private readonly BanksController _controller;

        public BanksControllerTests()
        {
            var services = MockServices.ScopedServiceProvider;

            _controller = new BanksController(
                services.GetService<IGuildBankManager>(),
                services.GetService<IDiscordClient>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
                }
            };
        }

        [Fact()]
        public async Task IntegrityTest()
        {
            _controller.User.SetId(Dummies.Alice.Id);
            var res = await _controller.ListBanks(Dummies.GuildA.Id);
            var items = (IEnumerable<ApiGuildBank>)((ObjectResult)res).Value;

            Assert.Empty(items);

            res = (ObjectResult)await _controller.CreateBank(Dummies.GuildA.Id, new ApiGuildBank(new GuildBank(Dummies.GuildA.Id, "Bank1") { ModeratorRoleId = 2 }));
            var bank = (ApiGuildBank)((ObjectResult)res).Value;

            Assert.Equal("Bank1", bank.Name);
            Assert.NotNull(bank.ModeratorRoleId);
            Assert.Equal(2ul, bank.ModeratorRoleId!.Value);
            Assert.Equal(1ul, bank.GuildId);
            Assert.Equal(1, bank.Id);

            // Test invariance of bankId.
            res = await _controller.EditBank(1, 1,
                new GuildBank(1, "NewName")
                {
                    Id = 999,
                    ModeratorRoleId = null
                });
            Assert.IsType<OkResult>(res);

            res = await _controller.GetBank(1, 1);
            bank = (ApiGuildBank)((ObjectResult)res).Value;

            Assert.Equal("NewName", bank.Name);
            Assert.Null(bank.ModeratorRoleId);
            Assert.Equal(1, bank.Id);


            res = await _controller.ListBanks(1);
            items = (IEnumerable<ApiGuildBank>)((ObjectResult)res).Value;

            Assert.NotEmpty(items);


            res = await _controller.RemoveBank(1, 1);
            Assert.IsType<NoContentResult>(res);


            res = await _controller.ListBanks(1);
            items = (IEnumerable<ApiGuildBank>)((ObjectResult)res).Value;

            Assert.Empty(items);
        }
    }
}