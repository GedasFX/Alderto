using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models.GuildBank
{
    public class GuildBankBankItem
    {
        /// <summary>
        /// Key. Guild Bank Id.
        /// </summary>
        public int GuildBankId { get; set; }

        /// <summary>
        /// Key. Guild Bank Item Id.
        /// </summary>
        public int GuildBankItemId { get; set; }

        /// <summary>
        /// Quantity of the item in the bank.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Guild bank object.
        /// </summary>
        [ForeignKey(nameof(GuildBankId))]
        public virtual GuildBank GuildBank { get; set; }

        /// <summary>
        /// Guild bank item object.
        /// </summary>
        [ForeignKey(nameof(GuildBankItemId))]
        public virtual GuildBankItem GuildBankItem { get; set; }
    }
}