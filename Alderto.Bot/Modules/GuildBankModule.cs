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
    [Group, Alias("GuildBank", "GB")]
    public class GuildBankModule : ModuleBase<SocketCommandContext>
    {
        [Group("Donations"), Alias("Donation")]
        public class DonationsModule : ModuleBase<SocketCommandContext>
        {
            private readonly IGuildBankManager _guildBankManager;
            private readonly IGuildUserManager _userManager;

            public DonationsModule(IGuildBankManager guildBankManager, IGuildUserManager userManager)
            {
                _guildBankManager = guildBankManager;
                _userManager = userManager;
            }

            [Command]
            [Summary("Checks the user's donations.")]
            public async Task List(
                [Summary("User to check donations of.")] IGuildUser donor = null)
            {
                if (donor == null)
                    donor = (IGuildUser)Context.Message.Author;

                var user = await _userManager.GetGuildMemberAsync(donor);

                var donations = (await _guildBankManager.GetDonationsAsync(user)).ToArray();
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

            [Command("Add")]
            [Summary("Registers a donation to the guild.")]
            [RequireRole("Admin")]
            public async Task Add(
                [Summary("User who has given a donation.")] IGuildUser donor,
                [Summary("The donation given.")] [Remainder] [MaxLength(100)] string donation)
            {
                if (string.IsNullOrWhiteSpace(donation) || donation.Length > 100)
                    return;

                var user = await _userManager.GetGuildMemberAsync(donor);
                await _guildBankManager.AddDonationAsync(user, donation);

                await this.ReplySuccessEmbedAsync($"{donor.Mention}'s donation of **{donation}** has been registered.");
            }

            [Command("Remove")]
            public async Task Remove(int id)
            {
                var donation = await _guildBankManager.GetDonationAsync(id);
                if (donation?.GuildId != Context.Guild.Id)
                {
                    await this.ReplyErrorEmbedAsync($"No donation with the id of **{id}** was found.");
                    return;
                }

                // Donation exists and it is from the current guild. Remove.
                await _guildBankManager.RemoveDonationAsync(donation);

                await this.ReplySuccessEmbedAsync(
                    $"Successfully removed <!{donation.MemberId}>'s donation of **{donation.Donation}**.");
            }
        }
    }
}