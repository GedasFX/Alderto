using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Bot.Data.Models
{
    public class Member
    {
        public Guid Id { get; set; }

        public string MemberId { get; set; }
        public string GuildId { get; set; }

        public DateTime? CurrencyLastClaimed { get; set; }
        public int CurrencyCount { get; set; }
        
		public string RecruitedByMemberId { get; set; }

        [ForeignKey(nameof(Id))]
        public virtual Member RecruitedByMember { get; set; }

		[ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }

        public virtual ICollection<Member> MembersRecruited { get; set; }
    }
}