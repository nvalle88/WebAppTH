using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace bd.webappth.web.Controllers.MVC
{
    public class MovimientosPersonalController : Controller
    {
        private readonly IApiServicio apiServicio;


        public MovimientosPersonalController(IApiServicio apiServicio)
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

            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaMovimientos(string identificacion)
        {
            InicializarMensaje("");

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var modeloEnviar = new AccionesPersonalPorEmpleadoViewModel {

                        DatosBasicosEmpleadoViewModel = new DatosBasicosEmpleadoViewModel
                        {
                            Identificacion = identificacion
                        },

                        NombreUsuarioActual = NombreUsuario
                };
                    

                    var modelo = await apiServicio.ObtenerElementoAsync1<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/ListarAccionesPersonalPorEmpleado");

                    return View(modelo);
                    
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