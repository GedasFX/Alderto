using Alderto.Data.Models.GuildBank;
using MediatR;

namespace Alderto.Application.Features.Bank.Events
{
    public class BankItemCreatedEvent : INotification
    {
        public GuildBankItem BankItem { get; }
        public CreateBankItem.Command Request { get; }

        public BankItemCreatedEvent(GuildBankItem bankItem, CreateBankItem.Command request)
        {
            BankItem = bankItem;
            Request = request;
        }
    }
}
