using Newtonsoft.Json;

namespace Alderto.Web.Models.Discord
{
    public class DiscordGuild
    {
        [JsonProperty("owner")]
        public bool Owner { get; set; }

        [JsonProperty("permissions")]
        public ulong Permissions { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}