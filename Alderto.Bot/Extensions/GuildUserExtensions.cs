using Discord;

namespace Alderto.Bot.Extensions
{
    public static class GuildUserExtensions
    {
        public static string GetFullName(this IGuildUser user, bool withNickname = true)
        {
            if (withNickname)
                return $"{user.Nickname ?? user.Username} [{user.Username}#{user.Discriminator}]";

            return $"{user.Username}#{user.Discriminator}";
        }
    }
}
