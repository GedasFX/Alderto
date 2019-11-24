using Alderto.Web.Models.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Alderto.Web.Services
{
    public class AuthService
    {
        private readonly DiscordHttpClient _discordApi;
        private readonly byte[] _jwtPrivateKey;

        public AuthService(IConfiguration config, ILogger<AuthService> logger, DiscordHttpClient discordApi)
        {
            //_context = context;
            _discordApi = discordApi;
            _jwtPrivateKey = Convert.FromBase64String(config["JWTPrivateKey"]);
        }

        public async Task<string> GenerateJwtAsync(AuthenticationTicket ticket)
        {
            var discordAccessToken = ticket.Properties.GetTokenValue("access_token");
            var guilds = await _discordApi.GetUserGuildsAsync(discordAccessToken);

            // Add claims to the JWT.
            var userClaims = ticket.Principal.Claims.Concat(GenerateGuildClaims(guilds));

            // Create the token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(
                subject: new ClaimsIdentity(userClaims),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_jwtPrivateKey), SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.UtcNow.AddMinutes(5)
            );

            return tokenHandler.WriteToken(token);
        }

        private static IEnumerable<Claim> GenerateGuildClaims(IEnumerable<DiscordApiGuild> guilds)
        {
            foreach (var guild in guilds)
            {
                yield return new Claim($"g_{guild.Id}", $"{((guild.Permissions & (1 << 2)) != 0 ? 'T' : 'F')};{guild.Name}");
            }
        }
    }
}
