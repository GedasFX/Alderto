using Alderto.Data.Models;
using MediatR;

namespace Alderto.Application.Features.Bank.Events
{
    public class BankUpdatedEvent : INotification
    {
        public GuildBank Bank { get; }
        public UpdateBank.Command Request { get; }

        public BankUpdatedEvent(GuildBank bank, UpdateBank.Command request)
        {
            Bank = bank;
            Request = request;
        }
    }
}
