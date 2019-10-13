using System;
using System.Threading.Tasks;
using Discord;
using Moq;

namespace Alderto.Tests.MockedEntities
{
    public static class Dummies
    {
        /// <summary>
        /// Admin in Guild A
        /// </summary>
        public static IGuildUser Alice { get; } = new MockGuildUser
        {
            Id = 1,
            GuildId = 1,
            CreatedAt = new DateTimeOffset(year: 2018, month: 1, day: 1, hour: 12, minute: 0, second: 0, TimeSpan.FromHours(0)),
            GuildPermissions = new GuildPermissions(administrator: true),
            RoleIds = new[] { 1ul }
        };

        /// <summary>
        /// Mod in Guild A
        /// </summary>
        public static IGuildUser BobA { get; } = new MockGuildUser
        {
            Id = 2,
            GuildId = 1,
            CreatedAt = new DateTimeOffset(year: 2018, month: 1, day: 2, hour: 12, minute: 0, second: 0, TimeSpan.FromHours(1)),
            GuildPermissions = new GuildPermissions(administrator: false),
            RoleIds = new[] { 1ul, 2ul }
        };

        /// <summary>
        /// Admin in Guild B
        /// </summary>
        public static IGuildUser BobB { get; } = new MockGuildUser
        {
            Id = 2,
            GuildId = 2,
            CreatedAt = new DateTimeOffset(year: 2018, month: 1, day: 2, hour: 12, minute: 0, second: 0, TimeSpan.FromHours(1)),
            GuildPermissions = new GuildPermissions(administrator: true),
            RoleIds = new[] { 1ul }
        };

        /// <summary>
        /// Mod in Guild B
        /// </summary>
        public static IGuildUser Charlie { get; } = new MockGuildUser
        {
            Id = 3,
            GuildId = 3,
            CreatedAt = new DateTimeOffset(year: 2018, month: 1, day: 2, hour: 12, minute: 0, second: 0, TimeSpan.FromHours(2)),
            GuildPermissions = new GuildPermissions(administrator: false),
            RoleIds = new[] { 1ul, 2ul }
        };

        public static IGuild GuildA { get; }
        public static IGuild GuildB { get; }

        static Dummies()
        {
            var channelA = new Mock<ITextChannel>();
            channelA.SetupGet(o => o.Id).Returns(1);
            channelA.SetupGet(o => o.Name).Returns("AChannel1");
            
            var channelB = new Mock<ITextChannel>();
            channelB.SetupGet(o => o.Id).Returns(2);
            channelB.SetupGet(o => o.Name).Returns("AChannel2");

            GuildA = new MockGuild(1, new[] { Alice, BobA }, new[] { channelA.Object, channelB.Object });
            GuildB = new MockGuild(2, new[] { BobB, Charlie }, new ITextChannel[0]);
        }
    }
}
