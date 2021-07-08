using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Discord.Dto;
using Alderto.Domain.Exceptions;
using AutoMapper;
using Discord;
using MediatR;

namespace Alderto.Application.Features.Discord.Query
{
    public static class Roles
    {
        public class List<TOut> : QueryRequest<IList<TOut>>
        {
            public List(ulong guildId, ulong memberId) : base(guildId, memberId)
            {
            }
        }

        public class QueryHandler : IRequestHandler<List<DiscordRoleDto>, IList<DiscordRoleDto>>
        {
            private readonly IMapper _mapper;
            private readonly IDiscordClient _client;

            public QueryHandler(IDiscordClient client, IMapper mapper)
            {
                _client = client;
                _mapper = mapper;
            }

            public async Task<IList<DiscordRoleDto>> Handle(List<DiscordRoleDto> request,
                CancellationToken cancellationToken)
            {
                var guild = await _client.GetGuildAsync(request.GuildId);
                if (guild == null)
                    throw new ValidationDomainException(ErrorMessage.GUILD_NOT_FOUND);

                return guild.Roles.Select(r => _mapper.Map<DiscordRoleDto>(r)).ToList();
            }
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<IRole, DiscordRoleDto>();
            }
        }
    }
}
