namespace Alderto.Services.Exceptions
{
    public class GuildNotFoundException : ApiException
    {
        public GuildNotFoundException() : base(ErrorMessages.GuildNotFound)
        {
        }
    }
}