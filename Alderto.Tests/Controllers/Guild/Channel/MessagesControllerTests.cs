using Xunit;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
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

            res = (ObjectResult)await _controller.CreateMessage(Dummies.GuildA.Id, new ApiMessage { ChannelId = 2, Id = 1000 });
            var message = (GuildManagedMessage)((ObjectResult)res).Value;

            Assert.Equal(2ul, message.ChannelId);
            Assert.Equal(1000ul, message.MessageId);
            Assert.Equal(1ul, message.GuildId);

            // Test invariance of bankId.
            res = await _controller.EditMessage(1, 1000,
                new ApiMessage()
                {
                    Contents = "Hello test!"
                });
            Assert.IsType<OkResult>(res);

            res = await _controller.GetMessage(1, 1000);
            var unit = (ApiMessage)((ObjectResult)res).Value;

            Assert.Equal("Hello test!", unit.Contents);
            Assert.Equal(1000ul, unit.Id);


            res = await _controller.ListMessages(1);
            items = (IEnumerable<GuildManagedMessage>)((ObjectResult)res).Value;

            Assert.NotEmpty(items);


            res = await _controller.RemoveMessage(1, 1000);
            Assert.IsType<NoContentResult>(res);


            res = await _controller.ListMessages(1);
            items = (IEnumerable<GuildManagedMessage>)((ObjectResult)res).Value;

            Assert.Empty(items);
        }
    }
}