using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudPlanificacionVacacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitudPlanificacionVacacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<ActionResult> ListadoEmpleadosPlanificacionVacaciones()
        {
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var listadoEmpleados = await ListarEmpleadosPertenecientesaDependencia(NombreUsuario);
            ViewData["IdPersona"] = new SelectList(await apiServicio.Listar<Persona>(new Uri(WebApp.BaseAddress), "/api/Empleados/ListarEmpleadosdeJefe"), "IdPersona", "Nombres");
            return View(listadoEmpleados);
        }

        public async Task<IActionResult> AprobacionPlanificacionVacaciones(int id)
        {
            try
            {
                if (id.ToString() != null)
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "/api/SolicitudPlanificacionVacaciones");

                  
                    var a = JsonConvert.DeserializeObject<SolicitudPlanificacionVacaciones>(respuesta.Resultado.ToString());
                    var solicitudPlanificacionVacaciones = new SolicitudPlanificacionVacaciones
                    {
                        IdEmpleado = a.IdEmpleado,
                    };
                    //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.ObtenerElementoAsync1<BrigadaSSO>(solicitudPlanificacionVacaciones, new Uri(WebApp.BaseAddress), "/api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    if (respuesta.IsSuccess)
                    {
                        var empleadoEnviar = new Empleado
                        {
                            IdEmpleado = a.IdEmpleado,
                        };
                        var empleado = await apiServicio.ObtenerElementoAsync1<EmpleadoSolicitudViewModel>(empleadoEnviar, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDatosEmpleadoSeleccionado");
                        ViewData["FechaDesde"] = a.FechaDesde;
                        ViewData["FechaHasta"] = a.FechaHasta;
                        ViewData["FechaSolicitud"] = a.FechaSolicitud;
                        ViewData["NombresApellidos"] = empleado.NombreApellido;
                        ViewData["Identificacion"] = empleado.Identificacion;
                        return View(a);
                    }

                }

                return BadRequest();
            }
            catch (Exception)
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

        public async Task<List<EmpleadoSolicitudViewModel>> ListarEmpleadosPertenecientesaDependencia(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.Listar<EmpleadoSolicitudViewModel>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeJefe");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new List<EmpleadoSolicitudViewModel>();
            }

        }

        public async Task<IActionResult> DetallePlanificacionVacaciones(int id)
        {
            var empleado = new Empleado()
            {
                IdEmpleado = id
            };

            var lista = new List<SolicitudPlanificacionVacaciones>();
            try
            {
                lista = await apiServicio.Listar<SolicitudPlanificacionVacaciones>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "/api/SolicitudPlanificacionVacaciones/ListarSolicitudesPlanificacionesVacaciones");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprobarVacaciones(string id, SolicitudPlanificacionVacaciones solicitudPlanificacionVacaciones)
        {
            Response response = new Response();
            try
            {
               
                    response = await apiServicio.EditarAsync(solicitudPlanificacionVacaciones.IdEmpleado.ToString(), solicitudPlanificacionVacaciones, new Uri(WebApp.BaseAddress),
                                                                 "/api/SolicitudPlanificacionVacaciones");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Solicitud Planificación Vacaciones", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un Solicitud Planificación Vacaciones",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("DetallePlanificacionVacaciones", new { id = solicitudPlanificacionVacaciones.IdEmpleado });
                    }
                    ViewData["Error"] = response.Message;
                    //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "/api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    return View(solicitudPlanificacionVacaciones);

                
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

       
    }
}