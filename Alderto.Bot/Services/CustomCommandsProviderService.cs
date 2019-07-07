using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using NLua;

namespace Alderto.Bot.Services
{
    public class CustomCommandsProviderService
    {
        private const int CustomCommandExecTimeout = 500;

        private readonly IAldertoDbContext _context;
        private readonly Lua _luaState;
        private readonly Dictionary<(ulong, string), LuaFunction> _commands;

        public CustomCommandsProviderService(IAldertoDbContext context, Lua luaState)
        {
            _context = context;
            _luaState = luaState;
            _commands = new Dictionary<(ulong, string), LuaFunction>();

        }

        public async Task RunCommand(ulong guildId, string cmdName, string args)
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
            var guild = await _context.Guilds.FindAsync(guildId);
            if (guild.IsPremium)
            {

            }
        }

        public void RegisterCommand(ulong guildId, string cmdName, string code)
        {
            var functionName = $"_{(uint)_commands.First().Key.GetHashCode()}";
            _luaState.DoString($"function {functionName} (args) {code} end");
        }
    }
}