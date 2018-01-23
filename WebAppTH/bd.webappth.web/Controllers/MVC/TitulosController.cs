using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class TitulosController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TitulosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
            ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
            ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "/api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Titulo titulo)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(titulo,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/Titulos/InsertarTitulo");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un título",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Titulos:", titulo.IdTitulo),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "/api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");
                return View(titulo);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Area de Conocimiento",
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
                                                                  "/api/Titulos");


                    respuesta.Resultado = JsonConvert.DeserializeObject<Titulo>(respuesta.Resultado.ToString());
                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                    ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "/api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");
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
        public async Task<IActionResult> Edit(string id, Titulo titulo)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, titulo, new Uri(WebApp.BaseAddress),
                                                                 "/api/Titulos");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Area de Conocimiento", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un título",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                    ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "/api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");
                    return View(titulo);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un título",
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

            var lista = new List<Titulo>();
            try
            {
                lista = await apiServicio.Listar<Titulo>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/Titulos/ListarTitulos");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando áreas de conocimientos",
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
                                                               , "/api/Titulos");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Area de Conocimiento", id),
                        Message = "Registro de título eliminado",
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
                    Message = "Eliminar Area de Conocimiento",
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