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

        public IEnumerable<ApiGuildBankItem>? Contents { get; set; }

        public ApiGuildBank(GuildBank guildBank)
        {
            Id = guildBank.Id;

            GuildId = guildBank.GuildId;
            Name = guildBank.Name;

            LogChannelId = guildBank.LogChannelId;
            ModeratorRoleId = guildBank.ModeratorRoleId;

            Contents = guildBank.Contents?.Select(c => new ApiGuildBankItem(c));
        }
    }
}
