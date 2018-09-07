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

namespace bd.webappth.web.Controllers.MVC
{
    public class AccionPersonalController : Controller
    {
        private readonly IApiServicio apiServicio;

        public AccionPersonalController(IApiServicio apiServicio)
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
        public IActionResult Create(string mensaje)
        {
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccionPersonal accionPersonal)
        {
            
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(accionPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/AccionesPersonal/InsertarAccionPersonal");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un acción de personal",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Acción de Personal:", accionPersonal.IdAccionPersonal),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                return View(accionPersonal);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Acción de Personal",
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
                                                                  "api/AccionesPersonal");


                    var a = JsonConvert.DeserializeObject<AccionPersonal>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        var empleadoEnviar = new Empleado
                        {
                            IdEmpleado = a.IdEmpleado,
                        };
                        var empleado = await apiServicio.ObtenerElementoAsync1<EmpleadoSolicitudViewModel>(empleadoEnviar, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDatosEmpleadoSeleccionado");
                        var respuestaAccionPersonal = await apiServicio.SeleccionarAsync<Response>(a.IdTipoAccionPersonal.ToString(), new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales");
                        var tipoaccionpersonal = JsonConvert.DeserializeObject<TipoAccionPersonal>(respuestaAccionPersonal.Resultado.ToString());
                        ViewData["NombresApellidos"] = empleado.NombreApellido;
                        ViewData["Identificacion"] = empleado.Identificacion;
                        ViewData["TipoAccionPersonal"] = tipoaccionpersonal.Nombre;
                        ViewData["Fecha"] = a.Fecha;
                        ViewData["FechaRige"] = a.FechaRige;
                        ViewData["FechaRigeHasta"] = a.FechaRigeHasta;
                        ViewData["NoDias"] = a.NoDias;
                        InicializarMensaje(null);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AccionPersonal accionPersonal)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, accionPersonal, new Uri(WebApp.BaseAddress),
                                                                 "api/AccionesPersonal");

                    if (response.IsSuccess)
                    {

                        return RedirectToAction("ListarEmpleadosconAccionPersonalPendiente");
                    }
                    ViewData["Error"] = response.Message;
                    return View(accionPersonal);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<AccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<AccionPersonal>(new Uri(WebApp.BaseAddress)
                                                                    , "api/AccionesPersonal/ListarAccionesPersonal");
                return View(lista);
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }

        public async Task<IActionResult> ListarEmpleadosconAccionPersonalPendiente()
        {

            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleadoconAccionPersonalPendiente");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando empleados con acción personal pendiente",
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
                                                               , "api/AccionesPersonal");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de acción de personal eliminado",
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
                    Message = "Eliminar Acción de Personal",
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