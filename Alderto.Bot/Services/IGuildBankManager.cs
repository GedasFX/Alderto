using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public interface IGuildBankManager
    {
        /// <summary>
        /// Registers a donation the user has given to the guild.
        /// </summary>
        /// <param name="member">User, who has given the donation.</param>
        /// <param name="donation">Donation.</param>
        Task AddDonationAsync(GuildMember member, string donation);

        /// <summary>
        /// Lists all donations given by the user.
        /// </summary>
        /// <param name="member">User to get donations of.</param>
        /// <returns>A collection of donations the user has made.</returns>
        Task<IEnumerable<GuildMemberDonation>> GetDonationsAsync(GuildMember member);

        /// <summary>
        /// Finds the donation given by the primary key.
        /// </summary>
        /// <param name="id">Primary key of a donation.</param>
        /// <returns></returns>
        Task<GuildMemberDonation> GetDonationAsync(int id);

        /// <summary>
        /// Removes a given <see cref="donation"/> from the store.
        /// </summary>
        /// <param name="donation">Donation to remove from the store.</param>
        Task RemoveDonationAsync(GuildMemberDonation donation);
    }
}