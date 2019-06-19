using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Thumb.Plugin.Assistant.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Return some default content");
        }
    }
}
