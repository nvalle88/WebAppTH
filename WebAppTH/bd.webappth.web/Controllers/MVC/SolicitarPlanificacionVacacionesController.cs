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
using Microsoft.AspNetCore.Mvc.Rendering;

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
                var response = await apiServicio.EditarAsync<Response>(
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
                var response = await apiServicio.EditarAsync<Response>(
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



        public async Task<IActionResult> IndexJefe()
        {

            var modelo = new DependenciaDatosViewModel();

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(
                        NombreUsuario,
                        new Uri(WebApp.BaseAddress),
                        "api/Empleados/EmpleadoSegunNombreUsuario");

                    if (empleado.EsJefe == true)
                    {

                        var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario };

                        modelo = await apiServicio.ObtenerElementoAsync1<DependenciaDatosViewModel>(
                            enviar,
                            new Uri(WebApp.BaseAddress),
                            "api/Dependencias/ObtenerDependenciaDatosViewModelPorUsuarioActual");

                        return View(modelo);
                    }

                    return this.Redireccionar(
                            "SolicitarPlanificacionVacaciones",
                            "Index",
                            $"{Mensaje.Informacion}|{Mensaje.AccesoNoAutorizado}"
                         );


                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> ConsultarDependencia(int id)
        {

            var modelo = new DependenciaDatosViewModel();

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(
                        NombreUsuario,
                        new Uri(WebApp.BaseAddress),
                        "api/Empleados/EmpleadoSegunNombreUsuario");

                    if (empleado.EsJefe == true)
                    {

                        var enviar = new IdFiltrosViewModel { IdDependencia = id };

                        modelo = await apiServicio.ObtenerElementoAsync1<DependenciaDatosViewModel>(
                            enviar,
                            new Uri(WebApp.BaseAddress),
                            "api/Dependencias/ObtenerDependenciaDatosViewModelPorIdDependencia");

                        return View(modelo);
                    }

                    return this.Redireccionar(
                            "SolicitarPlanificacionVacaciones",
                            "Index",
                            $"{Mensaje.Informacion}|{Mensaje.AccesoNoAutorizado}"
                         );


                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> Solicitudes(int id)
        {

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario, IdEmpleado = id };

                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                        enviar,
                        new Uri(WebApp.BaseAddress),
                        "api/SolicitudPlanificacionVacaciones/ObtenerListaSolicitudPlanificacionVacacionesViewModelPorEmpleado");

                    if (respuesta.IsSuccess)
                    {
                        var lista = JsonConvert.DeserializeObject<List<SolicitudPlanificacionVacacionesViewModel>>(respuesta.Resultado.ToString());

                        return View(lista);
                    }

                    return this.Redireccionar(
                            "SolicitarPlanificacionVacaciones",
                            "Index",
                            $"{Mensaje.Aviso}|{respuesta.Message}"
                         );
                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> Aprobar(string id)
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

                        await CargarCombosAprobador();

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
        public async Task<IActionResult> Aprobar(SolicitudPlanificacionVacacionesViewModel modelo)
        {

            try
            {
                var response = await apiServicio.EditarAsync<Response>(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/SolicitudPlanificacionVacaciones/AprobarSolicitudPlanificacionVacaciones"
                    );

                if (response.IsSuccess)
                {

                    return this.Redireccionar(
                            "SolicitarPlanificacionVacaciones",
                            "Solicitudes",
                           new { id = modelo.DatosBasicosEmpleadoViewModel.IdEmpleado },
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


        public async Task CargarCombosAprobador()
        {

            //** Estados de aprobación vacaciones
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/SolicitudPlanificacionVacaciones/ListarEstadosAprobador");

            ViewData["IdListaEstado"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");
            
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



    }
}