using System;
using System.Security.Claims;

namespace Alderto.Tests.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static void SetId(this ClaimsPrincipal principal, ulong id)
        {
            if (!(principal.Identity is ClaimsIdentity identity))
                throw new ArgumentException("Identity must be of type " + nameof(ClaimsIdentity));

            var prevClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (prevClaim != null)
                identity.RemoveClaim(prevClaim);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
        }
    }
}