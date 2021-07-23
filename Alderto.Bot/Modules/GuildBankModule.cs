using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank.Dto;
using Alderto.Bot.Extensions;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Alderto.Domain.Exceptions;
using AutoMapper;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Modules
{
    [Group, Alias("GuildBank", "GB", "GuildBanks", "Banks", "Bank")]
    public class GuildBankModule : ModuleBase<SocketCommandContext>
    {
        private readonly AldertoDbContext _context;
        private readonly IMapper _mapper;

        public GuildBankModule(AldertoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Command("list")]
        public async Task List()
        {
            var banks = await _mapper.ProjectTo<BankBriefDto>(_context.GuildBanks.ListItems(Context.Guild.Id))
                .ToListAsync();
            await this.ReplyEmbedAsync(banks.Aggregate(new StringBuilder(), (c, i) => c.Append($"**{i.Name,32}**\n"))
                .ToString());
        }

        [Command("items")]
        public async Task Items(string bankName)
        {
            var bank = await _mapper.ProjectTo<BankDto>(_context.GuildBanks.Include(b => b.Contents)
                .FindItem(Context.Guild.Id, bankName)).SingleOrDefaultAsync();
            if (bank == null)
                throw new ValidationDomainException(ErrorMessage.BANK_NOT_FOUND);

            var res = bank.Contents.Aggregate(new StringBuilder(), (current, item) =>
                current.Append(
                    $"**{item.Name}**\n{(string.IsNullOrEmpty(item.Description) ? "N/A" : item.Description)}\n*qty*: {item.Quantity} @ {item.Value} ea.\n\n"));
            await this.ReplyEmbedAsync(res.ToString());
        }
    }
}
