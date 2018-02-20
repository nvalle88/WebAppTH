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
    public class EjemplosController : Controller
    {

        private readonly IApiServicio apiServicio;


        public EjemplosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }


        public async Task<IActionResult> Index()
        {
            var lista= await apiServicio.Listar<Ejemplo>(new Uri(WebApp.BaseAddress), "api/Ejemplos/ListarEjemplos");
            return View(lista);
        }
    }
}