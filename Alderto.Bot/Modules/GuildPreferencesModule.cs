using System;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Services;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
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
            public async Task Prefix()
            {
                var pref = await _guildPreferencesManager.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Prefix: **{pref.Prefix}**");
            }

            [Command("CurrencySymbol"), Alias("CS")]
            public async Task CurrencySymbol()
            {
                var pref = await _guildPreferencesManager.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Currency Symbol: **{pref.CurrencySymbol}**");
            }

            [Command("TimelyCooldown"), Alias("TimelyCD", "TCD")]
            public async Task TimelyCooldown()
            {
                var pref = await _guildPreferencesManager.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Timely Cooldown: **{TimeSpan.FromSeconds(pref.TimelyCooldown)}**");
            }

            [Command("TimelyRewardQuantity"), Alias("TimelyRQ", "TRQ")]
            public async Task TimelyRewardQuantity()
            {
                var pref = await _guildPreferencesManager.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Timely Reward Quantity: **{pref.TimelyRewardQuantity}**");
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
            public async Task Prefix(string prefix)
            {
                await _guildPreferencesManager.UpdatePreferencesAsync(Context.Guild.Id, pref => pref.Prefix = prefix);
                await this.ReplySuccessEmbedAsync($"Prefix was changed to: **{prefix}**");
            }

            [Command("CurrencySymbol"), Alias("CS")]
            public async Task CurrencySymbol(string currencySymbol)
            {
                await _guildPreferencesManager.UpdatePreferencesAsync(Context.Guild.Id, configuration => configuration.CurrencySymbol = currencySymbol);
                await this.ReplySuccessEmbedAsync($"Currency Symbol was changed to: **{currencySymbol}**");
            }

            [Command("TimelyCooldown"), Alias("TimelyCD", "TCD")]
            public async Task TimelyCooldown(int cooldown)
            {
                await _guildPreferencesManager.UpdatePreferencesAsync(Context.Guild.Id, configuration => configuration.TimelyCooldown = cooldown);
                await this.ReplySuccessEmbedAsync($"Timely Cooldown was changed to: **{TimeSpan.FromSeconds(cooldown)}**");
            }

            [Command("TimelyRewardQuantity"), Alias("TimelyRQ", "TRQ")]
            public async Task TimelyRewardQuantity(int quantity)
            {
                await _guildPreferencesManager.UpdatePreferencesAsync(Context.Guild.Id, configuration => configuration.TimelyRewardQuantity = quantity);
                await this.ReplySuccessEmbedAsync($"Timely Reward Quantity was changed to: **{quantity}**");
            }
        }
    }
}
