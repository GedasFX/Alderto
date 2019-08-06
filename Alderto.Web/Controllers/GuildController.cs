using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize]
    public class GuildController : Controller
    {
        public IActionResult Index()
        {
            return null;
        }
    }
}