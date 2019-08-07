namespace Alderto.Web.Services
{
    public class DiscordRestBot : DiscordRestBase
    {
        public DiscordRestBot(string token) : base($"Bot {token}")
        {
        }
    }
}