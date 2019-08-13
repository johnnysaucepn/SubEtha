using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Matrix.Site.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}