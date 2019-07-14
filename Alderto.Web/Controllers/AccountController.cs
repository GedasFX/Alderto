using Alderto.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authorization;

namespace Alderto.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [Route("some"), Authorize]
        public IActionResult Some()
        {
            return Ok();
        }

        [Route("logout"), Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LogIn(string returnUrl = null)
        {
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(
                    provider: "Discord",
                    redirectUrl: Url.Action(action: nameof(LogInCallback), new { returnUrl }));

            return Challenge(properties, DiscordAuthenticationDefaults.AuthenticationScheme);
        }

        [Route("login-callback")]
        public async Task<IActionResult> LogInCallback(string returnUrl = null)
        {
            // If return url was not specified, just return 200.
            var onSuccess = returnUrl == null ? (IActionResult)Ok() : LocalRedirect(returnUrl);

            // Collect information from the external login provider.
            var info = await _signInManager.GetExternalLoginInfoAsync();

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"{info.Principal.Identity.Name} logged in with {info.LoginProvider} provider.");
                return onSuccess;
            }

            // User is not registered. Register him.
            var user = new ApplicationUser(info.Principal.Identity.Name)
            {
                Id = ulong.Parse(info.ProviderKey)
            };
            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, info);

            // Retry sign in after user was registered.
            result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"{info.Principal.Identity.Name} was registered and logged in with {info.LoginProvider} provider.");
                return onSuccess;
            }

            // User failed to register. Internal error.
            return StatusCode(500);
        }
    }
}