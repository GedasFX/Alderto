using System.ComponentModel.DataAnnotations;

namespace Alderto.Web.Models.GuildPreferences
{
    public class GuildPreferencesInputModel
    {
        [MaxLength(20)]
        public string? Prefix { get; set; }

        [MaxLength(50)]
        public string? CurrencySymbol { get; set; }

        public int? TimelyRewardQuantity { get; set; }

        public int? TimelyCooldown { get; set; }

        public ulong? AcceptedMemberRoleId { get; set; }
    }
}
