using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Alderto.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [Route("[action]"), ActionName("SignIn")]
        public IActionResult SignIn(string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("LogInCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider: "Discord", redirectUrl: redirectUrl);
            return new ChallengeResult("Discord", properties);
        }

        [Route("[action]"), ActionName("LoginCallback")]
        public async Task<string> LogInCallbackAsync(string remoteError = null)
        {
            if (remoteError != null)
            {
                return $"Error from external provider: {remoteError}";
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return "Error loading external login information.";
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"{info.Principal.Identity.Name} logged in with {info.LoginProvider} provider.");
                return "success";
            }
            if (result.IsLockedOut)
            {
                return "lockedout";
            }
            else
            {
                // User is not registered. Register him.
                //await _userManager.CreateAsync(new IdentityUser(info.Principal.Identity.Name) { });
            }

            return "";
        }
    }
}