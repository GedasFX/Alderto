﻿using System;
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
            
            ServiceProvider = new ServiceCollection()
                .AddDbContext<IAldertoDbContext, MockDbContext>()
                .AddDbContext<SqliteDbContext>()
                .BuildServiceProvider();
        }
    }
}