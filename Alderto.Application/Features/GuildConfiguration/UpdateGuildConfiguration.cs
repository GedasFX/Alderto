using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.GuildCommandAlias;
using Alderto.Domain.Services;
using AutoMapper;
using MediatR;

namespace Alderto.Application.Features.GuildConfiguration
{
    public class UpdateGuildConfiguration
    {
        public class Command : Request
        {
            [MaxLength(20)]
            public string? Prefix { get; }

            public ulong? ModeratorRoleId { get; }

            public Command(ulong guildId, ulong memberId, string? prefix = null, ulong? moderatorRoleId = null)
                : base(guildId, memberId)
            {
                Prefix = prefix;
                ModeratorRoleId = moderatorRoleId;
            }
        }

        public class CommandHandler : IRequestHandler<RegisterCommandAlias.RhCommand>
        {
            private readonly IGuildSetupService _guildSetupService;
            private readonly IMapper _mapper;

            public CommandHandler(IGuildSetupService guildSetupService, IMapper mapper)
            {
                _guildSetupService = guildSetupService;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(RegisterCommandAlias.RhCommand request, CancellationToken cancellationToken)
            {
                var setup = await _guildSetupService.GetGuildSetupAsync(request.GuildId);
                var configuration = setup.Configuration;

                // If config.GuildId == 0, then it means that the guild uses default preferences.
                // Default preferences [id == 0] are applied only when the GetPreferencesAsync() cannot find them in database.
                var guildPreferencesPresentInDatabase = configuration.GuildId > 0;

                _mapper.Map(request, configuration);
                if (!guildPreferencesPresentInDatabase)
                    configuration.GuildId = 0;

                await _guildSetupService.UpdateGuildConfigurationAsync(request.GuildId, configuration);

                return Unit.Value;
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, Data.Models.GuildConfiguration>();
            }
        }
    }
}