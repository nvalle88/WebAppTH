using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var usuario = new UsuarioViewModel { NombreUsuarioActual = NombreUsuario};

                    lista = await apiServicio.ObtenerElementoAsync1<List<InduccionViewModel>>(usuario, new Uri(WebApp.BaseAddress)
                             , "api/Induccion/ListarEstadoInduccionEmpleados");


                    return View(lista);
                }

                return RedirectToAction("Login", "Login");


            }
            catch (Exception ex)
            {
                return BadRequest();

            }
        }
    }
}