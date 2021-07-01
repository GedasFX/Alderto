namespace Alderto.Application.Features.Discord.Dto
{
    public class DiscordRoleDto
    {
        public ulong Id { get; }
        public string Name { get; }

        public DiscordRoleDto(ulong id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
