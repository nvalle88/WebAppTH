using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using Newtonsoft.Json;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Enumeradores;
using bd.webappseguridad.entidades.Enumeradores;
using System.Security.Claims;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitarPlanificacionVacacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitarPlanificacionVacacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Index()
        {
            
            var lista = new List<SolicitudPlanificacionVacacionesViewModel>();

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario};

                    lista = await apiServicio.Listar<SolicitudPlanificacionVacacionesViewModel>(
                        enviar,
                        new Uri(WebApp.BaseAddress),
                        "api/SolicitudPlanificacionVacaciones/ListarSolicitudesPlanificacionesVacaciones");

                    return View(lista);

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        
        public async Task<IActionResult> Create()
        {
            try {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario };

                    var modelo = await apiServicio.ObtenerElementoAsync1<SolicitudPlanificacionVacacionesViewModel>(
                        enviar,
                        new Uri(WebApp.BaseAddress),
                        "api/SolicitudPlanificacionVacaciones/CrearSolicitudesPlanificacionesVacaciones");

                    return View(modelo);

                }

                return RedirectToAction("Login", "Login");

            } catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudPlanificacionVacacionesViewModel modelo)
        {
            
            try {
                var response = await apiServicio.InsertarAsync(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/SolicitudPlanificacionVacaciones/InsertarSolicitudPlanificacionVacaciones"
                    );

                if (response.IsSuccess) {

                    return this.Redireccionar(
                            "SolicitarPlanificacionVacaciones",
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



        
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                        id,
                        new Uri(WebApp.BaseAddress),
                        "api/SolicitudPlanificacionVacaciones/ObtenerSolicitudPlanificacionVacacionesViewModel");

                    
                    if (respuesta.IsSuccess)
                    {
                        respuesta.Resultado = JsonConvert.DeserializeObject<SolicitudPlanificacionVacacionesViewModel>(respuesta.Resultado.ToString());

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
        public async Task<IActionResult> Edit(SolicitudPlanificacionVacacionesViewModel modelo)
        {

            try
            {
                var response = await apiServicio.InsertarAsync(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/SolicitudPlanificacionVacaciones/EditarSolicitudPlanificacionVacaciones"
                    );

                if (response.IsSuccess)
                {

                    return this.Redireccionar(
                            "SolicitarPlanificacionVacaciones",
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


        
        /*
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/SolicitudPlanificacionVacaciones");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de Solicitud Planificación Vacaciones eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = response.Message });
                //return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = WebApp.NombreAplicacion,
                    Message = "Eliminar Solicitud de Planificación Vacaciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }
        */
        private async Task<Empleado> ObtenerEmpleadoLogueado(string nombreUsuario)
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