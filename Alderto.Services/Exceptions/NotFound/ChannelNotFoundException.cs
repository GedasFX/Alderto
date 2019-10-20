namespace Alderto.Services.Exceptions
{
    public class ChannelNotFoundException : ApiException
    {
        public ChannelNotFoundException() : base(ErrorMessages.ChannelNotFound)
        {
        }
    }
}