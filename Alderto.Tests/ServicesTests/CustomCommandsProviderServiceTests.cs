using System.Threading.Tasks;
using Alderto.Bot.Modules;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Tests.MockedEntities;
using Microsoft.EntityFrameworkCore;
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
if (num1 > num2) then
  result = num1;
else
  result = num2;
end
"
            });
            await _context.SaveChangesAsync();

            await _service.ReloadCommands(1);

            Assert.Equal(4, (int)(await _service.RunCommandAsync(1, "test", "4, 2"))[0]);
        }
    }
}
