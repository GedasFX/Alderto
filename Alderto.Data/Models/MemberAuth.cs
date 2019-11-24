using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class MemberSession
    {
        /// <summary>
        /// [Key #1] Id of member this refresh token belongs to.
        /// </summary>
        public ulong MemberId { get; set; }

        /// <summary>
        /// [Key #2] Encrypted refresh token.
        /// </summary>
        public DateTimeOffset IssuedAt { get; set; }

        [ForeignKey(nameof(MemberId))]
        public virtual Member? Member { get; set; }
    }
}
