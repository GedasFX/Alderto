using Alderto.Data.Models;
using MediatR;

namespace Alderto.Application.Features.Bank.Events
{
    public class BankItemUpdatedEvent : INotification
    {
        public GuildBankItem BankItem { get; }
        public UpdateBankItem.Command Request { get; }

        public BankItemUpdatedEvent(GuildBankItem bankItem, UpdateBankItem.Command request)
        {
            BankItem = bankItem;
            Request = request;
        }
    }
}
