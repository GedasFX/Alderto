using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Alderto.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Alderto.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        [Route("some"), Authorize]
        public IActionResult Some()
        {
            return Ok();
        }

        [Route("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        //[HttpPost]
        [Route("login")]
        [Authorize(AuthenticationSchemes = DiscordAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Login(string returnUrl = null)
        {
            // Authorized using discord. Create JWT token.
            var tokenHandler = new JwtSecurityTokenHandler();

            var userClaims = User.Claims.ToList();
            userClaims.Add(new Claim(ClaimTypes.Role, "User"));

            var token = tokenHandler.CreateJwtSecurityToken(
                subject: new ClaimsIdentity(userClaims),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Jwt:SigningSecret"])),
                    SecurityAlgorithms.HmacSha256Signature),
                expires: DateTime.UtcNow.AddDays(7)
            );

            return new OkObjectResult(tokenHandler.WriteToken(token));
        }
    }
}