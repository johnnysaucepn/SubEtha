using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Thumb.Plugin.Assistant.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return Ok("Return some default content");
        }
    }
}
