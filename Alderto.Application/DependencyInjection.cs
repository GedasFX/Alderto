using Alderto.Application.Behaviors;
using Alderto.Application.Repository;
using Alderto.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAlderto(this IServiceCollection collection)
        {
            collection
                .AddMediatR(typeof(DependencyInjection))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            collection
                .AddAutoMapper(typeof(DependencyInjection));

            collection
                .AddRepositories();

            collection
                .AddAldertoDomainServices();

            return collection;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection collection) =>
            collection
                .AddScoped<CurrencyRepository>()
                .AddScoped<CurrencyTransactionRepository>();
    }
}
