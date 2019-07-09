using System;
using Alderto.Data;
using Alderto.Tests.MockedEntities;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Tests
{
    public static class Dummies
    {
        public static MockGuildUser Alice { get; }
        public static MockGuildUser Bob { get; }

        public static MockGuild Guild { get; }

        public static IServiceProvider ServiceProvider { get; }

        static Dummies()
        {
            Alice = new MockGuildUser
            {
                Id = 1,
                GuildId = 5,
                CreatedAt = new DateTimeOffset(year: 2018, month: 1, day: 1, hour: 12, minute: 0, second: 0, TimeSpan.FromHours(0))
            };
            Bob = new MockGuildUser
            {
                Id = 2,
                CreatedAt = new DateTimeOffset(year: 2018, month: 1, day: 2, hour: 12, minute: 0, second: 0, TimeSpan.FromHours(1))
            };

            Guild = new MockGuild
            {
                Id = 5
            };
            
            ServiceProvider = new ServiceCollection()
                .AddDbContext<IAldertoDbContext, MockDbContext>()
                .BuildServiceProvider();
        }
    }
}
