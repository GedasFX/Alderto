using System.Linq;
using System.Threading.Tasks;
using Alderto.Services;
using Alderto.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    public class NewsController : ApiControllerBase
    {
        private readonly INewsProvider _news;

        public NewsController(INewsProvider news)
        {
            _news = news;
        }

        [HttpGet("api/news"), AllowAnonymous]
        public async Task<IActionResult> GetLatestNewsAsync(int count = 10)
        {
            if (count > 100)
                return BadRequest(ErrorMessages.PayloadOver100);

            var news = await _news.GetLatestNewsAsync(count);

            return Content(news.Select(m => new ApiMessage(m)));
        }
    }
}