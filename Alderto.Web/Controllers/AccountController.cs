using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace Alderto.Web.Controllers
{
    [ApiController, Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("login")]
        [Authorize(AuthenticationSchemes = DiscordAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Login()
        {
            // Authorized using discord. Create JWT token.

            // Fetch the authentication result. It contains the access token to discord.
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var tokenHandler = new JwtSecurityTokenHandler();

            // Add claims to the JWT.
            var userClaims = authResult.Principal.Claims.ToList();

            // Store data for the token
            var userId = userClaims.Find(c => c.Type == ClaimTypes.NameIdentifier);
            var userDiscordToken = authResult.Properties.Items[".Token.access_token"];

            // Create the token.
            var token = tokenHandler.CreateJwtSecurityToken(
                subject: new ClaimsIdentity(new[]
                {
                    userId,
                    new Claim("discord", userDiscordToken)
                }),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Convert.FromBase64String(_configuration["JWTPrivateKey"])),
                    SecurityAlgorithms.HmacSha256Signature),
                expires: authResult.Properties.ExpiresUtc?.DateTime
            );

            var user = new
            {
                id = userId.Value,
                username = userClaims.Find(c => c.Type == ClaimTypes.Name).Value,
                discord = userDiscordToken,
                token = tokenHandler.WriteToken(token)
            };

            _logger.LogInformation($"User {User.Identity.Name} has logged in.");
            await HttpContext.SignOutAsync(); // Cookie is no longer needed. Sign out.

            // Faking a view. Sends the token to localStorage and redirecting to main webpage.
            return Content(
                "<script>" +
                    $"localStorage.setItem('user', '{ JsonConvert.SerializeObject(user) }');" +
                    "window.location.href = '/'" +
                "</script>", "text/html");
        }
    }
}