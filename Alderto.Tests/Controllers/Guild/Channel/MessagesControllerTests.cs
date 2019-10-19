using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Data.Models;
using Alderto.Services;
using Alderto.Services.Exceptions;
using Alderto.Tests.Extensions;
using Alderto.Tests.MockedEntities;
using Alderto.Web.Models;
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

            _controller = new MessagesController(services.GetService<IMessagesManager>(), services.GetService<IDiscordClient>())
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
                await _controller.CreateMessage(Dummies.GuildA.Id, new ApiManagedMessage(new GuildManagedMessage(1, 1, 1000, null!))));
            
            // Impossible to test due to inability to create mocked entities (IAsyncEnumerable duality), even with moq.
        }
    }
}