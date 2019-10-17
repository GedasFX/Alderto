namespace Alderto.Services.Exceptions
{
    public class MessageNotFoundException : ApiException
    {
        public MessageNotFoundException() : base(ErrorMessages.MessageNotFound)
        {
        }
    }
}