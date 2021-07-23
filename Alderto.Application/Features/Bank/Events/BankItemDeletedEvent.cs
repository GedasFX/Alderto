using Alderto.Data.Models;
using MediatR;

namespace Alderto.Application.Features.Bank.Events
{
    public class BankItemDeletedEvent : INotification
    {
        public GuildBankItem BankItem { get; }
        public DeleteBankItem.Command Request { get; }

        public BankItemDeletedEvent(GuildBankItem bankItem, DeleteBankItem.Command request)
        {
            BankItem = bankItem;
            Request = request;
        }
    }
}
