using System;
using Discord;

namespace Alderto.Tests.MockedEntities
{
    public static class Dummies
    {
        public static IGuildUser Alice { get; }
        public static IGuildUser Bob { get; }

        public static IGuild Guild { get; }

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
        }
    }
}
