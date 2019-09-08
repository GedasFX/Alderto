using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services.GuildBankManagers
{
    public interface IGuildBankItemManager
    {
        Task<GuildBankItem> GetBankItemAsync(int itemId);
        Task<GuildBankItem> GetBankItemAsync(int bankId, string itemName);

        Task<List<GuildBankItem>> GetGuildBankContentsAsync(int bankId);
        Task<GuildBankItem> CreateBankItemAsync(int bankId, GuildBankItem item);
        Task UpdateBankItemAsync(int itemId, Action<GuildBankItem> changes);
        Task RemoveBankItemAsync(int itemId);
    }
}
