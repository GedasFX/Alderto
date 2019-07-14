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
        /// Last known username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Last known discriminator of the user.
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// A collection of guilds the user is in.
        /// </summary>
        public virtual IEnumerable<GuildMember> GuildMembers { get; set; }

        public Member(ulong id)
        {
            Id = id;
        }

        public Member(ulong id, string username, string discriminator) : this(id)
        {
            Username = username;
            Discriminator = discriminator;
        }
    }
}
