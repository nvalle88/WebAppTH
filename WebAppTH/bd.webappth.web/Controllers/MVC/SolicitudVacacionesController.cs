using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using System.Security.Claims;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.webappth.entidades.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudVacacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitudVacacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {

            return View();
        }

        public async Task<ActionResult> ListadoEmpleadosSolicitudVacaciones()
        {
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var listadoEmpleados = await ListarEmpleadosPertenecientesaDependenciaconVacaciones(NombreUsuario);
            ViewData["IdPersona"] = new SelectList(await apiServicio.Listar<Persona>(new Uri(WebApp.BaseAddress), "/api/Empleados/ListarEmpleadosdeJefe"), "IdPersona", "Nombres");
            return View(listadoEmpleados);
        }


        public async Task<IActionResult> AprobacionSolicitudVacacion(int id)
        {
            try
            {
                if (id.ToString() != null)
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "/api/SolicitudVacaciones");


                    var a = JsonConvert.DeserializeObject<SolicitudVacaciones>(respuesta.Resultado.ToString());
                    var solicitudVacaciones = new SolicitudVacaciones
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
                        ViewData["PlanAnual"] = a.PlanAnual;
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

        public async Task<List<EmpleadoSolicitudViewModel>> ListarEmpleadosPertenecientesaDependenciaconVacaciones(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.Listar<EmpleadoSolicitudViewModel>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeJefeconSolucitudesVacaciones");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new List<EmpleadoSolicitudViewModel>();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudVacaciones solicitudVacaciones)
        {



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
                                                             "/api/SolicitudVacaciones/InsertarSolicitudVacaciones");
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
                //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "/api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                return View(solicitudVacaciones);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex,
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
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "/api/SolicitudVacaciones");


                    respuesta.Resultado = JsonConvert.DeserializeObject<SolicitudVacaciones>(respuesta.Resultado.ToString());
                    //ViewData["FechaDesde"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "/api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    if (respuesta.IsSuccess)
                    {
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


        public async Task<IActionResult> DetalleSolicitudVacaciones(int id)
        {
            var empleado = new Empleado()
            {
                IdEmpleado = id
            };

            var lista = new List<SolicitudVacaciones>();
            try
            {
                lista = await apiServicio.Listar<SolicitudVacaciones>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "/api/SolicitudVacaciones/ListarSolicitudesVacaciones");

                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
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
                if (!string.IsNullOrEmpty(id))
                {
                    solicitudVacaciones.FechaSolicitud = DateTime.Now;
                    response = await apiServicio.EditarAsync(id, solicitudVacaciones, new Uri(WebApp.BaseAddress),
                                                                 "/api/SolicitudVacaciones");

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

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "/api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    return View(solicitudVacaciones);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprobarSolicitudVacaciones(string id, SolicitudVacaciones solicitudVacaciones)
        {
            //var dias = (solicitudVacaciones.FechaHasta.Date - solicitudVacaciones.FechaDesde.Date).Days;
            Response response = new Response();
            try
            {
                solicitudVacaciones.FechaRespuesta = DateTime.Now;
                response = await apiServicio.EditarAsync(solicitudVacaciones.IdEmpleado.ToString(), solicitudVacaciones, new Uri(WebApp.BaseAddress),
                                                             "/api/SolicitudVacaciones");

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

                    if (solicitudVacaciones.Estado == 1)
                    {

                        var accionPersonal = new AccionPersonal
                        {
                            IdEmpleado = solicitudVacaciones.IdEmpleado,
                            IdTipoAccionPersonal = Convert.ToInt32(AccionPersonalEnum.vacaciones),
                            Fecha = solicitudVacaciones.FechaSolicitud,
                            Numero = null,
                            Solicitud = null,
                            Explicacion = null,
                            FechaRige = solicitudVacaciones.FechaDesde,
                            FechaRigeHasta = solicitudVacaciones.FechaHasta.Date,
                            NoDias = (solicitudVacaciones.FechaHasta.Date - solicitudVacaciones.FechaDesde.Date).Days,
                            Estado = 0
                        };
                        response = await apiServicio.InsertarAsync(accionPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/AccionesPersonal/InsertarAccionPersonal");
                        if (response.IsSuccess)
                        {

                            var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                            {
                                ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                                ExceptionTrace = null,
                                Message = "Se ha creado una acción de persona de vacaciones",
                                UserName = "Usuario 1",
                                LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                                LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                                EntityID = string.Format("{0} {1}", "Acción de personal:", accionPersonal.IdAccionPersonal),
                            });
                        }
                    }


                    return RedirectToAction("DetalleSolicitudVacaciones", new { id = solicitudVacaciones.IdEmpleado });
                }
                ViewData["Error"] = response.Message;
                //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "/api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                return View(solicitudVacaciones);


                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
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
                                                                    , "/api/SolicitudVacaciones/ListarSolicitudesVacaciones");

                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex,
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
                                                               , "/api/SolicitudVacaciones");
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
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Solicitud de Planificación Vacaciones",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }
    }
}