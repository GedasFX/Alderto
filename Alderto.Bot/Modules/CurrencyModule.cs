using System;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Application.Features.Currency;
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
        public async Task TransferAsync(
            [Summary("")] string currencyName,
            [Summary("")] string action,
            [Summary("Amount of currency")] int amount,
            [Summary("Users to interact with")] params IGuildUser[] recipients)
        {
            if (Context.User is not IGuildUser user)
                return;

            switch (action.ToLowerInvariant())
            {
                case "give":
                case "send":
                    if (amount <= 0)
                        throw new EntryPointNotFoundException("Please send more than nothing :)");
                    foreach (var recipient in recipients)
                    {
                        await _mediator.Send(new TransferCurrency.Command(user.GuildId, user.Id, recipient.Id,
                            currencyName,
                            amount));
                        await Task.Delay(50);
                    }

                    break;
                case "award":
                    if (!user.GuildPermissions.Administrator)
                    {
                        var setup = await _setupService.GetGuildSetupAsync(user.GuildId);
                        if (setup.Configuration.ModeratorRoleId == null ||
                            user.RoleIds.Contains((ulong) setup.Configuration.ModeratorRoleId))
                            throw new EntryPointNotFoundException("Only admins are allowed to use this command");
                    }

                    foreach (var recipient in recipients)
                    {
                        await _mediator.Send(new TransferCurrency.Command(user.GuildId, user.Id, recipient.Id,
                            currencyName,
                            amount,
                            true));
                        await Task.Delay(50);
                    }

                    break;
                default:
                    throw new EntryPointNotFoundException("The only valid actions are give/send or award");
            }

            await this.ReplySuccessEmbedAsync(
                $"Successfully sent {amount} to {string.Join(", ", recipients.Select(r => r.Mention))}");
        }

        [Command]
        [Summary("Checks the amount of points a given user has.")]
        public async Task CheckAsync(
            [Summary("")] string currencyName,
            [Summary("")] string action,
            [Summary("Person to check. If no user was provided, checks personal points.")]
            IGuildUser? user = null)
        {
            user ??= (IGuildUser) Context.User;

            switch (action.ToLowerInvariant())
            {
                case "check":
                case "$":
                    var wallet = await _mediator.Send(new GetWallet.QueryByName(user.GuildId, user.Id, currencyName));
                    await this.ReplyEmbedAsync($"{user.Mention} has {wallet.Amount} {wallet.CurrencySymbol}");
                    break;
            }
        }

        [Command]
        [Summary("Grants a timely currency reward.")]
        public async Task Timely()
        {
            // var user = (IGuildUser) Context.User;
            // var dbUser = (await _guildMemberManager.GetGuildMemberAsync(user.GuildId, user.Id))!;
            //
            // var preferences = await _guildPreferences.GetPreferencesAsync(user.GuildId);
            // var timelyCooldown = preferences.TimelyCooldown;
            // var currencySymbol = preferences.CurrencySymbol;
            // var timelyAmount = preferences.TimelyRewardQuantity;
            //
            // var timeRemaining = await _currencyManager.GrantTimelyRewardAsync(dbUser, timelyAmount, timelyCooldown);
            //
            // // If null - points were given out. Otherwise its time remaining until next claim.
            // if (timeRemaining != null)
            // {
            //     await this.ReplyErrorEmbedAsync(
            //         $"{user.Mention} will be able to claim more {currencySymbol} in **{timeRemaining}**.");
            //     return;
            // }
            //
            // // Points were given out.
            // await this.ReplySuccessEmbedAsync(
            //     ($"{user.Mention} was given {timelyAmount} {currencySymbol}. New total: **{dbUser.CurrencyCount}**."));
        }

        [Group("manage"), RequireUserPermission(GuildPermission.Administrator)]
        public abstract class Manage : ModuleBase<SocketCommandContext>
        {
            private readonly IMediator _mediator;

            public Manage(IMediator mediator)
            {
                _mediator = mediator;
            }

            [Command("create")]
            public async Task CreateCurrencyAsync(string name, string currencySymbol, [Remainder] string description)
            {
                if (Context.Message.Author is not IGuildUser author)
                    return;

                await _mediator.Send(new CreateCurrency.Command(author.GuildId, author.Id, name, currencySymbol)
                {
                    Description = description
                });

                await this.ReplySuccessEmbedAsync($"Currency {name} {currencySymbol} created successfully.");
            }


            [NamedArgumentType]
            public abstract class EditCurrencyArgs
            {
                public string? Description { get; init; }
                public string? CurrencySymbol { get; init; }
            }

            [Command("edit")]
            public async Task EditCurrencyAsync(string name, EditCurrencyArgs args)
            {
                if (Context.Message.Author is not IGuildUser author)
                    return;

                await _mediator.Send(new EditCurrency.Command(author.GuildId, author.Id, name)
                {
                    Description = args.Description,
                    CurrencySymbol = args.CurrencySymbol
                });

                await this.ReplySuccessEmbedAsync($"Currency {name} updated successfully.");
            }

            [Command("remove")]
            public async Task RemoveCurrencyAsync(string name)
            {
                if (Context.Message.Author is not IGuildUser author)
                    return;

                var currency = await _mediator.Send(new RemoveCurrency.Command(author.GuildId, author.Id, name));

                await this.ReplySuccessEmbedAsync(
                    $"Currency {currency.Name} {currency.CurrencySymbol} was removed successfully. All records were cleared.");
            }
        }
    }
}