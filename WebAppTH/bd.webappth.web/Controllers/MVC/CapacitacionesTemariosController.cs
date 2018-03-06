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

namespace bd.webappth.web.Controllers.MVC
{
    public class CapacitacionesTemariosController : Controller
    {
        private readonly IApiServicio apiServicio;

        public CapacitacionesTemariosController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create(string mensaje)
        {
            
            ViewData["IdCapacitacionAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<CapacitacionAreaConocimiento>(new Uri(WebApp.BaseAddress), "api/CapacitacionesAreasConocimientos/ListarCapacionesAreasConocimientos"), "IdCapacitacionAreaConocimiento", "Descripcion");
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CapacitacionTemario capacitacionTemario)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(capacitacionTemario);
            }

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(capacitacionTemario,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/CapacitacionesTemarios/InsertarCapacitacionTemario");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un temario de capacitación ",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Temario de Capacitación :", capacitacionTemario.IdCapacitacionTemario),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdCapacitacionAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<CapacitacionAreaConocimiento>(new Uri(WebApp.BaseAddress), "api/CapacitacionesAreasConocimientos/ListarCapacionesAreasConocimientos"), "IdCapacitacionAreaConocimiento", "Descripcion");
                return View(capacitacionTemario);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Temario de Capacitación ",
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
                                                                  "api/CapacitacionesTemarios");


                    respuesta.Resultado = JsonConvert.DeserializeObject<CapacitacionTemario>(respuesta.Resultado.ToString());
                    ViewData["IdCapacitacionAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<CapacitacionAreaConocimiento>(new Uri(WebApp.BaseAddress), "api/CapacitacionesAreasConocimientos/ListarCapacionesAreasConocimientos"), "IdCapacitacionAreaConocimiento", "Descripcion");
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
        public async Task<IActionResult> Edit(string id, CapacitacionTemario capacitacionTemario)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, capacitacionTemario, new Uri(WebApp.BaseAddress),
                                                                 "api/CapacitacionesTemarios");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Temario de Capacitación ", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un temario de capacitación ",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    ViewData["IdCapacitacionAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<CapacitacionAreaConocimiento>(new Uri(WebApp.BaseAddress), "api/CapacitacionesAreasConocimientos/ListarCapacionesAreasConocimientos"), "IdCapacitacionAreaConocimiento", "Descripcion");
                    return View(capacitacionTemario);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un temario de capacitación ",
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

            var lista = new List<CapacitacionTemario>();
            try
            {
                lista = await apiServicio.Listar<CapacitacionTemario>(new Uri(WebApp.BaseAddress)
                                                                    , "api/CapacitacionesTemarios/ListarCapacitacionesTemarios");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Temario de Capacitación ",
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
                                                               , "api/CapacitacionesTemarios");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Temario de Capacitación", id),
                        Message = "Registro de Temario de Capacitación  eliminado",
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
                    Message = "Eliminar Temario de Capacitación",
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