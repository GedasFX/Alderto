using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Bot.Lua
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds a singleton instance of <see cref="ICustomCommandProvider"/> to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        public static IServiceCollection AddLuaCommandHandler(this IServiceCollection services) =>
            services.AddScoped<ICustomCommandProvider, CustomCommandProvider>();
    }
}