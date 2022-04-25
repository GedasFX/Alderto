﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;

#nullable disable

namespace Alderto.Tests.MockedEntities
{
    public class MockGuildUser : IGuildUser
    {
        public ulong Id { get; set; }

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public string AvatarId { get; set; }

        public string Discriminator { get; set; }

        public ushort DiscriminatorValue { get; set; }

        public bool IsBot { get; set; }

        public bool IsWebhook { get; set; }

        public string Username { get; set; }
        public UserProperties? PublicFlags { get; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Mention => '@' + Username + '#' + Discriminator;

        public IActivity Activity { get; set; }

        public UserStatus Status { get; set; }
        IReadOnlyCollection<ClientType> IPresence.ActiveClients => ActiveClients;

        public IReadOnlyCollection<IActivity> Activities { get; }

        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128) => throw new NotImplementedException();
        public string GetDefaultAvatarUrl() => throw new NotImplementedException();
        public Task<IDMChannel> GetOrCreateDMChannelAsync(RequestOptions options = null) => throw new NotImplementedException();
        public bool IsDeafened { get; set; }
        public bool IsMuted { get; set; }
        public bool IsSelfDeafened { get; set; }
        public bool IsSelfMuted { get; set; }
        public bool IsSuppressed { get; set; }
        public IVoiceChannel VoiceChannel { get; set; }
        public string VoiceSessionId { get; set; }
        public ChannelPermissions GetPermissions(IGuildChannel channel)
        {
            throw new NotImplementedException();
        }

        public string GetGuildAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            throw new NotImplementedException();
        }

        public string GetDisplayAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            throw new NotImplementedException();
        }

        public Task KickAsync(string reason = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task ModifyAsync(Action<GuildUserProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task AddRoleAsync(ulong roleId, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task AddRoleAsync(IRole role, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task AddRolesAsync(IEnumerable<ulong> roleIds, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRoleAsync(ulong roleId, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRoleAsync(IRole role, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRolesAsync(IEnumerable<ulong> roleIds, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task SetTimeOutAsync(TimeSpan span, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTimeOutAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset? JoinedAt { get; set; }
        public string DisplayName { get; }
        public string Nickname { get; set; }
        public string DisplayAvatarId { get; }
        public string GuildAvatarId { get; }
        public GuildPermissions GuildPermissions { get; set; }
        public IGuild Guild { get; set; }
        public ulong GuildId { get; set; }
        public IReadOnlyCollection<ulong> RoleIds { get; set; }
        public bool? IsPending { get; }
        public int Hierarchy { get; }
        public DateTimeOffset? TimedOutUntil { get; }

        public DateTimeOffset? PremiumSince => throw new NotImplementedException();

        public IImmutableSet<ClientType> ActiveClients => throw new NotImplementedException();

        public bool IsStreaming => throw new NotImplementedException();
        public bool IsVideoing { get; }
        public DateTimeOffset? RequestToSpeakTimestamp { get; }
    }
}
