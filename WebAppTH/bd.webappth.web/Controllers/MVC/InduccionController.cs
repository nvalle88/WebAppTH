using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bd.webappth.web.Controllers.MVC
{
    public class InduccionController : Controller
    {

        private readonly IApiServicio apiServicio;

        public InduccionController(IApiServicio apiServicio)
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

        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);

            var lista = new List<InduccionViewModel>();
            

            try
            {
                lista = await apiServicio.Listar<InduccionViewModel>(new Uri(WebApp.BaseAddress)
                         , "api/Induccion/ListarEstadoInduccionEmpleados");

                
                return View(lista);


            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;

                return View(lista);

            }
        }
    }
}