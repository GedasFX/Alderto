using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace Alderto.Services
{
    public interface INewsProvider
    {
        /// <summary>
        /// Gets the <see cref="count"/> latest news
        /// </summary>
        /// <param name="count">Amount of news to fetch</param>
        /// <returns><see cref="count"/> latest news.</returns>
        Task<IEnumerable<IMessage>> GetLatestNewsAsync(int count);
    }
}