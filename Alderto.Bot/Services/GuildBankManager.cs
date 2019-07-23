using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public class GuildBankManager : IGuildBankManager
    {
        private readonly IAldertoDbContext _context;

        public GuildBankManager(IAldertoDbContext context)
        {
            _context = context;
        }

        public async Task AddDonationAsync(GuildMember member, string donation)
        {
            _context.Attach(member);

            await _context.GuildMemberDonations.AddAsync(new GuildMemberDonation
            {
                MemberId = member.MemberId,
                GuildId = member.GuildId,
                DonationDate = DateTimeOffset.UtcNow,
                Donation = donation
            });

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GuildMemberDonation>> GetDonationsAsync(GuildMember member)
        {
            _context.Attach(member);

            await _context.Entry(member).Collection(m => m.Donations).LoadAsync();
            return member.Donations;
        }

        public Task<GuildMemberDonation> GetDonationAsync(int id)
        {
            return _context.GuildMemberDonations.FindAsync(id);
        }

        public async Task RemoveDonationAsync(GuildMemberDonation donation)
        {
            _context.Remove(donation);
            await _context.SaveChangesAsync();
        }
    }
}