using Xunit;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Services.Exceptions.NotFound;
using Alderto.Tests.Extensions;
using Alderto.Tests.MockedEntities;
using Alderto.Web.Models;
using Alderto.Web.Models.Bank;
using Discord;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Web.Controllers.Guild.Channel.Tests
{
    public class MessagesControllerTests
    {
        private readonly MessagesController _controller;

        public MessagesControllerTests()
        {
            var services = MockServices.ScopedServiceProvider;

            _controller = new MessagesController(services.GetService<IMessagesManager>())
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
            var res = await _controller.ListMessages(Dummies.GuildA.Id);
            var items = (IEnumerable<GuildManagedMessage>)((ObjectResult)res).Value;

            Assert.Empty(items);

            await Assert.ThrowsAsync<MessageNotFoundException>(async () =>
                await _controller.CreateMessage(Dummies.GuildA.Id, new ApiMessage { ChannelId = 1, Id = 1000 }));
            
            // Impossible to test due to inability to create mocked entities, even with moq.
        }
    }
}