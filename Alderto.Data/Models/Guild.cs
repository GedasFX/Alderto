using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class Guild
    {
        /// <summary>
        /// Discord guild identifier.
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
        public virtual GuildConfiguration Configuration { get; set; }

        /// <summary>
        /// A collection of members the guild contains.
        /// </summary>
        public virtual IEnumerable<GuildMember> GuildMembers { get; set; }

        /// <summary>
        /// A collection of all custom commands registered to the guild.
        /// </summary>
        public virtual IEnumerable<CustomCommand> CustomCommands { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="Guild"/>.
        /// </summary>
        public Guild()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Guild"/>, with primary key (<see cref="Id"/> set.
        /// </summary>
        /// <param name="id"><see cref="Id"/> property.</param>
        public Guild(ulong id)
        {
            Id = id;
        }
    }
}