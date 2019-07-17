using Alderto.Data.Models;

namespace Alderto.Bot.Extensions
{
    public static class GuildConfigurationExtensions
    {
        /// <summary>
        /// Gets the guild's set, or default if unset, command prefix.
        /// </summary>
        /// <param name="guildPreferences">Guild preferences.</param>
        /// <returns>Guild's set, or default if unset, command prefix.</returns>
        public static string GetPrefix(this GuildConfiguration guildPreferences)
        {
            var prefix = guildPreferences.Prefix;
            return string.IsNullOrWhiteSpace(prefix) ? prefix : GuildConfiguration.DefaultPrefix;
        }

        /// <summary>
        /// Gets the guild's set, or default if unset, currency symbol.
        /// </summary>
        /// <param name="guildPreferences">Guild preferences.</param>
        /// <returns>Guild's set, or default if unset, currency symbol.</returns>
        public static string GetCurrencySymbol(this GuildConfiguration guildPreferences)
        {
            var currencySymbol = guildPreferences.CurrencySymbol;
            return string.IsNullOrWhiteSpace(currencySymbol) ? currencySymbol : GuildConfiguration.DefaultCurrencySymbol;
        }

        /// <summary>
        /// Gets the guild's set, or default if unset, timely cooldown.
        /// </summary>
        /// <param name="guildPreferences">Guild preferences.</param>
        /// <returns>Guild's set, or default if unset, timely cooldown.</returns>
        public static int GetTimelyCooldown(this GuildConfiguration guildPreferences)
        {
            var timelyCooldown = guildPreferences.TimelyCooldown;
            return timelyCooldown ?? GuildConfiguration.DefaultTimelyCooldown;
        }

        /// <summary>
        /// Gets the guild's set, or default if unset, timely reward quantity.
        /// </summary>
        /// <param name="guildPreferences">Guild preferences.</param>
        /// <returns>Guild's set, or default if unset, timely reward quantity.</returns>
        public static int GetTimelyRewardQuantity(this GuildConfiguration guildPreferences)
        {
            var timelyRewardQuantity = guildPreferences.TimelyRewardQuantity;
            return timelyRewardQuantity ?? GuildConfiguration.DefaultTimelyRewardQuantity;
        }
    }
}
