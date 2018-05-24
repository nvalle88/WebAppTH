using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using Newtonsoft.Json;
using bd.webappth.entidades.Utils;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.Servicios;
using System.Security.Claims;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitarVacacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitarVacacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }




        public async Task<IActionResult> Create()
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario };

                    var modelo = await apiServicio.ObtenerElementoAsync1<SolicitudVacacionesViewModel>(
                        enviar,
                        new Uri(WebApp.BaseAddress),
                        "api/SolicitudVacaciones/CrearSolicitudesVacaciones");

                    return View(modelo);

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudVacacionesViewModel modelo)
        {

            try
            {
                if (modelo.PlanAnual == true && modelo.IdSolicitudPlanificacionVacaciones < 1) {

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SeleccioneSolicitudPlanificacionVacaciones}|{"10000"}";

                    var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                    if (claim.IsAuthenticated == true)
                    {
                        var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                        var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario };

                        modelo = await apiServicio.ObtenerElementoAsync1<SolicitudVacacionesViewModel>(
                            enviar,
                            new Uri(WebApp.BaseAddress),
                            "api/SolicitudVacaciones/CrearSolicitudesVacaciones");

                        return View(modelo);

                    }

                    return RedirectToAction("Login", "Login");
                }

                var response = await apiServicio.InsertarAsync(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/SolicitudVacaciones/InsertarSolicitudVacaciones"
                    );

                if (response.IsSuccess)
                {

                    return this.Redireccionar(
                            "SolicitarVacaciones",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                return View(modelo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Index()
        {

            var lista = new List<SolicitudVacacionesViewModel>();

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario };

                    lista = await apiServicio.Listar<SolicitudVacacionesViewModel>(
                        enviar,
                        new Uri(WebApp.BaseAddress),
                        "api/SolicitudVacaciones/ListarSolicitudesVacacionesViewModel");

                    return View(lista);

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/SolicitudVacaciones");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                        id,
                        new Uri(WebApp.BaseAddress),
                        "api/SolicitudVacaciones/ObtenerSolicitudVacacionesViewModel");


                    if (respuesta.IsSuccess)
                    {
                        respuesta.Resultado = JsonConvert.DeserializeObject<SolicitudVacacionesViewModel>(respuesta.Resultado.ToString());

                        return View(respuesta.Resultado);
                    }

                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SolicitudVacacionesViewModel modelo)
        {

            try
            {
                if (modelo.PlanAnual == true && modelo.IdSolicitudPlanificacionVacaciones < 1)
                {

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SeleccioneSolicitudPlanificacionVacaciones}|{"10000"}";

                    var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                    if (claim.IsAuthenticated == true)
                    {
                        var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                        var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario};

                        modelo = await apiServicio.ObtenerElementoAsync1<SolicitudVacacionesViewModel>(
                            enviar,
                            new Uri(WebApp.BaseAddress),
                            "api/SolicitudVacaciones/ObtenerSolicitudVacacionesViewModel");

                        return View(modelo);

                    }

                    return RedirectToAction("Login", "Login");
                }

                var response = await apiServicio.EditarAsync<Response>(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/SolicitudVacaciones/EditarSolicitudVacaciones"
                    );

                if (response.IsSuccess)
                {

                    return this.Redireccionar(
                            "SolicitarVacaciones",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                return View(modelo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<Empleado> ObtenerEmpleadoLogueado(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.ObtenerElementoAsync1<Empleado>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerEmpleadoLogueado");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new Empleado();
            }

        }


    }
}