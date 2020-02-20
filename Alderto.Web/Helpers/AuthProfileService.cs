using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.DataProtection;

namespace Alderto.Web.Helpers
{
    public class AuthProfileService : IProfileService
    {
        private readonly IDataProtector _protector;

        public AuthProfileService(IDataProtectionProvider dpProvider)
        {
            _protector = dpProvider.CreateProtector(DataProtectionPurposes.DiscordToken);
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.Add(new Claim("discord", _protector.Protect(context.Subject.FindFirstValue("discord"))));
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context) => Task.CompletedTask;
    }
}
