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
        /// Guild's chosen prefix
        /// </summary>
        public char? Prefix { get; set; }
        
        /// <summary>
        /// Time when guild premium status runs out. Returns null if is not premium.
        /// </summary>
        public DateTime? PremiumUntil { get; set; }

        /// <summary>
        /// A collection of members the guild contains.
        /// </summary>
        public virtual IEnumerable<GuildMember> Members { get; set; }

        /// <summary>
        /// A collection of all custom commands registered to the guild.
        /// </summary>
        public virtual IEnumerable<CustomCommand> CustomCommands { get; set; }
        
        /// <summary>
        /// Creates a guild
        /// </summary>
        /// <param name="id"></param>
        public Guild(ulong id)
        {
            Id = id;
        }
    }
}