using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bd.webappth.web.Controllers.MVC
{
    public class InduccionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}