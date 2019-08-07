using Alderto.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds access to DiscordAPI REST features on the behalf of a Bot.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="token">Bot authorization token.</param>
        public static IServiceCollection AddDiscordClient(this IServiceCollection services, string token) =>
            services.AddSingleton(new DiscordRestBot(token));

    }
}