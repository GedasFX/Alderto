using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
using Alderto.Services;
using Discord;
using Discord.Commands;
using Discord.Net;

namespace Alderto.Bot.Modules
{
    [RequireRole("Admin")]
    public class UserManagementModule : ModuleBase<SocketCommandContext>
    {
        private readonly IGuildMemberManager _guildMemberManager;
        private readonly IGuildPreferencesProvider _guildPreferencesProvider;

        public UserManagementModule(IGuildMemberManager guildMemberManager, IGuildPreferencesProvider guildPreferencesProvider)
        {
            _guildMemberManager = guildMemberManager;
            _guildPreferencesProvider = guildPreferencesProvider;
        }

        [Command("Accept")]
        [Summary("Changes the username of a user and gives him a Member role")]
        [RequireBotPermission(GuildPermission.ManageNicknames | GuildPermission.ManageRoles)]
        public async Task Accept(
            [Summary("User")] IGuildUser user,
            [Remainder] [Summary("Nickname. Does not change nickname if none was specified.")] string nickname = null)
        {
            var pref = await _guildPreferencesProvider.GetPreferencesAsync(user.GuildId);

            try
            {
                await _guildMemberManager.AcceptMemberAsync(user, nickname,
                    Context.Guild.Roles.SingleOrDefault(r => r.Id == pref.AcceptedMemberRoleId));
            }
            catch (HttpException)
            {
                await this.ReplyErrorEmbedAsync(
                    "The bot was unable to modify the person. Is the bot role above the person you are trying to accept?");
                return;
            }

            await this.ReplySuccessEmbedAsync($"{user.Mention} has accepted. Welcome!");
        }
    }
}
