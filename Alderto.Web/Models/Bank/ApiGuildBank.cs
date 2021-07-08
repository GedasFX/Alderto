using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Web.Models.Bank
{
    public class ApiGuildBank
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }

        [Required]
        public string Name { get; set; }

        public ulong? LogChannelId { get; set; }
        public ulong? ModeratorRoleId { get; set; }

        public IEnumerable<ApiGuildBankItem>? Contents { get; set; }

        public ApiGuildBank()
        {
        }

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