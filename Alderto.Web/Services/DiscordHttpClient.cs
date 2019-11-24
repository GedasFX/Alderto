using Alderto.Web.Models.Discord;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alderto.Web.Services
{
    public class DiscordHttpClient
    {
        private static readonly Uri UserUri = new Uri("https://discordapp.com/api/v6/users/@me");
        private static readonly Uri UserGuildsUri = new Uri("https://discordapp.com/api/v6/users/@me/guilds");

        private readonly HttpClient _httpClient;

        public DiscordHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<DiscordApiUser> GetUserAsync(string discordAccessToken)
            => GetResourceAsync<DiscordApiUser>(discordAccessToken, UserUri);

        public Task<IEnumerable<DiscordApiGuild>> GetUserGuildsAsync(string discordAccessToken)
            => GetResourceAsync<IEnumerable<DiscordApiGuild>>(discordAccessToken, UserGuildsUri);

        private async Task<T> GetResourceAsync<T>(string discordAccessToken, Uri uri)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", discordAccessToken);

            var response = await _httpClient.SendAsync(request);

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
