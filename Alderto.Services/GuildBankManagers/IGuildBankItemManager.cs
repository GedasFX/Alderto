using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services.GuildBankManagers
{
    public interface IGuildBankItemManager
    {
        Task<GuildBankItem> CreateItemAsync(ulong guildId, string name, string description = null, double value = 0, string imageUrl = null);
        Task UpdateItemAsync(ulong guildId, string itemName, Action<GuildBankItem> changes);
        Task RemoveItemAsync(ulong guildId, string itemName);
        Task<GuildBankItem> GetItemAsync(ulong guildId, string itemName);

        Task<GuildBankBankItem> CreateBankItemAsync(int bankId, int itemId);
        Task UpdateBankItemAsync(int bankId, int itemId, Action<GuildBankBankItem> changes);
        Task RemoveBankItemAsync(int bankId, int itemId);
        Task<GuildBankBankItem> GetBankItemAsync(int bankId, int itemId);

        Task<List<GuildBankItem>> GetGuildItems(ulong guildId);
        Task<List<GuildBankBankItem>> GetGuildBankItems(int bankId);
    }
}
