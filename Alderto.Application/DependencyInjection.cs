using Alderto.Application.Behaviors;
using Alderto.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
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
                .AddAldertoDomainServices();

            return collection;
        }

        public static IApplicationBuilder UseAlderto(this IApplicationBuilder app)
        {
            return app;
        }
    }
}