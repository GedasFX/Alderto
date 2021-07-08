using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank.Dto;
using Alderto.Application.Features.Bank.Query;
using Alderto.Bot.Extensions;
using Alderto.Domain.Exceptions;
using Discord.Commands;
using MediatR;

namespace Alderto.Bot.Modules
{
    [Group, Alias("GuildBank", "GB", "GuildBanks", "Banks", "Bank")]
    public class GuildBankModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public GuildBankModule(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("list")]
        public async Task List()
        {
            var banks = await _mediator.Send(new Banks.List<BankBriefDto>(Context.Guild.Id, Context.User.Id));
            await this.ReplyEmbedAsync(banks.Aggregate(new StringBuilder(), (c, i) => c.Append($"**{i.Name,32}**\n"))
                .ToString());
        }

        [Command("items")]
        public async Task Items(string bankName)
        {
            var bank = await _mediator.Send(new Banks.Find<BankDto>(Context.Guild.Id, Context.User.Id, bankName));
            if (bank == null)
                throw new ValidationDomainException(ErrorMessage.BANK_NOT_FOUND);

            var res = bank.Contents.Aggregate(new StringBuilder(), (current, item) =>
                current.Append(
                    $"**{item.Name}**\n{(string.IsNullOrEmpty(item.Description) ? "N/A" : item.Description)}\n*qty*: {item.Quantity} @ {item.Value} ea.\n\n"));
            await this.ReplyEmbedAsync(res.ToString());
        }
    }
}
