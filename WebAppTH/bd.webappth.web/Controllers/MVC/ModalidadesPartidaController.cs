using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.Negocio;


namespace bd.webappth.web.Controllers.MVC
{
    public class ModalidadesPartidaController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ModalidadesPartidaController(IApiServicio apiServicio)
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
            ViewData["IdRelacionLaboral"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionLaboral>(new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaborales"), "IdRelacionLaboral", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModalidadPartida ModalidadPartida)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ModalidadPartida);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ModalidadPartida,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ModalidadesPartida/InsertarModalidadPartida");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una modalidad partida",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Modalidad Partida:", ModalidadPartida.IdModalidadPartida),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdRelacionLaboral"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionLaboral>(new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaborales"), "IdRelacionLaboral", "Nombre");

                return View(ModalidadPartida);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando modalidad partida",
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
                                                                  "api/ModalidadesPartida");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ModalidadPartida>(respuesta.Resultado.ToString());

                    ViewData["IdRelacionLaboral"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionLaboral>(new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaborales"), "IdRelacionLaboral", "Nombre");

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
        public async Task<IActionResult> Edit(string id, ModalidadPartida ModalidadPartida)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, ModalidadPartida, new Uri(WebApp.BaseAddress),
                                                                 "api/ModalidadesPartida");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Modalidad Partida", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una modalidad partida",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdRelacionLaboral"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionLaboral>(new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaborales"), "IdRelacionLaboral", "Nombre");

                    return View(ModalidadPartida);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una modalidad partida",
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

            var lista = new List<ModalidadPartida>();
            try
            {
                lista = await apiServicio.Listar<ModalidadPartida>(new Uri(WebApp.BaseAddress)
                                                                    , "api/ModalidadesPartida/ListarModalidadesPartida");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una modalidad partida",
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
                                                               , "api/ModalidadesPartida");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de modalidad partida",
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
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar una modalidad partida",
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