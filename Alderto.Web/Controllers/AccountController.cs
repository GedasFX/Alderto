using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("account")]
    public class AccountController : ApiControllerBase
    {
        [HttpGet("login")]
        [Authorize(AuthenticationSchemes = DiscordAuthenticationDefaults.AuthenticationScheme)]
        public async Task<ActionResult> LoginAsync(string? returnUrl = null)
        {
            await HttpContext.SignInAsync("idsrv", User);

            return Redirect(returnUrl ?? "/");
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(DiscordAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}