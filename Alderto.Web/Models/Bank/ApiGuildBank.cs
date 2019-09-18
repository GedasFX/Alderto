using Alderto.Data.Models.GuildBank;

namespace Alderto.Web.Models.Bank
{
    public class ApiGuildBank : GuildBank
    {
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
        public ApiGuildBank(int id, ulong guildId, ulong? logChannelId, ulong moderatorRoleId, string name)
            : this(guildId, name)
        {
            Id = id;
            LogChannelId = logChannelId;
            ModeratorRoleId = moderatorRoleId;
        }
        public ApiGuildBank(GuildBank guildBank)
            : this(guildBank.Id, guildBank.GuildId, guildBank.LogChannelId, guildBank.ModeratorRoleId, guildBank.Name)
        {
            
        }
    }
}
