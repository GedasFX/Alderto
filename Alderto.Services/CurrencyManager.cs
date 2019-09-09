using System;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Services
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly IAldertoDbContext _context;

        public CurrencyManager(IAldertoDbContext context)
        {
            _context = context;
        }

        public async Task ModifyPointsAsync(GuildMember guildMember, int deltaPoints)
        {
            guildMember.CurrencyCount += deltaPoints;
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