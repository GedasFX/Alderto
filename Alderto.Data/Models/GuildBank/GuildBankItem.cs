using System.ComponentModel.DataAnnotations;

namespace Alderto.Data.Models.GuildBank
{
    public class GuildBankItem
    {
        /// <summary>
        /// Key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Id of guild, which has created the item.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Name of the item.
        /// </summary>
        [MaxLength(70)]
        public string Name { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        [MaxLength(280)]
        public string Description { get; set; }

        /// <summary>
        /// Monetary value per unit.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Guild, which has created the item.
        /// </summary>
        public virtual Guild Guild { get; set; }
    }
}