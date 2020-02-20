using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Alderto.Web.Services;
using Microsoft.AspNetCore.Authentication;

namespace Alderto.Web.Controllers
{
    [Route("account")]
    public class AccountController : ApiControllerBase
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        [Authorize(AuthenticationSchemes = "Discord")]
        public ActionResult Login()
        {
            return Redirect("/");
        }

        [HttpPost("login"), Authorize(AuthenticationSchemes = "RefreshToken")]
        public async Task<ActionResult<string>> RefreshJwtAsync()
        {
            var authResult = await HttpContext.AuthenticateAsync("RefreshToken");
            var jwt = await _authService.GenerateJwtAsync(authResult.Ticket);

            return jwt;
        }

        [HttpPost("logout"), AllowAnonymous]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync("TokenRefresh");
            return Ok();
        }
    }
}