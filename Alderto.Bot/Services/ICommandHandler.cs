using System.Threading.Tasks;
using Discord.WebSocket;

namespace Alderto.Bot.Services
{
    public interface ICommandHandler
    {
        /// <summary>
        /// Registers all modules as to how commands should be handled.
        /// </summary>
        Task InstallCommandsAsync();

        /// <summary>
        /// Handles socket messages by executing a previously installed command.
        /// </summary>
        /// <param name="messageParam">Message to handle.</param>
        /// <returns></returns>
        Task HandleCommandAsync(SocketMessage messageParam);
    }
}