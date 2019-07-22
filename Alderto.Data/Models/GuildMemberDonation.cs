using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildMemberDonation
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Donation the user has provided.
        /// </summary>
        [MaxLength(100)]
        public string Donation { get; set; }

        /// <summary>
        /// Foreign key to user, who provided the donation.
        /// </summary>
        public Guid GuildMemberId { get; set; }

        /// <summary>
        /// User, who provided the donation.
        /// </summary>
        [ForeignKey(nameof(GuildMemberId))]
        public virtual GuildMember GuildMember { get; set; }
    }
}