using Alderto.Application.Behaviors;
using Alderto.Application.Features.Currency;
using Alderto.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAlderto(this IServiceCollection collection)
        {
            collection.AddMediatR(typeof(DependencyInjection));
            collection.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            collection.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            collection.AddAutoMapper(typeof(DependencyInjection));

            return collection;
        }

        public static IApplicationBuilder UseAlderto(this IApplicationBuilder app)
        {
            return app;
        }
    }
}