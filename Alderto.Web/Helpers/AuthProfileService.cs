using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Alderto.Web.Services;
using Discord;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.DataProtection;

namespace Alderto.Web.Helpers
{
    public class AuthProfileService : IProfileService
    {
        private readonly IDataProtector _protector;
        private readonly DiscordHttpClient _discord;
        private readonly IDiscordClient _bot;

        public AuthProfileService(IDataProtectionProvider dpProvider, DiscordHttpClient discord, IDiscordClient bot)
        {
            _protector = dpProvider.CreateProtector(DataProtectionPurposes.DiscordToken);
            _discord = discord;
            _bot = bot;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.Caller == IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint)
            {
                var accessToken = _protector.Unprotect(context.Subject.FindFirstValue("discord"));

                var user = await _discord.GetUserAsync(accessToken);
                var userGuilds = await _discord.GetUserGuildsAsync(accessToken);

                var mutualGuilds = userGuilds.Where(userGuild => _bot.GetGuildAsync(ulong.Parse(userGuild.Id, CultureInfo.InvariantCulture)).Result != null);
                var mutualGuildsMap = new Dictionary<string, string>();
                foreach (var guild in mutualGuilds)
                {
                    mutualGuildsMap.Add(guild.Name, $"{guild.Id}:{guild.Permissions}:{guild.Owner}:{guild.Icon}");
                }

                var serializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                };

                context.IssuedClaims.Add(new Claim("user", JsonSerializer.Serialize(user, serializerOptions), "json"));
                context.IssuedClaims.Add(new Claim("user_guilds", JsonSerializer.Serialize(mutualGuildsMap, serializerOptions), "json"));
            }

            else if (context.Caller == IdentityServerConstants.ProfileDataCallers.ClaimsProviderAccessToken)
            {
                var accessToken = context.Subject.FindFirstValue("discord");
                context.IssuedClaims.Add(new Claim("discord", _protector.Protect(accessToken)));
            }
        }

        public Task IsActiveAsync(IsActiveContext context) => Task.CompletedTask;
    }
}
