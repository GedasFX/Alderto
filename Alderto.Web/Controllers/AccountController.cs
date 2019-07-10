using Alderto.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Alderto.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
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

        [Route("some"), ActionName("Some")]
        public IActionResult Some()
        {
            return Ok();
        }

        [Route("logout"), ActionName("LogOut"), AllowAnonymous]
        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Route("login"), ActionName("LogIn")]
        public IActionResult LogIn()
        {
            var user = User;
            // AuthorizeAttribute ensures login procedure.
            // If code inside this method was reached, the person is logged in.
            return Ok();
        }

        [Route("login-callback"), ActionName("LogInCallback")]
        public async Task<IActionResult> LogInCallbackAsync(string remoteError = null, string returnUrl = null)
        {
            // Return URL cannot be null or empty. If it is, redirect to root.
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "/";

            // Should never trigger.
            if (remoteError != null)
            {
                // Todo: better solution
                return StatusCode(statusCode: 500, $"Error from external provider: {remoteError}");
            }

            // Collect information from the external login provider.
            var info = await _signInManager.GetExternalLoginInfoAsync();

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"{info.Principal.Identity.Name} logged in with {info.LoginProvider} provider.");
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                // Todo: better solution
                return Forbid();
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
                _logger.LogInformation($"{info.Principal.Identity.Name} logged in with {info.LoginProvider} provider.");
                return LocalRedirect(returnUrl);
            }

            // User failed to register. Internal error.
            return StatusCode(500);
        }
    }
}