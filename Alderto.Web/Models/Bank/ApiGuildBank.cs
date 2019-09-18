using System.Collections.Generic;
using System.Linq;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Newtonsoft.Json;

namespace Alderto.Web.Models.Bank
{
    public sealed class ApiGuildBank : GuildBank
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

        [JsonIgnore]
        public override Guild Guild
        {
            get => base.Guild;
            set => base.Guild = value;
        }

        public new IEnumerable<ApiGuildBankItem> Contents { get; set; }

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
