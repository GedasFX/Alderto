namespace Alderto.Web.Models.Discord
{
#nullable disable
    public class DiscordApiGuild
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ulong Permissions { get; set; }
        public bool Owner { get; set; }
        public string Icon { get; set; }
    }
#nullable restore
}