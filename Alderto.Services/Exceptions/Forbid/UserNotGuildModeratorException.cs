namespace Alderto.Services.Exceptions
{
    public class UserNotGuildModeratorException : ApiException
    {
        public UserNotGuildModeratorException() : base(ErrorMessages.UserNotGuildModerator)
        {
        }
    }
}