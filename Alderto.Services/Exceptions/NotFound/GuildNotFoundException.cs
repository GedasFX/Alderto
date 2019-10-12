namespace Alderto.Services.Exceptions.NotFound
{
    public class GuildNotFoundException : ApiException
    {
        public GuildNotFoundException() : base(ErrorMessages.GuildNotFound)
        {
        }
    }
}