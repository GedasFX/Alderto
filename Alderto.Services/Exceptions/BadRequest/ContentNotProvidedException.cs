namespace Alderto.Services.Exceptions
{
    public class ContentNotProvidedException : ApiException
    {
        public ContentNotProvidedException() : base(ErrorMessages.ContentNotProvided)
        {
        }
    }
}