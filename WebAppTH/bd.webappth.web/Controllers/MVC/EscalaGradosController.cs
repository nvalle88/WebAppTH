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
    public class EscalaGradosController : Controller
    {
        private readonly IApiServicio apiServicio;


        public EscalaGradosController(IApiServicio apiServicio)
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
            InicializarMensaje(mensaje);
            ViewData["IdGrupoOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GrupoOcupacional>(new Uri(WebApp.BaseAddress), "api/GruposOcupacionales/ListarGruposOcupacionales"), "IdGrupoOcupacional", "TipoEscala");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EscalaGrados escalaGrados)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                ViewData["IdGrupoOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GrupoOcupacional>(new Uri(WebApp.BaseAddress), "api/GruposOcupacionales/ListarGruposOcupacionales"), "IdGrupoOcupacional", "TipoEscala");
                return View(escalaGrados);

            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(escalaGrados,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/EscalasGrados/InsertarEscalaGrados");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una escala grado",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Escala Grados:", escalaGrados.IdEscalaGrados),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdGrupoOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GrupoOcupacional>(new Uri(WebApp.BaseAddress), "api/GruposOcupacionales/ListarGruposOcupacionales"), "IdGrupoOcupacional", "TipoEscala");
                return View(escalaGrados);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Escala Grados",
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
                                                                  "api/EscalasGrados");


                    respuesta.Resultado = JsonConvert.DeserializeObject<EscalaGrados>(respuesta.Resultado.ToString());
                    ViewData["IdGrupoOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GrupoOcupacional>(new Uri(WebApp.BaseAddress), "api/GruposOcupacionales/ListarGruposOcupacionales"), "IdGrupoOcupacional", "TipoEscala");
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
        public async Task<IActionResult> Edit(string id, EscalaGrados escalaGrados)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, escalaGrados, new Uri(WebApp.BaseAddress),
                                                                 "api/EscalasGrados");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Escala Grados", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una escala grado",
                            UserName = "Usuario 1"
                        });
                        InicializarMensaje(null);
                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    ViewData["IdGrupoOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<GrupoOcupacional>(new Uri(WebApp.BaseAddress), "api/GruposOcupacionales/ListarGruposOcupacionales"), "IdGrupoOcupacional", "TipoEscala");
                    return View(escalaGrados);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una escala grado",
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

            var lista = new List<EscalaGrados>();
            try
            {
                lista = await apiServicio.Listar<EscalaGrados>(new Uri(WebApp.BaseAddress)
                                                                    , "api/EscalasGrados/ListarEscalasGrados");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando escalas grados",
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
                                                               , "api/EscalasGrados");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Escala Grados", id),
                        Message = "Registro de escala grado eliminado",
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
                    Message = "Eliminar Escala Grados",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<JsonResult> ListarEscalasGradosPorGrupoOcupacional(int idgrupoocupacional)
        {
            try
            {
                var grupoocupacional = new GrupoOcupacional
                {
                    IdGrupoOcupacional = idgrupoocupacional,
                };
                var listarescalasgrados = await apiServicio.Listar<EscalaGrados>(grupoocupacional, new Uri(WebApp.BaseAddress), "api/EscalasGrados/ListarEscalasGradosPorGrupoOcupacional");
                return Json(listarescalasgrados);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }
    }
}