namespace Alderto.Services.Exceptions.Forbid
{
    public class UserNotGuildModeratorException : ApiException
    {
        public UserNotGuildModeratorException() : base(ErrorMessages.UserNotGuildModerator)
        {
        }
    }
}