using System;
using Alderto.Data.Models;
using AutoMapper;

namespace Alderto.Application.Features.ManagedMessage.Dto
{
    public class ManagedMessageDto
    {
        public ulong Id { get; set; }
        public ulong ChannelId { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public string? Content { get; set; }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<GuildManagedMessage, ManagedMessageDto>();
            }
        }
    }
}
