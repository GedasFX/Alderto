using Alderto.Data.Models;
using MediatR;

namespace Alderto.Application.Features.Bank.Events
{
    public class BankCreatedEvent : INotification
    {
        public GuildBank Bank { get; }
        public CreateBank.Command Request { get; }

        public BankCreatedEvent(GuildBank bank, CreateBank.Command request)
        {
            Bank = bank;
            Request = request;
        }
    }
}
