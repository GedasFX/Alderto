using System;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Services;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    [Group("Config"), Alias("Preferences", "Pref", "Cfg")]
    public class GuildPreferencesModule : ModuleBase<SocketCommandContext>
    {
        [Group, Alias("Get")]
        public class GetPreferenceModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildPreferencesProvider _guildPreferencesProvider;

            public GetPreferenceModule(IGuildPreferencesProvider guildPreferencesProvider)
            {
                _guildPreferencesProvider = guildPreferencesProvider;
            }

            [Command("Prefix")]
            [Summary("Gets the guild's set prefix.")]
            public async Task Prefix()
            {
                var pref = await _guildPreferencesProvider.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Prefix: **{pref.Prefix}**");
            }

            [Command("CurrencySymbol"), Alias("CS")]
            [Summary("Gets the guild's set currency symbol.")]
            public async Task CurrencySymbol()
            {
                var pref = await _guildPreferencesProvider.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Currency Symbol: **{pref.CurrencySymbol}**");
            }

            [Command("TimelyCooldown"), Alias("TimelyCD", "TCD")]
            [Summary("Gets the guild's set timely cooldown.")]
            public async Task TimelyCooldown()
            {
                var pref = await _guildPreferencesProvider.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Timely Cooldown: **{TimeSpan.FromSeconds(pref.TimelyCooldown)}**");
            }

            [Command("TimelyRewardQuantity"), Alias("TimelyRQ", "TRQ")]
            [Summary("Gets the guild's set timely reward quantity.")]
            public async Task TimelyRewardQuantity()
            {
                var pref = await _guildPreferencesProvider.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync($"Timely Reward Quantity: **{pref.TimelyRewardQuantity}**");
            }

            [Command("AcceptedRole"), Alias("Role")]
            [Summary("Gets the guild's set default accepted role.")]
            public async Task AcceptedRole()
            {
                var pref = await _guildPreferencesProvider.GetPreferencesAsync(Context.Guild.Id);
                await this.ReplySuccessEmbedAsync(
                    $"Accepted role: **ID: {pref.AcceptedMemberRoleId}, Name: {Context.Guild.Roles.SingleOrDefault(r => r.Id == pref.AcceptedMemberRoleId)?.Name}**");
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
            [Summary("Sets the guild's set prefix.")]
            public async Task Prefix(
                [Summary("Prefix.")] string prefix)
            {
                await _guildPreferencesProvider.UpdatePreferencesAsync(Context.Guild.Id, pref => pref.Prefix = prefix);
                await this.ReplySuccessEmbedAsync($"Prefix was changed to: **{prefix}**");
            }

            [Command("CurrencySymbol"), Alias("CS")]
            [Summary("Sets the guild's set currency symbol.")]
            public async Task CurrencySymbol(
                [Summary("Currency Symbol")] string currencySymbol)
            {
                await _guildPreferencesProvider.UpdatePreferencesAsync(Context.Guild.Id, configuration => configuration.CurrencySymbol = currencySymbol);
                await this.ReplySuccessEmbedAsync($"Currency Symbol was changed to: **{currencySymbol}**");
            }

            [Command("TimelyCooldown"), Alias("TimelyCD", "TCD")]
            [Summary("Sets the guild's set timely cooldown.")]
            public async Task TimelyCooldown(
                [Summary("Timely Cooldown (in seconds)")] int cooldown)
            {
                await _guildPreferencesProvider.UpdatePreferencesAsync(Context.Guild.Id, configuration => configuration.TimelyCooldown = cooldown);
                await this.ReplySuccessEmbedAsync($"Timely Cooldown was changed to: **{TimeSpan.FromSeconds(cooldown)}**");
            }

            [Command("TimelyRewardQuantity"), Alias("TimelyRQ", "TRQ")]
            [Summary("Sets the guild's set timely reward quantity.")]
            public async Task TimelyRewardQuantity(
                [Summary("Timely Reward Quantity")] int quantity)
            {
                await _guildPreferencesProvider.UpdatePreferencesAsync(Context.Guild.Id, configuration => configuration.TimelyRewardQuantity = quantity);
                await this.ReplySuccessEmbedAsync($"Timely Reward Quantity was changed to: **{quantity}**");
            }

            [Command("AcceptedRole"), Alias("Role")]
            [Summary("Sets the guild's set default accepted role.")]
            public async Task AcceptedRole(
                [Summary("Name of the default accepted role")]
                string roleName)
            {
                // Get the roles.
                var roles = Context.Guild.Roles.Where(r => r.Name == roleName).ToArray();

                // Ensure there is no ambiguity.
                if (roles.Length > 1)
                {
                    await this.ReplyErrorEmbedAsync(
                        "Multiple roles with the same name were found. Make sure the role is uniquely named. Can change the name of the role once the initialization process is over.");
                    return;
                }

                if (roles.Length == 0)
                {
                    await this.ReplyErrorEmbedAsync(
                        "No roles with the given name were found. Is the role name correct?");
                    return;
                }

                // No ambiguity detected. Continue.
                var role = roles[0];

                await _guildPreferencesProvider.UpdatePreferencesAsync(Context.Guild.Id,
                    configuration => configuration.AcceptedMemberRoleId = role.Id);
                await this.ReplySuccessEmbedAsync(
                    $"Accepted role was changed to: **ID: {role.Id}, Name: {role.Name}**");
            }
        }
    }
}
