using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Alderto.Data.Models.GuildBank
{
    public class GuildBank
    {
        /// <summary>
        /// Key. Guild Bank Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Discord Guild Id.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Name of the bank.
        /// </summary>
        [MaxLength(32), Required]
        public string Name { get; set; }

        /// <summary>
        /// Id of channel to log changes to the bank to.
        /// </summary>
        public ulong? LogChannelId { get; set; }

        /// <summary>
        /// Id of role, which allows to modify contents of the bank.
        /// </summary>
        public ulong? ModeratorRoleId { get; set; }

        /// <summary>
        /// Guild, referenced by <see cref="GuildId"/>.
        /// </summary>
        [ForeignKey(nameof(GuildId))]
        public virtual Guild? Guild { get; set; }

        /// <summary>
        /// A collection of items in the bank.
        /// </summary>
        public virtual ICollection<GuildBankItem>? Contents { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="GuildBank"/>.
        /// For use by Entity Framework.
        /// </summary>
        [JsonConstructor]
        public GuildBank()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GuildBank"/>, with <see cref="GuildId"/>) property set.
        /// </summary>
        /// <param name="guildId">Id of the guild, which owns the guild.</param>
        /// <param name="name">Name of the bank.</param>
        public GuildBank(ulong guildId, string name)
        {
            GuildId = guildId;
            Name = name;
        }
    }
}
