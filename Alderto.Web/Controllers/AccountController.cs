using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("login")]
        [Authorize(AuthenticationSchemes = DiscordAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Login()
        {
            // Authorized using discord.

            // Fetch the authentication result. It contains the access token to discord.
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var userClaims = authResult.Principal.Claims.ToList();

            var user = new
            {
                id = userClaims.Find(c => c.Type == ClaimTypes.NameIdentifier).Value,
                username = userClaims.Find(c => c.Type == ClaimTypes.Name).Value,
                token = authResult.Properties.Items[".Token.access_token"]
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.id),
                new Claim("Discord", user.token)
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            // Reload SignIn with new Cookie
            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(principal);

            // Returns the token to the creator of the window. Faking a view.
            return Content(
                "<script>" +
                    $"localStorage.setItem('user', '{ JsonConvert.SerializeObject(user) }');" +
                     "window.location.href = '/'" +
                "</script>", "text/html");
        }
    }
}