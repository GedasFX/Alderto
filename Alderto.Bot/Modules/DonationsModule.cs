using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Services;
using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    public class DonationsModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDonationsManager _donationsManager;
        private readonly IGuildUserManager _userManager;

        public DonationsModule(IDonationsManager donationsManager, IGuildUserManager userManager)
        {
            _donationsManager = donationsManager;
            _userManager = userManager;
        }

        [Command("Donated")]
        public async Task Donated(IGuildUser donor, [Remainder] string donation)
        {
            if (string.IsNullOrWhiteSpace(donation) || donation.Length > 100)
                return;

            var user = await _userManager.GetGuildMemberAsync(donor);
            await _donationsManager.AddDonationAsync(user, donation);

            await this.ReplySuccessEmbedAsync($"{donor.Mention}'s donation of **{donation}** has been registered.");
        }

        [Command("Donations")]
        public async Task Donations(IGuildUser donor = null)
        {
            if (donor == null)
                donor = (IGuildUser)Context.Message.Author;

            var user = await _userManager.GetGuildMemberAsync(donor);

            var donations = await _donationsManager.ListDonationsAsync(user);
            if (donations == null)
                await this.ReplyErrorEmbedAsync("User has not made any donations.");
            else
                await this.ReplySuccessEmbedAsync("User has made the following donations:", builder =>
                {
                    foreach (var donation in donations)
                    {
                        builder.AddField($"{donation.DonationDate}", $"{donation.Donation}");
                    }
                });
        }
    }
}