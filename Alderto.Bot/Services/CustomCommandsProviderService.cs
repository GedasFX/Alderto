using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Microsoft.EntityFrameworkCore;
using NLua;

namespace Alderto.Bot.Services
{
    public class CustomCommandsProviderService
    {
        private const int CustomCommandExecTimeout = 100;

        private readonly IAldertoDbContext _context;
        private readonly Lua _luaState;
        private readonly Dictionary<(ulong, string), LuaFunction> _commands;

        public CustomCommandsProviderService(IAldertoDbContext context)
        {
            _context = context;
            _luaState = new Lua();
            _commands = new Dictionary<(ulong, string), LuaFunction>();

            // Load Lua code
            _luaState.LoadCLRPackage();
            _luaState.DoString("import ('Alderto.Bot', 'Alderto.Bot.Commands')");

            // Prevent additional namespaces to be added
            _luaState.DoString("import = function () end");
        }

        public async Task RunCommandAsync(ulong guildId, string cmdName, string args)
        {
            _commands.TryGetValue(ValueTuple.Create(guildId, cmdName), out var func);

            using (var c = new CancellationTokenSource())
            {
                c.CancelAfter(CustomCommandExecTimeout);
                await Task.Run(() => func?.Call(args), c.Token);
            }
        }

        public async Task ReloadCommands(ulong guildId)
        {
            var guild = await _context.Guilds
                .Include(g => g.CustomCommands)
                .SingleOrDefaultAsync(g => g.Id == guildId);

            if (/* guild.PremiumUntil != null */ /* premium feature. For now free */ true)
            {
                foreach (var cmd in guild.CustomCommands)
                {
                    await RegisterCommand(guildId: guildId, cmdName: cmd.TriggerKeyword, code: cmd.LuaCode);
                }
            }
        }

        public async Task RegisterCommand(ulong guildId, string cmdName, string code)
        {
            var functionName = $"_{(uint)_commands.First().Key.GetHashCode()}";
            using (var c = new CancellationTokenSource())
            {
                c.CancelAfter(CustomCommandExecTimeout);
                await Task.Run(action: () =>
                {
                    _luaState.DoString($"function {functionName} (args) {code} end");
                    _commands[(guildId, cmdName)] = _luaState.GetFunction(functionName);
                }, cancellationToken: c.Token);
            }
        }
    }
}