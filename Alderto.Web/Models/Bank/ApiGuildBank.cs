using System.Collections.Generic;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Web.Models.Bank
{
    public class ApiGuildBank
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong? LogChannelId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// True if person requesting this resource has write access to it.
        /// </summary>
        public bool CanModify { get; set; }

        public ICollection<GuildBankItem> Contents { get; set; }

        public ApiGuildBank() { }
        public ApiGuildBank(ulong guildId, string name)
        {
            GuildId = guildId;
            Name = name;
        }
        public ApiGuildBank(int id, ulong guildId, ulong? logChannelId, string name)
            : this(guildId, name)
        {
            Id = id;
            LogChannelId = logChannelId;
        }
        public ApiGuildBank(GuildBank guildBank)
            : this(guildBank.Id, guildBank.GuildId, guildBank.LogChannelId, guildBank.Name)
        {
            Contents = guildBank.Contents;
        }
    }
}
