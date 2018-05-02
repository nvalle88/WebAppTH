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

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var modeloEnviar = new AccionesPersonalPorEmpleadoViewModel
                    {
                        NombreUsuarioActual = NombreUsuario
                    };


                    var modelo = await apiServicio.Listar<AccionPersonalViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/ListarEmpleadosConAccionPersonal");

                    return View(modelo);

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(string mensaje, int id)
        {
            try
            {
                InicializarMensaje(mensaje);

                var modelo = new AccionPersonalViewModel { IdAccionPersonal = id };

                var respuesta = await apiServicio.ObtenerElementoAsync<AccionPersonalViewModel>(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerAccionPersonalViewModel");

                if (respuesta.IsSuccess)
                {
                    modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());


                    var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

                    ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");



                    var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacion");

                    ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


                    return View(modelo);

                }

                return BadRequest();

            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccionPersonalViewModel accionPersonalViewModel)
        {
            try
            {

                var modeloEnviar = new AccionPersonal
                {
                    IdAccionPersonal = accionPersonalViewModel.IdAccionPersonal,
                    IdEmpleado = accionPersonalViewModel.DatosBasicosEmpleadoViewModel.IdEmpleado,
                    
                    Estado = accionPersonalViewModel.Estado,

                };

                var respuesta = await apiServicio.InsertarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/EditarAccionPersonal");

                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = respuesta.Message });
                }

                return View(accionPersonalViewModel);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}