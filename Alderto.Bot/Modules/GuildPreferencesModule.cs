using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Services;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Config"), Alias("Preferences", "Pref", "Cfg")]
    public class GuildPreferencesModule : ModuleBase<SocketCommandContext>
    {
        [Group("Get")]
        public class GetPreferenceModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildPreferencesManager _guildPreferencesManager;

            public GetPreferenceModule(IGuildPreferencesManager guildPreferencesManager)
            {
                _guildPreferencesManager = guildPreferencesManager;
            }

            [Command("Prefix")]
            public async Task GetPrefix()
            {
                var pref = await _guildPreferencesManager.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Prefix: **{pref.Prefix}**");
            }
        }

        [Group("Set")]
        public class SetPreferenceModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildPreferencesManager _guildPreferencesManager;

            public SetPreferenceModule(IGuildPreferencesManager guildPreferencesManager)
            {
                _guildPreferencesManager = guildPreferencesManager;
            }

            [Command("Prefix")]
            public async Task SetPrefix(string prefix)
            {
                var guildId = Context.Guild.Id;
                await _guildPreferencesManager.UpdatePreferencesAsync(guildId, pref => pref.Prefix = prefix);
                await this.ReplySuccessEmbedAsync($"Prefix was changed to: **{prefix}**");
            }
        }
    }
}
