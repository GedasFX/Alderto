namespace Alderto.Services.Exceptions.NotFound
{
    public class BankNotFoundException : ApiException
    {
        public BankNotFoundException() : base(ErrorMessages.BankNotFound)
        {
        }
    }
}