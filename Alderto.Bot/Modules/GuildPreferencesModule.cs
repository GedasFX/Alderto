using System.Threading.Tasks;
using Alderto.Application.Features.GuildConfiguration;
using Alderto.Bot.Extensions;
using Alderto.Domain.Services;
using Discord;
using Discord.Commands;
using MediatR;

namespace Alderto.Bot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    [Group("Config"), Alias("Preferences", "Pref", "Cfg")]
    public class GuildPreferencesModule : ModuleBase<SocketCommandContext>
    {
        [Group, Alias("Get")]
        public class GetPreferenceModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildSetupService _guildSetupService;

            public GetPreferenceModule(IGuildSetupService guildSetupService)
            {
                _guildSetupService = guildSetupService;
            }

            [Command("Prefix")]
            [Summary("Gets the guild's set prefix.")]
            public async Task Prefix()
            {
                var guildSetup = await _guildSetupService.GetGuildSetupAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Prefix: **{guildSetup.Configuration.Prefix}**");
            }
        }

        [Group("Set")]
        public class SetPreferenceModule : ModuleBase<SocketCommandContext>
        {
            private readonly IMediator _mediator;

            public SetPreferenceModule(IMediator mediator)
            {
                _mediator = mediator;
            }

            [Command("Prefix")]
            [Summary("Sets the guild's set prefix.")]
            public async Task Prefix(
                [Summary("Prefix.")] string prefix)
            {
                await _mediator.Send(new UpdateGuildConfiguration.Command(Context.Guild.Id, Context.User.Id, prefix));
                await this.ReplySuccessEmbedAsync($"Prefix was changed to: **{prefix}**");
            }
        }
    }
}