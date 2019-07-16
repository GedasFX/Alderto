using System.Threading.Tasks;
using Alderto.Bot.Exceptions;

namespace Alderto.Bot.Services
{
    public interface ICustomCommandProviderService
    {
        /// <summary>
        /// Executes the command to be run on Lua kernel.
        /// </summary>
        /// <param name="guildId">Command owning guild Id.</param>
        /// <param name="cmdName">Command trigger. First word after .cc</param>
        /// <param name="args">null padded array of arguments. For example: { null, 0, 1, "cabbage soup" }, or { null } (if no arguments).
        ///     Null will later be converted into $"_{guildId}_{cmdName}".</param>
        /// <returns>Whatever the Lua function returned.</returns>
        Task<object[]> RunCommandAsync(ulong guildId, string cmdName, params object[] args);

        /// <summary>
        /// Executes the command to be run on Lua kernel.
        /// </summary>
        /// <param name="functionName">Lua registered function name to run. Looks like "_{guildId}_{cmdName}"</param>
        /// <param name="args">null padded array of arguments. For example: { null, 0, 1, "cabbage soup" }, or { null } (if no arguments).
        ///     Null will later be converted into <see cref="functionName"/>.</param>
        /// <exception cref="LuaCommandNotFoundException">Thrown when function with name <see cref="functionName"/> is not found.</exception>
        /// <returns>Whatever the Lua function returned.</returns>
        Task<object[]> RunCommandAsync(string functionName, params object[] args);

        /// <summary>
        /// Reloads all commands registered to the specified guild.
        /// </summary>
        /// <param name="guildId">Id of guild, where commands need to be reloaded.</param>
        /// <returns></returns>
        Task ReloadCommands(ulong guildId);

        /// <summary>
        /// Registers a command to the Lua kernel.
        /// </summary>
        /// <param name="functionName">Lua registered function name to run. Looks like "_{guildId}_{cmdName}".</param>
        /// <param name="code">Function code. Does not include the header or ending, just the body.</param>
        /// <returns></returns>
        Task RegisterCommand(string functionName, string code);
    }
}