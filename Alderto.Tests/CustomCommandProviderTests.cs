using System.Threading.Tasks;
using Alderto.Bot.Lua;
using Alderto.Bot.Lua.Exceptions;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Tests.MockedEntities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Alderto.Tests
{
    public class CustomCommandProviderTests
    {
        private readonly ICustomCommandProvider _provider;
        private readonly AldertoDbContext _context;

        public CustomCommandProviderTests()
        {
            var services = MockServices.ScopedServiceProvider;

            _context = services.GetService<AldertoDbContext>();
            _provider = services.GetService<ICustomCommandProvider>();
        }

        [Fact]
        public async Task TestExecution()
        {
            await _context.CustomCommands.AddAsync(new CustomCommand(1, "test", @"
if (args[0] == '_1_test') then
    arg1 = tonumber(args[1])
    if (arg1 > tonumber(args[2])) then
      return arg1
    else
      return args[2]
    end
else 
    return 3
end"
            ));

            await _context.Guilds.AddAsync(new Guild(1));
            await _context.SaveChangesAsync();

            await _provider.ReloadCommands(1);

            var cmd = await _provider.RunCommandAsync(guildId: 1, cmdName: "test", null!, "4", "2");
            Assert.Equal(expected: 4, (long)cmd[0]);

            await Assert.ThrowsAsync<LuaCommandNotFoundException>(async () =>
                await _provider.RunCommandAsync(guildId: 1, cmdName: "not found", null!, "4", "2"));
        }
    }
}
