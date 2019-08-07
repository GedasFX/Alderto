using Alderto.Bot.Services;
using Alderto.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Bot
{
    public static class BotServicesExtensions
    {
        public static IServiceCollection AddBotServices(this IServiceCollection services)
        {
            services
                // Add discord socket client
                .AddDiscordClient(LogSeverity.Debug)

                // Add command handling services
                .AddCommandService(RunMode.Sync, ignoreExtraArgs: true)
                .AddCommandHandler()

                // Add managers for various bot modules.
                .AddBotManagers()

                // Add Lua command provider.
                .AddLuaCommandHandler();

            return services;
        }

        /// <summary>
        /// Adds a singleton instance of <see cref="DiscordSocketClient"/> to the service collection. Can specify a Log Level.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        /// <param name="logSeverity">[Optional] Log severity. Defaults to <see cref="LogSeverity.Info"/>.</param>
        public static IServiceCollection AddDiscordClient(this IServiceCollection services, LogSeverity logSeverity = LogSeverity.Info) =>
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = logSeverity
            }));

        /// <summary>
        /// Adds a singleton instance of <see cref="CommandService"/> to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        /// <param name="defaultRunMode">Default run mode. Defaults to <see cref="RunMode.Sync"/>.</param>
        /// <param name="ignoreExtraArgs">Ignore extra arguments. Set to false if bot should ignore too many parameters.</param>
        public static IServiceCollection AddCommandService(this IServiceCollection services, RunMode defaultRunMode = RunMode.Sync, bool ignoreExtraArgs = false) =>
            services.AddSingleton(new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = defaultRunMode,
                IgnoreExtraArgs = ignoreExtraArgs
            }));

        /// <summary>
        /// Adds a singleton instance of <see cref="ICommandHandler"/> to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        public static IServiceCollection AddCommandHandler(this IServiceCollection services) =>
            services.AddSingleton<ICommandHandler, CommandHandler>();

        /// <summary>
        /// Adds a singleton instance of <see cref="Lua.ICustomCommandProvider"/> to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        public static IServiceCollection AddLuaCommandHandler(this IServiceCollection services) =>
            services.AddScoped<Lua.ICustomCommandProvider, Lua.CustomCommandProvider>();

        /// <summary>
        /// Adds a bunch of managers for various bot Modules activities to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        public static IServiceCollection AddBotManagers(this IServiceCollection services)
        {
            services

                // Add User provider
                .AddScoped<IGuildMemberManager, GuildMemberManager>()

                // Add providers for various bot activities
                .AddScoped<IGuildPreferencesManager, GuildPreferencesManager>()
                .AddScoped<ICurrencyManager, CurrencyManager>()
                .AddScoped<IGuildBankManager, GuildBankManager>();

            return services;
        }
    }
}