﻿using System;
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

        [Command("list"), Priority(1)]
        public async Task ListAsync()
        {
            if (Context.User is not IGuildUser author)
                return;

            var list = await _mediator.Send(new Currencies.List(author.GuildId, author.Id));
            var fields = list.Select(c =>
            {
                var timelyString = c.TimelyAmount > 0 && c.TimelyInterval > 0
                    ? $"✅ Timely grants {c.TimelyAmount} {c.Symbol} every {TimeSpan.FromSeconds((int) c.TimelyInterval):g}"
                    : "❌ Timely is disabled";
                return ($"[{c.Name}] {c.Symbol}",
                    $"{c.Description}```{timelyString}```");
            });

            await this.ReplyEmbedAsync($"List of currencies in {author.Guild.Name}", extra: b =>
            {
                foreach (var (name, value) in fields)
                {
                    b.AddField(name, value);
                }
            });
        }

        [Command]
        public async Task GetAsync(string currencyName)
        {
            if (Context.User is not IGuildUser author)
                return;

            var currency = await _mediator.Send(new Currencies.FindByName(author.GuildId, author.Id, currencyName));

            if (currency == null)
            {
                await this.ReplyErrorEmbedAsync("Currency not found");
                return;
            }

            var timelyString = currency.TimelyAmount > 0 && currency.TimelyInterval > 0
                ? $"✅ Timely grants {currency.TimelyAmount} {currency.Symbol} every {TimeSpan.FromSeconds((int) currency.TimelyInterval):g}"
                : "❌ Timely is disabled";

            await this.ReplyEmbedAsync($"List of currencies in {author.Guild.Name}",
                extra: b =>
                {
                    b.AddField($"[{currency.Name}] {currency.Symbol}", $"{currency.Description}```{timelyString}```");
                });
        }

        [Command, Priority(1)]
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
                case "get":
                case "info":
                case "inspect":
                    await GetAsync(currencyName);
                    return;

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

                case "history":
                case "logs":
                    await Logs(currencyName, tokens, author);
                    return;
            }
        }

        private async Task Logs(string currencyName, IReadOnlyList<string> tokens, IGuildUser author)
        {
            if (tokens.Count == 0 || !MentionUtils.TryParseUser(tokens[0], out var memberId))
                throw new EntryPointNotFoundException("Mention the user to view logs of");

            if (!(tokens.Count > 0 && int.TryParse(tokens[0], out var pageNo)))
                pageNo = 1;

            pageNo -= 1;

            var logs = await _mediator.Send(new CurrencyTransactions.List(author.GuildId, memberId,
                currencyName, pageNo));

            (string, string, string) GetSymbols(CurrencyTransactions.Dto.TransactionEntry t)
            {
                if (t.IsAward)
                {
                    if (t.SenderId == t.RecipientId)
                        return ("⏰", "👌", MentionUtils.MentionUser(t.RecipientId));

                    if (t.SenderId == memberId)
                        return ("🎁", "👉", MentionUtils.MentionUser(t.RecipientId));

                    if (t.RecipientId == memberId)
                        return ("🏆", "👈", MentionUtils.MentionUser(t.SenderId));
                }

                if (t.SenderId == memberId)
                    return ("📉", "👉", MentionUtils.MentionUser(t.RecipientId));

                if (t.RecipientId == memberId)
                    return ("📈", "👈", MentionUtils.MentionUser(t.SenderId));

                // Should never happen
                return ("❓", "❓", MentionUtils.MentionUser(memberId));
            }

            var fields = logs.Transactions.Select(t =>
            {
                var date = $"{t.Date:dd MMM yyy HH\\:mm\\:ss}";
                var (symbol, direction, otherParty) = GetSymbols(t);
                return (date, $"{symbol} **{t.Amount}** {logs.Symbol} {direction} {otherParty}");
            });

            await this.ReplyEmbedAsync($"Showing results {20 * pageNo + 1}-{20 * (pageNo + 1)}",
                $"Transaction history for currency '{logs.Name}'", extra: b =>
                {
                    foreach (var (name, value) in fields)
                    {
                        b.AddField(name, value, true);
                    }
                });
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
                    throw new ValidationException($"Expected discord mention. Found '{token}'");

                await _mediator.Send(new TransferCurrency.Command(author.GuildId, author.Id, recipientId,
                    currencyName,
                    amount, action == "award"));
            }

            await this.ReplySuccessEmbedAsync(
                $"Successfully sent {amount} to {string.Join(", ", tokens[1..])}");
        }
    }
}