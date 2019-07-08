using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class Member
    {
        /// <summary>
        /// Unique member identifier within the bot. Differs between guilds.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Discord Member Id.
        /// </summary>
        public ulong MemberId { get; set; }

        /// <summary>
        /// Discord Guild Id in which member is member of.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Time of last currency claim. Value is null if never claimed timely reward.
        /// </summary>
        public DateTime? CurrencyLastClaimed { get; set; }

        /// <summary>
        /// Amount of currency a person has.
        /// </summary>
        public int CurrencyCount { get; set; }
        
        /// <summary>
        /// Id of member, who recruited the current member.
        /// </summary>
		public Guid? RecruitedByMemberId { get; set; }

        /// <summary>
        /// Member, referenced by <see cref="RecruitedByMemberId"/>.
        /// </summary>
        [ForeignKey(nameof(RecruitedByMemberId))]
        public virtual Member RecruitedByMember { get; set; }

        /// <summary>
        /// Guild, referenced by <see cref="GuildId"/>
        /// </summary>
        [ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }

        /// <summary>
        /// Creates a new member object with the given Guild id and Member id
        /// </summary>
        /// <param name="guildId">Id og the guild user is in.</param>
        /// <param name="memberId">Id of the user.</param>
        public Member(ulong guildId, ulong memberId)
        {
            GuildId = guildId;
            MemberId = memberId;
        }
    }
}