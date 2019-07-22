using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public class DonationsManager
    {
        private readonly IAldertoDbContext _context;

        public DonationsManager(IAldertoDbContext context)
        {
            _context = context;
        }

        public void AddDonation(GuildMember member, string donation)
        {
            _context.Attach(member);

            _context.GuildMemberDonations.AddAsync(new GuildMemberDonation
            {
                GuildMemberId = member.Id,
                Donation = donation
            });
        }
    }
}