using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.Impl
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly AldertoDbContext _context;

        public CurrencyManager(AldertoDbContext context)
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

        public async Task<IEnumerable<GuildMember>> GetRichestUsersAsync(ulong guildId, int take = 10, int skip = 0)
        {
            return await _context.GuildMembers.AsQueryable()
                .Where(g => g.GuildId == guildId)
                .OrderByDescending(g => g.CurrencyCount)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}