using Alderto.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Domain
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all managers for data manipulation from the database.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        public static IServiceCollection AddAldertoDomainServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IGuildSetupService, GuildSetupService>();

            return services;
        }
    }
}