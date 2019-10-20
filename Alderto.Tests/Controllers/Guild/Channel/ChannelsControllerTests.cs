using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Tests.Extensions;
using Alderto.Tests.MockedEntities;
using Alderto.Web.Models;
using Discord;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Web.Controllers.Guild.Channel.Tests
{
    public class ChannelsControllerTests
    {
        private readonly ChannelsController _controller;

        public ChannelsControllerTests()
        {
            var services = MockServices.ScopedServiceProvider;

            _controller = new ChannelsController(services.GetService<IDiscordClient>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
                }
            };
        }

        [Fact]
        public async Task IntegrityTest()
        {
            _controller.User.SetId(Dummies.Alice.Id);

            var res = await _controller.ListChannels(Dummies.GuildA.Id);
            var channels = (IEnumerable<ApiGuildChannel>)((ObjectResult)res).Value;
            Assert.Collection(channels, 
                e => Assert.Equal("AChannel1", e.Name),
                e => Assert.Equal(2ul, e.Id));
        }
    }
}