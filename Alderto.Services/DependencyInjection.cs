using Alderto.Services.GuildBankManagers;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Services
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all managers for data manipulation from the database.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        public static IServiceCollection AddBotManagers(this IServiceCollection services)
        {
            services

                // Add User provider
                .AddScoped<IGuildMemberManager, GuildMemberManager>()

                // Add providers for various bot activities
                .AddSingleton<IGuildPreferencesProvider, GuildPreferencesProvider>()
                .AddScoped<ICurrencyManager, CurrencyManager>()

                // Add Guild bank managers
                .AddScoped<IGuildBankManager, GuildBankManager>()
                .AddScoped<IGuildBankContentsManager, GuildBankContentsManager>()
                .AddScoped<IGuildLogger, GuildLogger>();

            return services;
        }
    }
}