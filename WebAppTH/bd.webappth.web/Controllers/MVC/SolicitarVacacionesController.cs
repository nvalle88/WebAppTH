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

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitarVacacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitarVacacionesController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create()
        {
            InicializarMensaje(null);
            return View();
        }

        public async Task<IActionResult> Index()
        {

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var empleadoJson = ObtenerEmpleadoLogueado(NombreUsuario);
            var idEmpleado = empleadoJson.Result.IdEmpleado;

            var empleado = new Empleado()
            {
                
                IdEmpleado = idEmpleado
            };

            var lista = new List<SolicitudVacaciones>();
            try
            {
                lista = await apiServicio.Listar<SolicitudVacaciones>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/SolicitudVacaciones/ListarSolicitudesVacaciones");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
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
                //return BadRequest();
                return RedirectToAction("Index", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Solicitud de Planificación Vacaciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudVacaciones solicitudVacaciones)
        {

            if (ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(null);
            }

            Response response = new Response();
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var empleadoJson = ObtenerEmpleadoLogueado(NombreUsuario);
                //var empleado = JsonConvert.DeserializeObject<Empleado>(empleadoJson.ToString());
                solicitudVacaciones.IdEmpleado = empleadoJson.Result.IdEmpleado;
                solicitudVacaciones.FechaSolicitud = DateTime.Now;
                solicitudVacaciones.FechaRespuesta = null;
                response = await apiServicio.InsertarAsync(solicitudVacaciones,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/SolicitudVacaciones/InsertarSolicitudVacaciones");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un Solicitud Planificación Vacaciones",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Solicitud Planificación Vacaciones:", solicitudVacaciones.IdSolicitudVacaciones),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                return View(solicitudVacaciones);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<entidades.Utils.Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/SolicitudVacaciones");


                    respuesta.Resultado = JsonConvert.DeserializeObject<SolicitudVacaciones>(respuesta.Resultado.ToString());
                    //ViewData["FechaDesde"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
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
        public async Task<IActionResult> Edit(string id, SolicitudVacaciones solicitudVacaciones)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(solicitudVacaciones.IdEmpleado.ToString()))
                {
                    if (solicitudVacaciones.Estado ==0)
                    {
                        solicitudVacaciones.FechaSolicitud = DateTime.Now;;
                        var resultado = await apiServicio.ObtenerElementoAsync1<Response>(solicitudVacaciones, new Uri(WebApp.BaseAddress), "api/SolicitudVacaciones/SolicitudVacaciones");
                    
                        response = await apiServicio.EditarAsync(solicitudVacaciones.IdEmpleado.ToString(), solicitudVacaciones, new Uri(WebApp.BaseAddress),
                                                                     "api/SolicitudVacaciones");

                        if (response.IsSuccess)
                        {
                            await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                            {
                                ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                                EntityID = string.Format("{0} : {1}", "Solicitud Horas Extras", id),
                                LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                                LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                                Message = "Se ha actualizado una Solicitud de Horas Extra",
                                UserName = "Usuario 1"
                            });

                            return RedirectToAction("Index");
                        }
                       // ViewData["Error"] = "Se ha actualizado una Solicitud de Horas Extra";
                    }
                    ViewData["Error"] = response.Message;
                    //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    return View(solicitudVacaciones);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una Solicitud de Horas Extra",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

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