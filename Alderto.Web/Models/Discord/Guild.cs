namespace Alderto.Web.Models.Discord
{
    public class Guild
    {
        public bool Owner { get; set; }
        public ulong Permissions { get; set; }
        public string Icon { get; set; }
        public ulong Id { get; set; }
        public string Name { get; set; }
    }
}