using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Assistant.Core.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Return some default content");
        }
    }
}
