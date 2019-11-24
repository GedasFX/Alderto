using System.Text.Json.Serialization;

namespace Alderto.Web.Models.Discord
{
    public class DiscordApiUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }

        public string? Avatar { get; set; }
        public bool? Bot { get; set; }

        [JsonPropertyName("mfa_enabled")]
        public bool? MfaEnabled { get; set; }

        public string? Locale { get; set; }

        [JsonPropertyName("verified")]
        public bool? EmailVerified { get; set; }
        public string? Email { get; set; }

        public int? Flags { get; set; }

        [JsonPropertyName("premium_type")]
        public int? PremiumType { get; set; }

#nullable disable
        public DiscordApiUser() { }
#nullable restore
    }


}