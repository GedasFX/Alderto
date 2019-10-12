namespace Alderto.Services.Exceptions.BadRequest
{
    public class NameCannotBeNullException : ApiException
    {
        public NameCannotBeNullException() : base(ErrorMessages.NameCannotBeNull)
        {
        }
    }
}