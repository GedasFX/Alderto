using System.Threading.Tasks;
using Discord.WebSocket;

namespace Alderto.Bot.Services
{
    public interface ICommandHandler
    {
        Task InstallCommandsAsync();
        Task HandleCommandAsync(SocketMessage messageParam);
    }
}