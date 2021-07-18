using Alderto.Data.Models.GuildBank;
using MediatR;

namespace Alderto.Application.Features.Bank.Events
{
    public class BankDeletedEvent : INotification
    {
        public GuildBank Bank { get; }
        public DeleteBank.Command Request { get; }

        public BankDeletedEvent(GuildBank bank, DeleteBank.Command request)
        {
            Bank = bank;
            Request = request;
        }
    }
}
