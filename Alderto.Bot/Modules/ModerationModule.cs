using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Preconditions;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        [Command("Accept")]
        [Summary("Changes the username of a user and gives him a Member role")]
        [RequireRole("Admin")]
        public async Task AcceptAsync([Summary("User")] IGuildUser user,
           [Remainder] [Summary("Username")] string username)
        {
            await user.ModifyAsync(u => u.Nickname = username);
            await user.AddRoleAsync(Context.Guild.Roles.FirstOrDefault(r => r.Name == "Member"));
            await ReplyAsync($"```User {user.Username}#{user.Discriminator} has been accepted.```");
        }
    }
}
