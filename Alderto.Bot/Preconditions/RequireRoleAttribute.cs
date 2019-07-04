using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Alderto.Bot.Preconditions
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string _roleName;

        /// <summary>
        /// Checks if user has a provided role
        /// Also checks if message sent was from a guild
        /// </summary>
        /// <param name="roleName">Role name to search for</param>
        public RequireRoleAttribute(string roleName) => _roleName = roleName;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // Check if this user is a Guild User, which is the only context where roles exist
            if (context.User is SocketGuildUser gUser)
            {
                // If this command was executed by a user with the appropriate role, return a success
                if (gUser.Roles.Any(r => r.Name == _roleName))
                    // Since no async work is done, the result has to be wrapped with `Task.FromResult` to avoid compiler errors
                    return Task.FromResult(PreconditionResult.FromSuccess());
                // If it wasn't, fail
                return Task.FromResult(PreconditionResult.FromError("You do not have the appropriate roles required to run this command."));
            }

            return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
        }
    }
}
