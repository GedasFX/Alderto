namespace Alderto.Services.Exceptions
{
    public class GuildPreferenceNotFoundException : ApiException
    {
        public GuildPreferenceNotFoundException() : base(ErrorMessages.GuildPreferenceNotFound)
        {
        }
    }
}
