using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public interface IGuildPreferencesProviderService
    {
        Task<GuildConfiguration> GetPreferencesAsync(ulong guildId);
    }
}