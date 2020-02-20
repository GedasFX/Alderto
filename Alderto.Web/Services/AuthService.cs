using Alderto.Web.Models.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Alderto.Web.Services
{
    public class AuthService
    {
        private readonly DiscordHttpClient _discordApi;
        private readonly IDataProtector _protector;
        private readonly byte[] _jwtPrivateKey;

        public AuthService(IConfiguration config, DiscordHttpClient discordApi, IDataProtectionProvider protector)
        {
            //_context = context;
            _discordApi = discordApi;
            _protector = protector.CreateProtector("DiscordToken");
            _jwtPrivateKey = Convert.FromBase64String(config["JWTPrivateKey"]);
        }

        public async Task<string> GenerateJwtAsync(string discordAccessToken, ICollection<Claim> userClaims)
        {
            // Get the guilds user is in.
            var guilds = await _discordApi.GetUserGuildsAsync(discordAccessToken);

            // Add claims to the JWT.
            userClaims.Add(GenerateGuildsClaim(guilds));
            userClaims.Add(new Claim("discord", _protector.Protect(discordAccessToken)));

            // Create the token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(
                subject: new ClaimsIdentity(userClaims),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_jwtPrivateKey), SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.UtcNow.AddMinutes(5)
            );

            return tokenHandler.WriteToken(token);
        }

        public Task<string> GenerateJwtAsync(AuthenticationTicket ticket)
            => GenerateJwtAsync(ticket.Properties.GetTokenValue("access_token"), ticket.Principal.Claims.ToList());

        private static Claim GenerateGuildsClaim(IEnumerable<DiscordApiGuild> guilds)
        {
            var sb = new StringBuilder();
            foreach (var guild in guilds)
            {
                if ((guild.Permissions & (1 << 2)) != 0)
                    sb.Append($"{guild.Id};");
            }

            // If is admin in at least 1 guild.
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            return new Claim($"admin_of", sb.ToString());
        }
    }
}
