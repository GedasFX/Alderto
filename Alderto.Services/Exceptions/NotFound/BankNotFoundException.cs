namespace Alderto.Services.Exceptions
{
    public class BankNotFoundException : ApiException
    {
        public BankNotFoundException() : base(ErrorMessages.BankNotFound)
        {
        }
    }
}