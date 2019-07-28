using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Alderto.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [Route("some"), Authorize]
        public IActionResult Some()
        {
            return Ok();
        }

        //[HttpPost]
        [Route("login")]
        [Authorize(AuthenticationSchemes = DiscordAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Login()
        {
            // Authorized using discord. Create JWT token.
            var tokenHandler = new JwtSecurityTokenHandler();

            var userClaims = User.Claims.ToList();
            userClaims.Add(new Claim(ClaimTypes.Role, "User"));

            var token = tokenHandler.CreateJwtSecurityToken(
                subject: new ClaimsIdentity(userClaims),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Jwt:SigningSecret"])),
                    SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.UtcNow.AddDays(7)
            );

            _logger.LogInformation($"User {User.Identity.Name} has logged in.");
            return Content($"<script>" +
                            $"window.opener.postMessage('{tokenHandler.WriteToken(token)}', '{Request.Scheme}://{Request.Host}{Request.PathBase}');" +
                            $"window.close();" +
                           $"</script>",
                "text/html");
        }

        //[Route("login")]
        //public IActionResult Login(string returnUrl = null)
        //{
        //    //var properties = _signInManager
        //    //    .ConfigureExternalAuthenticationProperties(
        //    //        provider: "Discord",  
        //    //        redirectUrl: Url.Action(action: nameof(LogInCallback), new { returnUrl }));

        //        return Challenge(
        //                new AuthenticationProperties { RedirectUri = Url.Action(nameof(LoginCallback), new { returnUrl }) },
        //                DiscordAuthenticationDefaults.AuthenticationScheme);
        //}
    }
}