using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Path to the image to be displayed in the bank.
        /// </summary>
        [MaxLength(140)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Monetary value per unit.
        /// </summary>
        public double Value { get; set; }
    }
}