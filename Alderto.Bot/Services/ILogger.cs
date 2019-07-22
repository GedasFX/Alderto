using System.Threading.Tasks;

namespace Alderto.Bot.Services
{
    public interface ILogger
    {
        /// <summary>
        /// Installs the logger.
        /// </summary>
        /// <returns></returns>
        Task InstallLogger();
    }
}