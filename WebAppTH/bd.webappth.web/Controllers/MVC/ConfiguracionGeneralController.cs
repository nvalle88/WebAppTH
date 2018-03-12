using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bd.webappth.web.Controllers.MVC
{
    public class ConfiguracionGeneralController : Controller
    {

        private readonly IApiServicio apiServicio;


        public ConfiguracionGeneralController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}