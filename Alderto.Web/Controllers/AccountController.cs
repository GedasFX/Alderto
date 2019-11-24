using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Alderto.Web.Services;
using Microsoft.AspNetCore.Authentication;

namespace Alderto.Web.Controllers
{
    [Route("account")]
    public class AccountController : ApiControllerBase
    {
        private readonly AuthService _authService;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        [Authorize(AuthenticationSchemes = DiscordAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult Login()
        {
            return Redirect("/");
        }

        [HttpPost("login"), Authorize("TokenRefresh")]
        public async Task<ActionResult<string>> RefreshJwtAsync()
        {
            var authResult = await HttpContext.AuthenticateAsync("TokenRefresh");
            var jwt = await _authService.GenerateJwtAsync(authResult.Ticket);

            return jwt;
        }

        [HttpPost("logout"), Authorize("TokenRefresh")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync("TokenRefresh");
            return Ok();
        }
    }
}