using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Alderto.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        private readonly AldertoDbContext _context;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(AldertoDbContext context, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // Skip creating a transaction if one is already in place
            if (_context.Database.CurrentTransaction != null)
                return await next();

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                _logger.Log(LogLevel.Information, "=== Begin transaction {TransactionId}  ===",
                    transaction.TransactionId);

                var response = await next();

                await transaction.CommitAsync(cancellationToken);
                _logger.Log(LogLevel.Information, "=== Commit transaction {TransactionId}  ===",
                    transaction.TransactionId);

                return response;
            }
            catch
            {
                // If error originated from the database side, transaction was already rolled back.
                if (_context.Database.CurrentTransaction != null)
                    await _context.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}