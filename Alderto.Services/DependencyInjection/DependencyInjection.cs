using System;
using Alderto.Services;
using Alderto.Services.Impl;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
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

                // Add Guild bank managers
                .AddScoped<IGuildBankManager, GuildBankManager>()
                .AddScoped<IGuildBankItemsManager, GuildBankItemsManager>()
                .AddScoped<IGuildLogger, GuildLogger>();

            return services;
        }

        /// <summary>
        /// Adds a news source for the application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static IServiceCollection AddNewsProvider(this IServiceCollection services,
            Action<NewsProviderOptions>? options = null) =>
            services.AddSingleton<INewsProvider, NewsProvider>().Configure(options);

        public sealed class NewsProviderOptions
        {
            public ulong NewsChannelId { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMessagesManager(this IServiceCollection services) =>
            services.AddScoped<IMessagesManager, MessagesManager>();
    }
}