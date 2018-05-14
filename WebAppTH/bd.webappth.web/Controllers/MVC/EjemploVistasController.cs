using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bd.webappth.web.Controllers.MVC
{
    public class EjemploVistasController : Controller
    {
        private readonly IApiServicio apiServicio;


        public EjemploVistasController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public JsonResult ListarPaises()
        {
            var Lista = new List<Pais> { new Pais { IdPais = 1, Nombre = "Cuba" }, new Pais { IdPais = 1, Nombre = "Ecuador" }, new Pais { IdPais = 1, Nombre = "España" } };
            return Json(Lista);
        }
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Index(EjemploVista ejemploVista)
        {

            return View();
        }
    }


}