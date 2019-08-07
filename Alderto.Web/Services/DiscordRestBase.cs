using System.Threading.Tasks;
using Alderto.Web.Helpers;
using Alderto.Web.Models.Discord;

namespace Alderto.Web.Services
{
    public abstract class DiscordRestBase
    {
        private readonly string _authHeader;

        protected DiscordRestBase(string authHeader)
        {
            _authHeader = authHeader;
        }

        public async Task<Guild[]> GetGuildsAsync()
        {
            return await FetchAsync<Guild[]>("/users/@me/guilds");
        }

        public async Task<T> FetchAsync<T>(string path, string method = "GET", string jsonData = null)
        {
            return await DiscordApi.FetchAsync<T>(path, method, _authHeader, jsonData);
        }
    }
}