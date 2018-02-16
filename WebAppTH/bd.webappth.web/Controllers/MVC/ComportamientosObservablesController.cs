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
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class ComportamientosObservablesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ComportamientosObservablesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
            ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
            ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
            return View();
        }

        public async Task<IActionResult> EliminarIndiceOcupacionalComportamiemtoObservable(int idComportamientoObservable, int idIndiceOcupacional)
        {
            try
            {

                var indiceOcupacionalComportamientoObservable = new IndiceOcupacionalComportamientoObservable
                {
                    IdComportamientoObservable = idComportamientoObservable,
                    IdIndiceOcupacional = idIndiceOcupacional
                };
                var response = await apiServicio.EliminarAsync(indiceOcupacionalComportamientoObservable, new Uri(WebApp.BaseAddress)
                                                               , "api/ComportamientosObservables/EliminarIndiceOcupacionalComportamiemtoObservable");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1} {2} {3}", "Compostamiento Observable ",
                                                                                        indiceOcupacionalComportamientoObservable.IdComportamientoObservable, "Índice Ocupacional", indiceOcupacionalComportamientoObservable.IdIndiceOcupacional),
                        Message = Mensaje.Satisfactorio,
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Detalles", "IndicesOcupacionales", new { id = indiceOcupacionalComportamientoObservable.IdIndiceOcupacional });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComportamientoObservable comportamientoObservable)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(comportamientoObservable,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ComportamientosObservables/InsertarComportamientoObservable");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un Comportamiento Observable",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Comportaminto Observable:", comportamientoObservable.IdComportamientoObservable),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
                ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
                return View(comportamientoObservable);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Comportaminto Observable",
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
                                                                  "api/ComportamientosObservables");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ComportamientoObservable>(respuesta.Resultado.ToString());
                    ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
                    ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
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
        public async Task<IActionResult> Edit(string id, ComportamientoObservable comportamientoObservable)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, comportamientoObservable, new Uri(WebApp.BaseAddress),
                                                                 "api/ComportamientosObservables");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Comportaminto Observable", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un Comportamiento Observable",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
                    ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
                    return View(comportamientoObservable);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un Comportamiento Observable",
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

            var lista = new List<ComportamientoObservable>();
            try
            {
                lista = await apiServicio.Listar<ComportamientoObservable>(new Uri(WebApp.BaseAddress)
                                                                    , "api/ComportamientosObservables/ListarComportamientosObservables");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Comportaminto Observable",
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
                                                               , "api/ComportamientosObservables");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de Comportaminto Observable eliminado",
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
                    Message = "Eliminar Comportamiento Observable",
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