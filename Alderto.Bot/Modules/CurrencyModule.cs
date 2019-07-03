using System.Threading.Tasks;
using Alderto.Bot.Data;
using Discord.Commands;

namespace Alderto.Bot.Modules
{
    class CurrencyModule : ModuleBase<SocketCommandContext>
    {
        private readonly SqliteDbContext _context;

        public CurrencyModule(SqliteDbContext context)
        {
            _context = context;
        }

        [Command("helloworld")]
        [Summary("Echoes text")]
        public Task SayAsync([Remainder] [Summary("The text to echo")]
            string echo) => ReplyAsync(echo);
    }
}
