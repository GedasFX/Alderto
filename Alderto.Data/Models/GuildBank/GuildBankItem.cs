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
        /// Guild Bank Id.
        /// </summary>
        public int GuildBankId { get; set; }

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

        /// <summary>
        /// Quantity of the item in the bank.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Guild bank object.
        /// </summary>
        [ForeignKey(nameof(GuildBankId))]
        public virtual GuildBank GuildBank { get; set; }

        public new GuildBankItem MemberwiseClone()
        {
            return (GuildBankItem) base.MemberwiseClone();
        }
    }
}