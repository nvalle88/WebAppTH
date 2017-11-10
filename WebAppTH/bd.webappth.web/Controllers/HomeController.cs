using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading;

namespace bd.webappth.web.Controllers
{
    public class HomesController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            //var b = string.Empty;
            var claim = HttpContext.User.Identities.Where(x=>x.NameClaimType==ClaimTypes.Name).FirstOrDefault();
            var token= claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            //var identity = (ClaimsPrincipal.Current.Claims.Where(x => x.Type == ClaimTypes.Name)).First();
            //var name= claim.Name;
            return View();
        }

        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        public IActionResult Salir()
        {
            return View();
        }

    }
}
