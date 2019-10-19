namespace Alderto.Services.Exceptions
{
    public class NameNotProvidedException : ApiException
    {
        public NameNotProvidedException() : base(ErrorMessages.NameNotProvided)
        {
        }
    }
}