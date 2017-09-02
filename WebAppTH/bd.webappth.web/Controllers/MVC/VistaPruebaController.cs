using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.entidades.Negocio;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;

namespace bd.webappth.web.Controllers.MVC
{
    public class VistaPruebaController : Controller
    {
        private readonly IApiServicio apiServicio;





        public VistaPruebaController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Index()
        {
            var lista = new List<AnoExperiencia>();

            lista = await apiServicio.Listar<AnoExperiencia>(new Uri(WebApp.BaseAddress)
                                                                , "/api/AnosExperiencia/ListarAnosExperiencia");
            return View(lista);

        }

        public async Task<JsonResult> Listar()
        {

            var lista = new List<AnoExperiencia>();

            lista = await apiServicio.Listar<AnoExperiencia>(new Uri(WebApp.BaseAddress)
                                                                , "/api/AnosExperiencia/ListarAnosExperiencia");
            return Json(lista);
        }
    }
}