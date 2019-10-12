namespace Alderto.Services.Exceptions.NotFound
{
    public class ChannelNotFoundException : ApiException
    {
        public ChannelNotFoundException() : base(ErrorMessages.ChannelNotFound)
        {
        }
    }
}