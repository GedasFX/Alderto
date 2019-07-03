using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Bot.Data.Models
{
    public class Member
    {
        [Key]
        public Guid Id { get; set; }

        public ulong MemberId { get; set; }
        public ulong GuildId { get; set; }

        public DateTime? CurrencyLastClaimed { get; set; }
        public int CurrencyCount { get; set; }
        
		public Guid? RecruitedByMemberId { get; set; }

        [ForeignKey(nameof(RecruitedByMemberId))]
        public virtual Member RecruitedByMember { get; set; }

        public virtual ICollection<Member> MembersRecruited { get; set; }
    }
}