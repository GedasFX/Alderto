using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class Member
    {
        [Key]
        public uint Id { get; set; }

        public ulong MemberId { get; set; }
        public ulong GuildId { get; set; }

        public DateTime? CurrencyLastClaimed { get; set; }
        public int CurrencyCount { get; set; }
        
		public uint? RecruitedByMemberId { get; set; }

        [ForeignKey(nameof(RecruitedByMemberId))]
        public virtual Member RecruitedByMember { get; set; }

        [ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }

        public virtual ICollection<Member> MembersRecruited { get; set; }
    }
}