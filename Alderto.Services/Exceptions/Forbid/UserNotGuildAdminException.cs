namespace Alderto.Services.Exceptions
{
    public class UserNotGuildAdminException : ApiException
    {
        public UserNotGuildAdminException() : base(ErrorMessages.UserNotGuildAdmin)
        {
        }
    }
}