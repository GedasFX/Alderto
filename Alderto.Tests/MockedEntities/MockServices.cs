using System;
using Alderto.Data;
using Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Tests.MockedEntities
{
    public static class MockServices
    {
        public static IServiceProvider ServiceProvider { get; }

#pragma warning disable IDE0067 // Dispose objects before losing scope
        public static IServiceProvider ScopedServiceProvider => ServiceProvider.CreateScope().ServiceProvider;
#pragma warning restore IDE0067 // Dispose objects before losing scope

        static MockServices()
        {
            ServiceProvider = new ServiceCollection()
                .AddDbContext<AldertoDbContext>(builder => builder.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .AddSingleton<IDiscordClient, MockDiscordClient>()
                .AddBotManagers()
                .AddMessagesManager()
                .BuildServiceProvider();
        }
    }
}