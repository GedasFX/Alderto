using System.Threading.Tasks;
using Alderto.Application.Features.GuildCommandAlias;
using Alderto.Bot.Extensions;
using Discord;
using Discord.Commands;
using MediatR;

namespace Alderto.Bot.Modules
{
    [Group("alias"), RequireUserPermission(GuildPermission.Administrator)]
    public class AliasModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public AliasModule(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("register"), Alias("add", "create")]
        [Summary("Registers a given alias to a specified command")]
        public async Task RegisterCommand(string alias, [Remainder] string command)
        {
            await _mediator.Send(new RegisterCommandAlias.RhCommand(Context.Guild.Id, Context.User.Id, alias, command));

            await this.ReplySuccessEmbedAsync($"Command '{command}' can now be triggered by typing '{alias}' instead.");
        }

        [Command("unregister"), Alias("remove", "delete")]
        [Summary("Registers a given alias to a specified command")]
        public async Task RegisterCommand(string alias)
        {
            var result =
                await _mediator.Send(new RemoveCommandAlias.RhCommand(Context.Guild.Id, Context.User.Id, alias));

            await this.ReplySuccessEmbedAsync(
                $"Alias '{result.Alias}' is no longer registered to command '{result.Command}'.");
        }
    }
}