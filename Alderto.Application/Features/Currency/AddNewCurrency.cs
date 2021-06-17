using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Alderto.Application.Features.Currency
{
    public class AddNewCurrency
    {
        public class Command : IRequest
        {
        }

        public class Response
        {
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}