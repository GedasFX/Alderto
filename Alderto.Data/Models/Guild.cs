using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alderto.Data.Models
{
    public class Guild
    {
        [Key]
        public ulong Id { get; set; }

        public char? Prefix { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}