namespace Alderto.Services.Exceptions
{
    public class UserNotFoundException : ApiException
    {
        public UserNotFoundException() : base(ErrorMessages.UserNotFound)
        {
        }
    }
}