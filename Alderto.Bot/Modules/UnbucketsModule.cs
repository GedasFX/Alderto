using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    // TODO: Remove this temporary module.
    [Group("Unbuckets"), Alias("Ub")]
    [RequireRole("Admin")]
    public class UnbucketsModule : ModuleBase<SocketCommandContext>
    {
        [Command("Accept")]
        [Summary("Changes the username of a user and gives him a Member role")]
        [RequireBotPermission(GuildPermission.ManageNicknames | GuildPermission.ManageRoles)]
        public async Task AcceptAsync([Summary("User")] IGuildUser user,
            [Remainder] [Summary("Username")] string username)
        {
            await user.ModifyAsync(u => u.Nickname = username ?? user.Username);
            await user.AddRoleAsync(Context.Guild.Roles.FirstOrDefault(r => r.Name == "Member"));

            await this.ReplySuccessEmbedAsync($"{Context.Message.Author.Mention} has accepted {user.Mention}.");
        }
    }
}
