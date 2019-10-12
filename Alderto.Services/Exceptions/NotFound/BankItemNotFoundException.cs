namespace Alderto.Services.Exceptions.NotFound
{
    public class BankItemNotFoundException : ApiException
    {
        public BankItemNotFoundException() : base(ErrorMessages.BankItemNotFound)
        {
        }
    }
}