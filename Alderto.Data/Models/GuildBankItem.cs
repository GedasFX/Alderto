﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data.Models
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
        public string Name { get; set; }

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
        [JsonConstructor]
        public GuildBankItem()
        {
        }

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
    }

    public static class GuildBankItemRepository
    {
        public static IQueryable<GuildBankItem> ListItems(
            this IQueryable<GuildBankItem> query, ulong guildId, int bankId) =>
            query.Include(q => q.GuildBank)
                .Where(b => b.GuildBank!.GuildId == guildId)
                .Where(b => b.GuildBank!.Id == bankId);

        public static IQueryable<GuildBankItem> FindItem(
            this IQueryable<GuildBankItem> query, ulong guildId, int bankId, int itemId) =>
            ListItems(query, guildId, bankId)
                .Where(q => q.Id == itemId);
    }
}
