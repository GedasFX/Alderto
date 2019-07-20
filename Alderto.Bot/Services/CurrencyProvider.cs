using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Extensions;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public class CurrencyProvider : ICurrencyProvider
    {
        // TODO: Decide if this is necessary.
        private const bool AllowNegativePoints = true;

        private readonly IAldertoDbContext _context;
        

        public CurrencyProvider(IAldertoDbContext context)
        {
            _context = context;
        }

        public async Task ModifyPointsAsync(ulong memberId, ulong guildId, int deltaPoints, bool saveChanges = true)
        {
            await ModifyPointsAsync(await _context.GetGuildMemberAsync(guildId, memberId), deltaPoints, saveChanges);
        }

        public async Task ModifyPointsAsync(GuildMember guildMember, int deltaPoints, bool saveChanges = true)
        {
            ModifyPoints(guildMember, deltaPoints);

            if (saveChanges)
                await _context.SaveChangesAsync();
        }

        public async Task ModifyPointsAsync(IEnumerable<GuildMember> guildMembers, int deltaPoints, bool saveChanges = true)
        {
            foreach (var guildMember in guildMembers)
            {
                ModifyPoints(guildMember, deltaPoints);
            }

            if (saveChanges)
                await _context.SaveChangesAsync();
        }

        private static void ModifyPoints(GuildMember guildMember, int deltaPoints)
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
        }
    }
}