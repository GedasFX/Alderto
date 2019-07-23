using System;
using System.ComponentModel.DataAnnotations;

namespace Alderto.Data.Models
{
    public class GuildMemberDonation
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Donation the user has provided.
        /// </summary>
        [MaxLength(100)]
        public string Donation { get; set; }

        /// <summary>
        /// Date and time when donation occured.
        /// </summary>
        public DateTimeOffset DonationDate { get; set; }

        /// <summary>
        /// Foreign key to the guild, in which the user resides.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Foreign key to the member, who has given the donation.
        /// </summary>
        public ulong MemberId { get; set; }

        /// <summary>
        /// User, who provided the donation.
        /// </summary>
        public virtual GuildMember GuildMember { get; set; }
    }
}