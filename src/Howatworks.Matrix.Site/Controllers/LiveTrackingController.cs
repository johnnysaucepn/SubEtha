using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Matrix.Site.Controllers
{
    public class LiveTrackingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}