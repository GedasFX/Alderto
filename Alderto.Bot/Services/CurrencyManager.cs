using System;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly IAldertoDbContext _context;

        // TODO: Decide if this is necessary.
        private const bool AllowNegativePoints = true;

        public CurrencyManager(IAldertoDbContext context)
        {
            _context = context;
        }

        public async Task ModifyPointsAsync(GuildMember guildMember, int deltaPoints)
        {
            var oldCurrencyCount = guildMember.CurrencyCount;

            if (deltaPoints > 0 && oldCurrencyCount > 0 && oldCurrencyCount + deltaPoints < 0)
            {
                // overflow, set to max value instead.
                guildMember.CurrencyCount = int.MaxValue;
            }
            else if (deltaPoints < 0 && oldCurrencyCount < 0 && oldCurrencyCount + deltaPoints > 0)
            {
                //underflow, set to min value instead
                guildMember.CurrencyCount = int.MinValue;
            }
            else
            {
                // Add currency to the user
                guildMember.CurrencyCount += deltaPoints;
            }

            if (!AllowNegativePoints && guildMember.CurrencyCount < 0)
            {
                guildMember.CurrencyCount = 0;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<TimeSpan?> GrantTimelyRewardAsync(GuildMember member, int amount, int cooldown)
        {
            var timeRemaining = member.CurrencyLastClaimed.AddSeconds(cooldown) - DateTimeOffset.UtcNow;

            // If time remaining is positive, that means cooldown hasn't expired yet.
            if (timeRemaining.Ticks > 0)
                return timeRemaining;

            // Cooldown expired. Update user.
            member.CurrencyLastClaimed = DateTimeOffset.UtcNow;
            member.CurrencyCount += amount;

            await _context.SaveChangesAsync();

            return null;
        }
    }
}