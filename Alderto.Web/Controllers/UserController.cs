using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize, Route("api/user")]
    public class UserController : Controller
    {
        public UserController()
        {
            
        }

        [HttpGet, Route("guilds")]
        public IActionResult Index()
        {
            return View();
        }
    }
}