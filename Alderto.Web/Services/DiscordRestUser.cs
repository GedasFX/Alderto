namespace Alderto.Web.Services
{
    public class DiscordRestUser : DiscordRestBase
    {
        public DiscordRestUser(string token) : base($"Bearer {token}")
        {
        }
    }
}