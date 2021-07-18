namespace Alderto.Web.Models
{
    public class ApiGuildUserInfo
    {
        public AccessLevel AccessLevel { get; init; }
    }

    public enum AccessLevel
    {
        Guest = 0,
        Member = 1,
        Moderator = 2,
        Admin = 3,
    }
}
