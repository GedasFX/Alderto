using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Alderto.Tests.MockedEntities
{
    public class MockSocketCommandContext : SocketCommandContext, ICommandContext
    {
        public new IDiscordClient Client { get; set; }
        public new IGuild Guild { get; set; }
        public new IMessageChannel Channel { get; set; }
        public new IUser User { get; set; }
        public new IUserMessage Message { get; set; }

        public MockSocketCommandContext() : base(null, null)
        {
            
        }

        public MockSocketCommandContext(DiscordSocketClient client, SocketUserMessage msg) : base(client, msg)
        {
            Client = client;
            Message = msg;
        }
    }
}
