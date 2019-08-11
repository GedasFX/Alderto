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
        /// Name of the item.
        /// </summary>
        [MaxLength(70)]
        public string Name { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        [MaxLength(280)]
        public string Description { get; set; }
    }
}