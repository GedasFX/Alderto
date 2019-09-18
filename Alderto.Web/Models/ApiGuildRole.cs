namespace Alderto.Web.Models
{
    public class ApiGuildRole
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public ApiGuildRole(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}