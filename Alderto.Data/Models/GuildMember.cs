using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildMember
    {
        /// <summary>
        /// Discord Member Id.
        /// </summary>
        public ulong MemberId { get; set; }

        /// <summary>
        /// Discord Guild Id in which member is member of.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Last known nickname of the user. Max Length: 32 (Discord limitation)
        /// </summary>
        [MaxLength(32)]
        public string Nickname { get; set; }

        /// <summary>
        /// Time of last currency claim.
        /// </summary>
        public DateTimeOffset CurrencyLastClaimed { get; set; }

        /// <summary>
        /// Time the user joined the guild.
        /// </summary>
        public DateTimeOffset? JoinedAt { get; set; }

        /// <summary>
        /// Amount of currency a person has.
        /// </summary>
        public int CurrencyCount { get; set; }

        /// <summary>
        /// Id of member, who recruited the current member.
        /// </summary>
        public ulong? RecruiterMemberId { get; set; }

        /// <summary>
        /// Guild, referenced by <see cref="GuildId"/>.
        /// </summary>
        [ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }

        /// <summary>
        /// Member, referenced by <see cref="MemberId"/>.
        /// </summary>
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }
        
        /// <summary>
        /// A collection of donations the user has provided.
        /// </summary>
        public virtual IEnumerable<GuildMemberDonation> Donations { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="GuildMember"/>.
        /// </summary>
        public GuildMember()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GuildMember"/>, with primary key (<see cref="MemberId"/> and <see cref="GuildId"/>) set.
        /// </summary>
        /// <param name="guildId">Id og the guild user is in.</param>
        /// <param name="memberId">Id of the user.</param>
        public GuildMember(ulong guildId, ulong memberId)
        {
            GuildId = guildId;
            MemberId = memberId;
        }
    }
}