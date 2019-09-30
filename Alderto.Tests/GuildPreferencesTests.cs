using System.Threading.Tasks;
using Alderto.Services;
using Alderto.Tests.MockedEntities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Alderto.Tests
{
    public class GuildPreferencesTests
    {
        private readonly IGuildPreferencesProvider _provider;

        public GuildPreferencesTests()
        {
            _provider = MockServices.ServiceProvider.GetService<IGuildPreferencesProvider>();
        }

        [Fact]
        public async Task GetPreferences()
        {
            // Get preferences. Should be default
            var pref = await _provider.GetPreferencesAsync(1);

            // Default preferences are marked with guild id 0
            Assert.Equal((ulong)0, pref.GuildId);

            await _provider.UpdatePreferencesAsync(guildId: 1, configuration => { configuration.CurrencySymbol = "test"; });

            // Should update as object reference is the same.
            Assert.NotEqual((ulong)0, pref.GuildId);

            // Check if currency symbol saved.
            Assert.Equal("test", pref.CurrencySymbol);
        }
    }
}