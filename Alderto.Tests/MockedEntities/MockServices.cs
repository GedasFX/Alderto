using System;
using Alderto.Bot;
using Alderto.Bot.Lua;
using Alderto.Data;
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
                .AddDiscordSocketClient("")
                .AddLuaCommandHandler()
                .AddBotManagers()
                .BuildServiceProvider();
        }
    }
}