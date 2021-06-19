using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class Member
    {
        /// <summary>
        /// Discord member identifier
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        /// <summary>
        /// Last known username of the user. Max Length: 32 (Discord limitation)
        /// </summary>
        [MaxLength(32)]
        public string? Username { get; set; }

        /// <summary>
        /// Last known discriminator of the user. Is always a length of 4.
        /// </summary>
        /// <example>0004 or 1234</example>
        [MinLength(4), MaxLength(4)]
        public string? Discriminator { get; set; }

        /// <summary>
        /// A collection of guilds the user is in.
        /// </summary>
        public virtual IEnumerable<GuildMember>? GuildMembers { get; set; }
        
        public virtual IList<GuildMemberWallet>? Wallets { get; set; } 

        /// <summary>
        /// Initializes a new empty instance of <see cref="Member"/>.
        /// For use by Entity Framework.
        /// </summary>
        private Member() { }

        /// <summary>
        /// Initializes a new instance of <see cref="GuildConfiguration"/>, with primary key (<see cref="Id"/>) set.
        /// </summary>
        public Member(ulong id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GuildConfiguration"/>, with primary key (<see cref="Id"/>),
        /// and common properties (<see cref="Username"/> and <see cref="Discriminator"/>) set.
        /// </summary>
        /// <param name="id"><see cref="Id"/> property.</param>
        /// <param name="username"><see cref="Username"/> property.</param>
        /// <param name="discriminator"><see cref="Discriminator"/> property.</param>
        public Member(ulong id, string username, string discriminator) : this(id)
        {
            Username = username;
            Discriminator = discriminator;
        }
    }
}
