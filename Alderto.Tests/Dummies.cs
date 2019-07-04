using System;
using System.ComponentModel.Design;
using Alderto.Data;
using Alderto.Tests.MockedEntities;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Alderto.Tests
{
    public static class Dummies
    {
        public static MockGuildUser Alice { get; }
        public static MockGuildUser Bob { get; }

        public static MockGuild Guild { get; }

        public static IAldertoDbContext Database { get; }

        public static MockSocketCommandContext SocketCommandContext { get; }

        public static IServiceProvider ServiceProvider { get; }

        static Dummies()
        {
            Alice = new MockGuildUser
            {
                Id = 1,
                GuildId = 5,
                CreatedAt = new DateTimeOffset(2018, 1, 1, 12, 0, 0, TimeSpan.FromHours(0))
            };
            Bob = new MockGuildUser
            {
                Id = 2,
                CreatedAt = new DateTimeOffset(2018, 1, 2, 12, 0, 0, TimeSpan.FromHours(1))
            };

            Guild = new MockGuild
            {
                Id = 5
            };

            Database = new MockDbContext();

            ServiceProvider = new ServiceCollection()
                .AddDbContext<IAldertoDbContext, MockDbContext>()
                .AddDbContext<SqliteDbContext>()
                .BuildServiceProvider();
        }
    }
}
