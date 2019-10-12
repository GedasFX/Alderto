namespace Alderto.Services.Exceptions.NotFound
{
    public class UserNotFoundException : ApiException
    {
        public UserNotFoundException() : base(ErrorMessages.UserNotFound)
        {
        }
    }
}