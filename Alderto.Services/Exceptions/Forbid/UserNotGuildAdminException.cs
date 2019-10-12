namespace Alderto.Services.Exceptions.Forbid
{
    public class UserNotGuildAdminException : ApiException
    {
        public UserNotGuildAdminException() : base(ErrorMessages.UserNotGuildAdmin)
        {
        }
    }
}