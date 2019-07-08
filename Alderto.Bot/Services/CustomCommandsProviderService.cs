using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Bot.Exceptions;
using Alderto.Data;
using Microsoft.EntityFrameworkCore;
using NLua;

namespace Alderto.Bot.Services
{
    public class CustomCommandsProviderService
    {
        /// <summary>
        /// Maximum amount of time allowed for a Lua Kernel to handle a command.
        /// Safeguard against code that is too heavy.
        /// </summary>
        private const int CustomCommandExecTimeout = 100;

        private readonly IAldertoDbContext _context;
        private readonly Lua _luaState;
        private readonly Dictionary<string, LuaFunction> _commands;

        public CustomCommandsProviderService(IAldertoDbContext context)
        {
            _context = context;
            _luaState = new Lua();
            _commands = new Dictionary<string, LuaFunction>();

            // Load Lua code
            _luaState.LoadCLRPackage();
            _luaState.DoString("import ('Alderto.Bot', 'Alderto.Bot.Commands')");

            // Prevent additional namespaces to be added
            _luaState.DoString("import = function () end");
        }

        /// <summary>
        /// Executes the command to be run on Lua kernel.
        /// </summary>
        /// <param name="guildId">Command owning guild Id.</param>
        /// <param name="cmdName">Command trigger. First word after .cc</param>
        /// <param name="args">null padded array of arguments. For example: { null, 0, 1, "cabbage soup" }, or { null } (if no arguments).
        ///     Null will later be converted into $"_{guildId}_{cmdName}".</param>
        /// <returns>Whatever the Lua function returned.</returns>
        public async Task<object[]> RunCommandAsync(ulong guildId, string cmdName, params object[] args)
        {
            return await RunCommandAsync($"_{guildId}_{cmdName}", args);
        }

        /// <summary>
        /// Executes the command to be run on Lua kernel.
        /// </summary>
        /// <param name="functionName">Lua registered function name to run. Looks like "_{guildId}_{cmdName}"</param>
        /// <param name="args">null padded array of arguments. For example: { null, 0, 1, "cabbage soup" }, or { null } (if no arguments).
        ///     Null will later be converted into <see cref="functionName"/>.</param>
        /// <exception cref="LuaCommandNotFoundException">Thrown when function with name <see cref="functionName"/> is not found.</exception>
        /// <returns>Whatever the Lua function returned.</returns>
        public async Task<object[]> RunCommandAsync(string functionName, params object[] args)
        {
            // Replace null with function name (now is later, see XML)
            args[0] = functionName;

            // Check if command exists. If not - throw exception.
            if (!_commands.TryGetValue(functionName, out var func))
                throw new LuaCommandNotFoundException($"Function {functionName} does not exist or is not registered.");

            // Function exists. Execute it.
            using (var c = new CancellationTokenSource())
            {
                // Safeguard agains infinite loops and such.
                c.CancelAfter(CustomCommandExecTimeout);

                // Wrap an array of objects into a new array so the fucntion in Lua has args parameter as an array.
                return await Task.Run(NewMethod(args, func), c.Token);
            }
        }

        private static System.Func<object[]> NewMethod(object[] args, LuaFunction func) => () => func.Call(new object[] { args });

        /// <summary>
        /// Reloads all commands registered to the specified guild.
        /// </summary>
        /// <param name="guildId">Id of guild, where commands need to be reloaded.</param>
        /// <returns></returns>
        public async Task ReloadCommands(ulong guildId)
        {
            var guild = await _context.Guilds
                .Include(g => g.CustomCommands)
                .SingleOrDefaultAsync(g => g.Id == guildId);

            if (/* guild.PremiumUntil != null */ /* premium feature. For now free */ true)
            {
                foreach (var cmd in guild.CustomCommands)
                {
                    await RegisterCommand(functionName: $"_{guildId}_{cmd.TriggerKeyword}", code: cmd.LuaCode);
                }
            }
        }

        /// <summary>
        /// Registers a command to the Lua kernel.
        /// </summary>
        /// <param name="functionName">Lua registered function name to run. Looks like "_{guildId}_{cmdName}".</param>
        /// <param name="code">Function code. Does not include the header or ending, just the body.</param>
        /// <returns></returns>
        public async Task RegisterCommand(string functionName, string code)
        {
            using (var c = new CancellationTokenSource())
            {
                c.CancelAfter(CustomCommandExecTimeout);
                await Task.Run(action: () =>
                {
                    _luaState.DoString($"function {functionName} (args) {code} end");
                    _commands[functionName] = _luaState.GetFunction(functionName);
                }, cancellationToken: c.Token);
            }
        }
    }
}