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
        /// A collection of members the guild contains.
        /// </summary>
        public virtual IEnumerable<Member> Members { get; set; }

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