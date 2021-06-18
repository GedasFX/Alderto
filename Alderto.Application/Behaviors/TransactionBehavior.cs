using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using MediatR;

namespace Alderto.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        private readonly AldertoDbContext _context;

        public TransactionBehavior(AldertoDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                await _context.Database.BeginTransactionAsync(cancellationToken);
                var response = await next();
                await _context.Database.CommitTransactionAsync(cancellationToken);
                return response;
            }
            catch
            {
                await _context.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}