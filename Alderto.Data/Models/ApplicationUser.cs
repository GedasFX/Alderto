using Microsoft.AspNetCore.Identity;

namespace Alderto.Data.Models
{
    /// <summary>
    /// User accessing web client.
    /// Primary purpose to wrap <see cref="IdentityUser{TKey}"/>
    /// </summary>
    public class ApplicationUser : IdentityUser<ulong>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationUser"/>
        /// </summary>
        public ApplicationUser()
        { 
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationUser"/>
        /// </summary>
        /// <param name="userName">The user name</param>
        public ApplicationUser(string userName) : base(userName)
        {
        }
    }
}
