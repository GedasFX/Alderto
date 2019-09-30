using System.Collections.Generic;
using System.Linq;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Web.Models.Bank
{
    public sealed class ApiGuildBank
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong? LogChannelId { get; set; }
        public string Name { get; set; }
        public ulong? ModeratorRoleId { get; set; }

        /// <summary>
        /// True if person requesting this resource has write access to it.
        /// </summary>
        public bool CanModify { get; set; }

        public ApiGuildBank() { }
        public ApiGuildBank(ulong guildId, string name)
        {
            GuildId = guildId;
            Name = name;
        }
        
        public IEnumerable<ApiGuildBankItem> Contents { get; set; }

        public ApiGuildBank(int id, ulong guildId, ulong? logChannelId, ulong? moderatorRoleId, string name)
            : this(guildId, name)
        {
            Id = id;
            LogChannelId = logChannelId;
            ModeratorRoleId = moderatorRoleId;
        }
        public ApiGuildBank(GuildBank guildBank)
            : this(guildBank.Id, guildBank.GuildId, guildBank.LogChannelId, guildBank.ModeratorRoleId, guildBank.Name)
        {
            Contents = guildBank.Contents?.Select(c => new ApiGuildBankItem(c));
        }
    }
}
