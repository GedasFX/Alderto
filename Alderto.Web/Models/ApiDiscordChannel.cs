namespace Alderto.Web.Models
{
    public class ApiDiscordChannel
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public ApiDiscordChannel(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
