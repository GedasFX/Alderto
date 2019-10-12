namespace Alderto.Services.Exceptions.NotFound
{
    public class MessageNotFoundException : ApiException
    {
        public MessageNotFoundException() : base(ErrorMessages.MessageNotFound)
        {
        }
    }
}