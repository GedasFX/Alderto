namespace Alderto.Services.Exceptions.BadRequest
{
    public class BankNameAlreadyExistsException : ApiException
    {
        public BankNameAlreadyExistsException() : base(ErrorMessages.BankNameAlreadyExists)
        {
        }
    }
}