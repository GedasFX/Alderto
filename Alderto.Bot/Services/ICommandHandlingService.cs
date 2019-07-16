using System.Threading.Tasks;

namespace Alderto.Bot.Services
{
    public interface ICommandHandlingService
    {
        Task InstallCommandsAsync();
    }
}