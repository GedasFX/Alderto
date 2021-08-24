namespace Alderto.Web.Models
{
    public class ApiDiscordRole
    {
        public ulong Id { get; }
        public string Name { get; }

        public ApiDiscordRole(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
