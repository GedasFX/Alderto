using Alderto.Data.Models;
using Discord;

namespace Alderto.Bot.Extensions
{
    public static class GuildMemberExtensions
    {
        public static string GetFullName(this GuildMember user, bool withNickname = true)
        {
            if (withNickname)
                return $"{user.Nickname ?? user.Member.Username} [{user.Member.Username}#{user.Member.Discriminator}]";

            return $"{user.Member.Username}#{user.Member.Discriminator}";
        }
    }
}
