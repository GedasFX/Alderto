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
        [MaxLength(70), Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Monetary value per unit.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Quantity of the item in the bank.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        [MaxLength(280)]
        public string? Description { get; set; }

        /// <summary>
        /// Path to the image to be displayed in the bank.
        /// </summary>
        [MaxLength(140)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Guild bank object.
        /// </summary>
        [ForeignKey(nameof(GuildBankId))]
        public virtual GuildBank? GuildBank { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="GuildBankItem"/>.
        /// For use by Entity Framework.
        /// </summary>
        public GuildBankItem() { }

        public GuildBankItem(string name)
        {
            Name = name;
        }

        public GuildBankItem(int id, int bankId, string name)
            : this(name)
        {
            Id = id;
            GuildBankId = bankId;
        }

        public new GuildBankItem MemberwiseClone()
        {
            return (GuildBankItem)base.MemberwiseClone();
        }
    }
}