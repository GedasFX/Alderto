namespace Alderto.Services.Exceptions
{
    public class NameCannotBeNullException : ApiException
    {
        public NameCannotBeNullException() : base(ErrorMessages.NameCannotBeNull)
        {
        }
    }
}