using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Alderto.Web.Models.Discord;
using Newtonsoft.Json;

namespace Alderto.Web.Helpers
{
    public class DiscordApi
    {
        /// <summary>
        /// Generates a <see cref="WebRequest"/> to the Discord API.
        /// </summary>
        /// <param name="path">Path to the API. Start with /. Example: "/users/@me"</param>
        /// <param name="method">Method to use. Defaults to HttpGet</param>
        /// <param name="authHeader">Authorization header. Use $"Bearer {token}" for users or $"Bot {token}" for bots.</param>
        /// <param name="jsonData">Data to send. Is needed for POST requests.</param>
        /// <returns></returns>
        public static async Task<WebRequest> CreateRequestAsync(string path, string method = "GET", string authHeader = null, string jsonData = null)
        {
            // First create the request.
            var req = WebRequest.Create("https://discordapp.com/api" + path);

            // Apply the default headers.
            req.Method = method;
            req.ContentType = "application/json; charset=utf-8";

            // Add Authorization Token
            if (authHeader != null)
                req.Headers.Add(HttpRequestHeader.Authorization, authHeader);

            // Add the data.
            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                using (var stream = new StreamWriter(req.GetRequestStream()))
                {
                    await stream.WriteAsync(jsonData);
                }
            }

            return req;
        }

        /// <summary>
        /// Fetches data from Discord API. Returns default on unsuccessful results.
        /// </summary>
        /// <typeparam name="T">Type of data expected.</typeparam>
        /// <param name="path">Path to the API. Start with /. Example: "/users/@me"</param>
        /// <param name="method">Method to use. Defaults to HttpGet</param>
        /// <param name="authHeader">Authorization header. Use $"Bearer {token}" for users or $"Bot {token}" for bots.</param>
        /// <param name="jsonData">Data to send. Is needed for POST requests.</param>
        /// <returns>The data from the given API resource. Default if it was unsuccessful.</returns>
        public static async Task<T> FetchAsync<T>(string path, string method = "GET", string authHeader = null, string jsonData = null)
        {
            var request = await CreateRequestAsync(path, method, authHeader, jsonData);

            WebResponse response;
            try
            {
                response = await request.GetResponseAsync();
            }
            catch (Exception)
            {
                return default;
            }

            string output;
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                output = await stream.ReadToEndAsync();
            }

            return JsonConvert.DeserializeObject<T>(output);
        }

        /// <summary>
        /// Contacts DiscordAPI and checks if user has Admin Permission in a given guild.
        /// </summary>
        /// <param name="guildId">Id of guild to check.</param>
        /// <param name="authHeader">Authorization header. Use $"Bearer {token}" for users or $"Bot {token}" for bots.</param>
        /// <returns>True if user is confirmed to be Admin</returns>
        public static async Task<bool> VerifyAdminAsync(ulong guildId, string authHeader)
        {
            var data = await FetchAsync<DiscordGuild[]>($"/users/@me/guilds?after={guildId - 1}&limit=1", authHeader: authHeader);

            // This should only work if there is 1 and only 1 guild. Do not confirm admin rights on 0 or many guilds. Possible attack.
            var guild = data?.SingleOrDefault();
            if (guild == null)
            {
                return false;
            }

            var permissions = guild.Permissions;

            const int administratorBit = 2;
            return (permissions & (1 << administratorBit)) != 0;
        }
    }
}