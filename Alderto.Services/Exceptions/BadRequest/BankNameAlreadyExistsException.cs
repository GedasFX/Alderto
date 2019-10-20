namespace Alderto.Services.Exceptions
{
    public class BankNameAlreadyExistsException : ApiException
    {
        public BankNameAlreadyExistsException() : base(ErrorMessages.BankNameAlreadyExists)
        {
        }
    }
}