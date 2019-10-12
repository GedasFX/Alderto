namespace Alderto.Services.Exceptions.BadRequest
{
    public class ChannelNotMessageChannelException : ApiException
    {
        public ChannelNotMessageChannelException() : base(ErrorMessages.ChannelNotMessageChannel)
        {
        }
    }
}