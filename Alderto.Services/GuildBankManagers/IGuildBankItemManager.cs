using System;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services.GuildBankManagers
{
    public interface IGuildBankItemManager
    {
        Task CreateAsync(ulong guildId, GuildBankItem item);
        Task UpdateAsync(ulong guildId, string itemName, Action<GuildBankItem> changes);
        Task RemoveAsync(ulong guildId, string itemName);
        Task<GuildBankItem> GetItemAsync(ulong guildId, string itemName);
        GuildBankBankItem GetBankItem(GuildBank bank, string itemName);
    }
}
