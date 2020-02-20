using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("account")]
    public class AccountController : ApiControllerBase
    {
        [HttpGet("login")]
        [Authorize(AuthenticationSchemes = "Discord")]
        public ActionResult Login(string? redirectUrl = null)
        {
            return Redirect(redirectUrl ?? "/");
        }
    }
}