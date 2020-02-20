using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("account")]
    public class AccountController : ApiControllerBase
    {
        [HttpGet("login")]
        [Authorize(AuthenticationSchemes = "Discord")]
        public ActionResult Login(string? returnUrl = null)
        {
            return Redirect(returnUrl ?? "/");
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}