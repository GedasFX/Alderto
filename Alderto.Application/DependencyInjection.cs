using System;
using Alderto.Application.Behaviors;
using Alderto.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Application
{
    public sealed class AldertoOptions
    {
    }

    public static class DependencyInjection
    {
        public static IServiceCollection AddAlderto(this IServiceCollection collection,
            Action<AldertoOptions>? options = null)
        {
            collection
                .AddMediatR(typeof(DependencyInjection))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            collection
                .AddAutoMapper(typeof(DependencyInjection));

            collection
                .AddAldertoDomainServices();

            collection.Configure(options);

            return collection;
        }
    }
}
