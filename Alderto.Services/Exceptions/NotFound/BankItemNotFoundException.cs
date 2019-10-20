namespace Alderto.Services.Exceptions
{
    public class BankItemNotFoundException : ApiException
    {
        public BankItemNotFoundException() : base(ErrorMessages.BankItemNotFound)
        {
        }
    }
}