using System;
using Alderto.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Tests.MockedEntities
{
    public class MockServiceProvider
    {
        public IServiceProvider ServiceProvider { get; }

        public MockServiceProvider()
        {
            ServiceProvider = new ServiceCollection()
                .AddDbContext<IAldertoDbContext, MockDbContext>(ServiceLifetime.Singleton)
                .BuildServiceProvider();
        }
    }
}