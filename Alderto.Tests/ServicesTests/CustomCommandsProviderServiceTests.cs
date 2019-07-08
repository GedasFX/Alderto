using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Tests.MockedEntities;
using Xunit;

namespace Alderto.Tests.ServicesTests
{
    public class CustomCommandsProviderServiceTests
    {
        private readonly CustomCommandsProviderService _service;
        private readonly IAldertoDbContext _context;

        public CustomCommandsProviderServiceTests()
        {
            _context = new MockDbContext();
            _service = new CustomCommandsProviderService(_context);
        }

        [Fact]
        public async Task TestExecution()
        {
            await _context.CustomCommands.AddAsync(new CustomCommand
            {
                GuildId = 1,
                TriggerKeyword = "test",
                LuaCode = @"
if (args[0] == '_1_test') then
    arg1 = tonumber(args[1])
    if (arg1 > tonumber(args[2])) then
      return arg1
    else
      return args[2]
    end
else 
    return 3
end
"
            });
            await _context.Guilds.AddAsync(new Guild(1));
            await _context.SaveChangesAsync();

            await _service.ReloadCommands(1);

            var cmd = await _service.RunCommandAsync(1, "test", null, "4", "2");
            Assert.Equal(4, (long)cmd[0]);
        }
    }
}
