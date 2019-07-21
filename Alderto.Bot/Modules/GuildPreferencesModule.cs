using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Services;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [Group("Config"), Alias("Preferences", "Pref", "Cfg")]
    public class GuildPreferencesModule : ModuleBase<SocketCommandContext>
    {
        [Group("Get")]
        public class GetPreferenceModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildPreferencesProvider _guildPreferencesProvider;

            public GetPreferenceModule(IGuildPreferencesProvider guildPreferencesProvider)
            {
                _guildPreferencesProvider = guildPreferencesProvider;
            }

            [Command("Prefix")]
            public async Task GetPrefix()
            {
                var pref = await _guildPreferencesProvider.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Prefix: **{pref.Prefix}**");
            }
        }

        [Group("Set")]
        public class SetPreferenceModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildPreferencesProvider _guildPreferencesProvider;

            public SetPreferenceModule(IGuildPreferencesProvider guildPreferencesProvider)
            {
                _guildPreferencesProvider = guildPreferencesProvider;
            }

            [Command("Prefix")]
            public async Task SetPrefix(string prefix)
            {
                var guildId = Context.Guild.Id;
                await _guildPreferencesProvider.UpdatePreferencesAsync(guildId, pref => pref.Prefix = prefix);
                await this.ReplySuccessEmbedAsync($"Prefix was changed to: **{prefix}**");
            }
        }
    }
}
