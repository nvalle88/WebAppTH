using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using Newtonsoft.Json;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudHorasExtraController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitudHorasExtraController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        

        public async Task<ActionResult> ListadoEmpleadosSolicitudHorasExtra()
        {
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var listadoEmpleados = await ListarEmpleadosPertenecientesaDependenciaconHorasExtra(NombreUsuario);
            ViewData["IdPersona"] = new SelectList(await apiServicio.Listar<Persona>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeJefe"), "IdPersona", "Nombres");
            return View(listadoEmpleados);
        }


        public async Task<IActionResult> AprobacionSolicitudHorasExtra(int id)
        {
            try
            {
                if (id.ToString() != null)
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "api/SolicitudHorasExtras");


                    var a = JsonConvert.DeserializeObject<SolicitudHorasExtras>(respuesta.Resultado.ToString());
                    var solicitudhorasextra = new SolicitudHorasExtras
                    {
                        IdEmpleado = a.IdEmpleado,
                    };
                    //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.ObtenerElementoAsync1<BrigadaSSO>(solicitudPlanificacionVacaciones, new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    if (respuesta.IsSuccess)
                    {
                        ViewData["Fecha"] = a.Fecha;
                        ViewData["CantidadHoras"] = a.CantidadHoras;
                        ViewData["FechaSolicitud"] = a.FechaSolicitud;
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

        public async Task<List<EmpleadoSolicitudViewModel>> ListarEmpleadosPertenecientesaDependenciaconHorasExtra(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.Listar<EmpleadoSolicitudViewModel>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeJefeconHorasExtra");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new List<EmpleadoSolicitudViewModel>();
            }

        }

        public async Task<IActionResult> DetalleSolicitudHorasExtra(int id)
        {
            var empleado = new Empleado()
            {
                IdEmpleado = id
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprobarSolicitudHorasExtra(string id, SolicitudHorasExtras solicitudhorasextras)
        {
            solicitudhorasextras.FechaAprobado = DateTime.Now;
            Response response = new Response();
            try
            {

                response = await apiServicio.EditarAsync(solicitudhorasextras.IdEmpleado.ToString(), solicitudhorasextras, new Uri(WebApp.BaseAddress),
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

                    return RedirectToAction("DetalleSolicitudHorasExtra", new { id = solicitudhorasextras.IdEmpleado });
                }
                ViewData["Error"] = response.Message;
                //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                return View(solicitudhorasextras);


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


    }
}