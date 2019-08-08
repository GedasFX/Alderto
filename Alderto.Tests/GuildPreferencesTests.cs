using System.Threading.Tasks;
using Alderto.Services;
using Alderto.Tests.MockedEntities;
using Xunit;

namespace Alderto.Tests
{
    public class GuildPreferencesTests
    {
        private readonly IGuildPreferencesProvider _provider;

        public GuildPreferencesTests()
        {
            _provider = new GuildPreferencesProvider(new MockServiceProvider().ServiceProvider);
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

            // Check if id update works
            await _provider.UpdatePreferencesAsync(guildId: 1, configuration => { configuration.GuildId = 44444; });
            Assert.Equal((ulong)1, pref.GuildId);
        }
    }
}