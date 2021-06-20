using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Application.Features.Currency;
using Alderto.Application.Features.Currency.Query;
using Alderto.Bot.Extensions;
using Alderto.Domain.Services;
using Discord;
using Discord.Commands;
using MediatR;

namespace Alderto.Bot.Modules
{
    [Group("currency")]
    public class CurrencyModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly IGuildSetupService _setupService;

        public CurrencyModule(IMediator mediator, IGuildSetupService setupService)
        {
            _mediator = mediator;
            _setupService = setupService;
        }

        [Command]
        public async Task HandleAsync(
            [Summary("")] string currencyName,
            [Summary("")] string action,
            params string[] tokens)
        {
            if (Context.User is not IGuildUser author)
                return;

            action = action.ToLowerInvariant();
            switch (action)
            {
                case "give":
                case "send":
                case "award":
                    await TransferCurrency(currencyName, action, tokens, author);
                    return;

                case "$":
                case "check":
                    await CheckCurrency(currencyName, tokens, author);
                    return;

                case "timely":
                    await Timely(currencyName, author);
                    return;
            }
        }

        private async Task Timely(string currencyName, IGuildUser author)
        {
            var gtrResult =
                await _mediator.Send(new GrantTimelyReward.Command(author.GuildId, author.Id, currencyName));

            var nextClaim = gtrResult.NextClaim.Days > 0
                ? $"{gtrResult.NextClaim:d}d {gtrResult.NextClaim:hh\\:mm\\:ss}"
                : $"{gtrResult.NextClaim:hh\\:mm\\:ss}";

            if (gtrResult.Success)
                await this.ReplySuccessEmbedAsync(
                    $"{author.Mention} has received {gtrResult.ReceivedAmount} {gtrResult.CurrencySymbol}!\nCan claim again in **{nextClaim}**");
            else
                await this.ReplyErrorEmbedAsync(
                    $"{author.Mention} will be able to claim more {gtrResult.CurrencySymbol} in **{nextClaim}**.");
        }

        private async Task CheckCurrency(string currencyName, IReadOnlyList<string> tokens, IGuildUser author)
        {
            if (!(tokens.Count > 0 && MentionUtils.TryParseUser(tokens[0], out var memberId)))
                memberId = author.Id;

            var wallet = await _mediator.Send(new Wallets.FindByName(author.GuildId, memberId, currencyName));

            await this.ReplyEmbedAsync(
                $"{MentionUtils.MentionUser(memberId)} has {wallet.Amount} {wallet.CurrencySymbol}");
        }

        private async Task TransferCurrency(string currencyName, string action, string[] tokens, IGuildUser author)
        {
            if (tokens.Length < 2)
                throw new ValidationException($"Amount to send and recipients are required");

            if (!int.TryParse(tokens[0], out var amount))
                throw new ValidationException($"Expected amount to transfer. Found '{tokens[0]}'");

            if (action != "award" && amount <= 0)
                throw new ValidationException("Amount to send must be positive ;)");

            if (action == "award")
                if (!author.GuildPermissions.Administrator)
                {
                    var setup = await _setupService.GetGuildSetupAsync(author.GuildId);
                    if (setup.Configuration.ModeratorRoleId == null ||
                        author.RoleIds.Contains((ulong) setup.Configuration.ModeratorRoleId))
                        throw new ValidationException("Only admins are allowed to award currency");
                }

            foreach (var token in tokens[1..])
            {
                if (!MentionUtils.TryParseUser(token, out var recipientId))
                    throw new ValidationException($"Expected discord mention. Found '{tokens}'");

                await _mediator.Send(new TransferCurrency.Command(author.GuildId, author.Id, recipientId,
                    currencyName,
                    amount));
            }

            await this.ReplySuccessEmbedAsync(
                $"Successfully sent {amount} to {string.Join(", ", tokens[1..])}");
        }
    }
}