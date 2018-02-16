using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using System.Security.Claims;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitarHorasExtrasController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitarHorasExtrasController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {

            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudHorasExtras solicitudhorasextra)
        {



            Response response = new Response();
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var empleadoJson = ObtenerEmpleadoLogueado(NombreUsuario);
                //var empleado = JsonConvert.DeserializeObject<Empleado>(empleadoJson.ToString());
                solicitudhorasextra.IdEmpleado = empleadoJson.Result.IdEmpleado;
                solicitudhorasextra.FechaSolicitud = DateTime.Now;
                solicitudhorasextra.Estado = 0;
                var HorasExtra = new SolicitudHorasExtras
                {
                    Fecha = solicitudhorasextra.Fecha,
                };
                var resultado = await apiServicio.ObtenerElementoAsync1<Response>(HorasExtra, new Uri(WebApp.BaseAddress), "api/SolicitudHorasExtras/EsExtraordinaria");

                solicitudhorasextra.EsExtraordinaria = false;
                if (resultado.IsSuccess)
                {
                    solicitudhorasextra.EsExtraordinaria = true;
                }


                response = await apiServicio.InsertarAsync(solicitudhorasextra,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/SolicitudHorasExtras/InsertarSolicitudHorasExtras");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una Solicitud de Horas Extra",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Solicitud Horas Extras:", solicitudhorasextra.IdSolicitudHorasExtras),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                return View(solicitudhorasextra);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Solicitud Horas Extras",
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
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/SolicitudHorasExtras");


                    respuesta.Resultado = JsonConvert.DeserializeObject<SolicitudHorasExtras>(respuesta.Resultado.ToString());
                    //ViewData["FechaDesde"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SolicitudHorasExtras solicitudhorasextra)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(solicitudhorasextra.IdEmpleado.ToString()))
                {
                    solicitudhorasextra.FechaSolicitud = DateTime.Now;
                    var HorasExtra = new SolicitudHorasExtras
                    {
                        Fecha = solicitudhorasextra.Fecha,
                    };
                    var resultado = await apiServicio.ObtenerElementoAsync1<Response>(HorasExtra, new Uri(WebApp.BaseAddress), "api/SolicitudHorasExtras/EsExtraordinaria");

                    solicitudhorasextra.EsExtraordinaria = false;
                    if (resultado.IsSuccess)
                    {
                        solicitudhorasextra.EsExtraordinaria = true;
                    }

                    response = await apiServicio.EditarAsync(solicitudhorasextra.IdEmpleado.ToString(), solicitudhorasextra, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudHorasExtras");

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
                    ViewData["Error"] = response.Message;
                    //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    return View(solicitudhorasextra);

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

        public async Task<IActionResult> Index()
        {

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var empleadoJson = await ObtenerEmpleadoLogueado(NombreUsuario);
            var idEmpleado = empleadoJson.IdEmpleado;

            var empleado = new Empleado()
            {
                IdEmpleado = idEmpleado
            };

            var lista = new List<SolicitudHorasExtras>();
            try
            {
                lista = await apiServicio.Listar<SolicitudHorasExtras>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/SolicitudHorasExtras/ListarSolicitudesHorasExtra");



                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Solicitud Horas Extras",
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
                                                               , "api/SolicitudHorasExtras");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de Solicitud Horas Extras eliminado",
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
                    Message = "Eliminar Solicitud de Horas Extra",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }
    }
}