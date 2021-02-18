using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Assistant.WebSockets.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Return some default content");
        }
    }
}
