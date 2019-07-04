using System;
using System.ComponentModel.Design;
using Alderto.Data;
using Alderto.Tests.MockedEntities;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Alderto.Tests
{
    public static class Dummies
    {
        public static MockUser Alice { get; }
        public static MockUser Bob { get; }

        public static MockGuild Guild { get; }

        public static SqliteDbContext Database { get; }

        public static MockSocketCommandContext SocketCommandContext { get; }

        public static IServiceProvider ServiceProvider { get; }

        static Dummies()
        {
            Alice = new MockUser
            {
                Id = 1,
                CreatedAt = new DateTimeOffset(2018, 1, 1, 12, 0, 0, TimeSpan.FromHours(0))
            };
            Bob = new MockUser
            {
                Id = 2,
                CreatedAt = new DateTimeOffset(2018, 1, 2, 12, 0, 0, TimeSpan.FromHours(1))
            };

            Guild = new MockGuild
            {
                Id = 5
            };

            SocketCommandContext = new MockSocketCommandContext(null, Mock.Of<SocketUserMessage>())
            {
                Guild = Guild
            };

            ServiceProvider = new ServiceCollection()
                .AddDbContext<SqliteDbContext>()
                .BuildServiceProvider();
        }
    }
}
