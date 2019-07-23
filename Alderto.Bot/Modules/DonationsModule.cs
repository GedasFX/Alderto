using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Bot.Extensions;
using Alderto.Bot.Preconditions;
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
        [Summary("Registers a donation to the guild.")]
        [RequireRole("Admin")]
        public async Task Donated(
            [Summary("User who has given a donation.")] IGuildUser donor,
            [Summary("The donation given.")] [Remainder] [MaxLength(100)] string donation)
        {
            if (string.IsNullOrWhiteSpace(donation) || donation.Length > 100)
                return;

            var user = await _userManager.GetGuildMemberAsync(donor);
            await _donationsManager.AddDonationAsync(user, donation);

            await this.ReplySuccessEmbedAsync($"{donor.Mention}'s donation of **{donation}** has been registered.");
        }

        [Command("Donations")]
        [Summary("Checks the user's donations.")]
        public async Task Donations(
            [Summary("User to check donations of.")] IGuildUser donor = null)
        {
            if (donor == null)
                donor = (IGuildUser)Context.Message.Author;

            var user = await _userManager.GetGuildMemberAsync(donor);

            var donations = (await _donationsManager.GetDonationsAsync(user)).ToArray();
            if (donations.Length == 0)
                await this.ReplyErrorEmbedAsync($"{donor.Mention} has not made any donations.");
            else
                await this.ReplySuccessEmbedAsync($"{donor.Mention} has made the following donations:", builder =>
                {
                    foreach (var donation in donations)
                    {
                        builder.AddField($"{donation.Id}: {donation.DonationDate}", $"**{donation.Donation}**");
                    }
                });
        }
    }
}