namespace Alderto.Services.Exceptions
{
    public class BotNotMessageOwnerException : ApiException
    {
        public BotNotMessageOwnerException() : base(ErrorMessages.BotNotMessageOwner)
        {
        }
    }
}