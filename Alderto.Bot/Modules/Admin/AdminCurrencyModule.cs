using System.Threading.Tasks;
using Alderto.Application.Features.Currency;
using Alderto.Bot.Extensions;
using Discord;
using Discord.Commands;
using MediatR;

namespace Alderto.Bot.Modules.Admin
{
    [Group("admin currency"), RequireUserPermission(GuildPermission.Administrator)]
    public class AdminCurrencyModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public AdminCurrencyModule(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("add"), Alias("insert", "create")]
        public async Task AddCurrencyAsync(string name, string currencySymbol, [Remainder] string? description = null)
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
        public class EditCurrencyArgs
        {
            public string? Description { get; set; }
            public string? Symbol { get; set; }
            public int? TimelyAmount { get; set; }
            public int? TimelyInterval { get; set; }
            public bool? IsLocked { get; set; }
        }

        [Command("edit"), Alias("update")]
        public async Task EditCurrencyAsync(string name, EditCurrencyArgs args)
        {
            if (Context.Message.Author is not IGuildUser author)
                return;

            await _mediator.Send(new UpdateCurrency.Command(author.GuildId, author.Id, name)
            {
                Description = args.Description,
                Symbol = args.Symbol,
                TimelyAmount = args.TimelyAmount,
                TimelyInterval = args.TimelyInterval,
                IsLocked = args.IsLocked,
            });

            await this.ReplySuccessEmbedAsync($"Currency '{name}' updated successfully.");
        }

        [Command("remove"), Alias("delete")]
        public async Task RemoveCurrencyAsync(string name)
        {
            if (Context.Message.Author is not IGuildUser author)
                return;

            var currency = await _mediator.Send(new DeleteCurrency.Command(author.GuildId, author.Id, name));

            await this.ReplySuccessEmbedAsync(
                $"Currency {currency.Name} {currency.Symbol} was removed successfully. All records were cleared.");
        }
    }
}
