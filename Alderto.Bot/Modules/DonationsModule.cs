using Discord;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    public class DonationsModule : ModuleBase<SocketCommandContext>
    {
        [Command("Donated")]
        public void Donated(IGuildUser donor, [Remainder] string donation)
        {

        }
    }
}