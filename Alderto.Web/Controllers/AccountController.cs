using System;
using System.Collections.Generic;
using System.Security.Claims;
using Alderto.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

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

        [Route("some"), ActionName("Some"), Authorize]
        public IActionResult Some()
        {
            return Ok();
        }

        [Route("logout"), ActionName("LogOut")]
        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Route("login"), ActionName("LogIn")]
        public IActionResult LogIn(string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(action: "LogInCallback", controller: "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider: "Discord", redirectUrl);
            return new ChallengeResult(authenticationScheme: "Discord", properties);
        }

        [Route("login-callback"), ActionName("LogInCallback")]
        public async Task<IActionResult> SignInCallbackAsync(string remoteError = null)
        {
            if (remoteError != null)
            {
                return StatusCode(statusCode: 500, $"Error from external provider: {remoteError}");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"{info.Principal.Identity.Name} logged in with {info.LoginProvider} provider.");
                return Ok();
            }
            if (result.IsLockedOut)
            {
                return Forbid();
            }
            else
            {
                // User is not registered. Register him.
                var user = new ApplicationUser(info.Principal.Identity.Name)
                {
                    Id = ulong.Parse(info.ProviderKey)
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user, info);

                // Unsure if needed yet
                //await _userManager.AddClaimsAsync(user, new[]
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                //});
            }

            return Ok();
        }
    }
}