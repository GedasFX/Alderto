namespace Alderto.Services.Exceptions.BadRequest
{
    public class BotNotMessageOwnerException : ApiException
    {
        public BotNotMessageOwnerException() : base(ErrorMessages.BotNotMessageOwner)
        {
        }
    }
}