using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class Guild
    {
        /// <summary>
        /// Key. Discord guild identifier.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        /// <summary>
        /// Time when guild premium status runs out. Returns null if is not premium.
        /// </summary>
        public DateTimeOffset? PremiumUntil { get; set; }

        /// <summary>
        /// The preferences of the guild.
        /// </summary>
        public virtual GuildConfiguration? Configuration { get; set; }

        /// <summary>
        /// The registered aliases for the specified guild.
        /// </summary>
        public virtual IList<GuildCommandAlias>? Aliases { get; set; }

        /// <summary>
        /// A collection of members of the guild.
        /// </summary>
        public virtual IList<GuildMember>? GuildMembers { get; set; }

        /// <summary>
        /// A collection of custom commands registered to the guild.
        /// </summary>
        public virtual IList<CustomCommand>? CustomCommands { get; set; }

        /// <summary>
        /// A collection of guild managed banks.
        /// </summary>
        public virtual IList<GuildBank>? GuildBanks { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Guild"/>, with primary key <see cref="Id"/> set.
        /// </summary>
        /// <param name="id"><see cref="Id"/> property.</param>
        public Guild(ulong id)
        {
            Id = id;
        }
    }
}
