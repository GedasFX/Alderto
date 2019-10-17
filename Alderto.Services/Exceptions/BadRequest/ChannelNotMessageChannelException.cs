namespace Alderto.Services.Exceptions
{
    public class ChannelNotMessageChannelException : ApiException
    {
        public ChannelNotMessageChannelException() : base(ErrorMessages.ChannelNotMessageChannel)
        {
        }
    }
}