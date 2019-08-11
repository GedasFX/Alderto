using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services
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
            //_context.Attach(member);

            //await _context.GuildMemberTransactions.AddAsync(new GuildBankTransaction
            //{
            //    MemberId = member.MemberId,
            //    GuildId = member.GuildId,
            //    TransactionDate = DateTimeOffset.UtcNow,
            //    Donation = donation
            //});

            //await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GuildBankTransaction>> GetDonationsAsync(GuildMember member)
        {
            return null;
            //_context.Attach(member);

            //await _context.Entry(member).Collection(m => m.Donations).LoadAsync();
            //return member.Donations;
        }

        public Task<GuildBankTransaction> GetDonationAsync(int id)
        {
            return null;
            //return _context.GuildMemberDonations.FindAsync(id);
        }

        public async Task RemoveDonationAsync(GuildBankTransaction donation)
        {
            _context.Remove(donation);
            await _context.SaveChangesAsync();
        }
    }
}