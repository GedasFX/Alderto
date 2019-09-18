namespace Alderto.Web.Models
{
    public class ApiGuildChannel
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public ApiGuildChannel(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}