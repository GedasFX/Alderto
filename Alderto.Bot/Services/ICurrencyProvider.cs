using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public interface ICurrencyProvider
    {
        Task ModifyPointsAsync(ulong memberId, ulong guildId, int deltaPoints, bool saveChanges = true);
        Task ModifyPointsAsync(GuildMember guildMember, int deltaPoints, bool saveChanges = true);
        Task ModifyPointsAsync(IEnumerable<GuildMember> guildMembers, int deltaPoints, bool saveChanges = true);
    }
}
