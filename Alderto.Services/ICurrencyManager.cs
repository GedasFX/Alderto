using System;
using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Services
{
    public interface ICurrencyManager
    {
        /// <summary>
        /// Modifies the points the user <see cref="guildMember"/> has by <see cref="deltaPoints"/>.
        /// </summary>
        /// <param name="guildMember"><see cref="GuildMember"/> to modify.</param>
        /// <param name="deltaPoints">Amount of points to modify by.</param>
        Task ModifyPointsAsync(GuildMember guildMember, int deltaPoints);

        /// <summary>
        /// Modifies the points the user <see cref="guildMember"/> has by <see cref="amount"/>.
        /// Also changes the <see cref="GuildMember.CurrencyLastClaimed"/> property to <see cref="DateTimeOffset.UtcNow"/>.
        /// If cooldown has not expired - returns time remaining, otherwise - null.
        /// </summary>
        /// <param name="guildMember"><see cref="GuildMember"/> to modify.</param>
        /// <param name="amount">Amount of points to modify by.</param>
        /// <param name="cooldown">Time (in seconds) between timely claims.</param>
        /// <returns>Time remaining until next claim. If null - points were given out.</returns>
        Task<TimeSpan?> GrantTimelyRewardAsync(GuildMember guildMember, int amount, int cooldown);
    }
}
