using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.ViewModels;
using bd.webappth.entidades.Utils;

namespace bd.webappth.web.Controllers.MVC
{
    public class CapacitacionPlanificacionesController : Controller
    {

        private readonly IApiServicio apiServicio;


        public CapacitacionPlanificacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }
        private void InicializarMensaje(string mensaje)

        {

            if (mensaje == null)

            {

                mensaje = "";

            }

            ViewData["Error"] = mensaje;

        }

        public async Task<IActionResult> Index()
        {
            ViewData["ListaCapacitacionTemario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<CapacitacionPlanificacionViewModel>(new Uri(WebApp.BaseAddress), "api/CapacitacionesTemarios/ListarCapacitacionesTemarios"), "IdCapacitacionTemario", "Tema");

            return View();
        }
    }
}