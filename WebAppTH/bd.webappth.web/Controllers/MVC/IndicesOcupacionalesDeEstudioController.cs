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
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class IndicesOcupacionalesDeEstudioController : Controller
    {
        private readonly IApiServicio apiServicio;


        public IndicesOcupacionalesDeEstudioController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create(int id)
        {
            ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");


            var indiceOcupacionalEstudio = new IndiceOcupacionalEstudio
            {
                IdIndiceOcupacional = id,
            };

            return PartialView("..//Indicadores//Createejemplo",indiceOcupacionalEstudio);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndiceOcupacionalEstudio IndiceOcupacionalEstudio)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(IndiceOcupacionalEstudio,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/IndicesOcupacionalesDeEstudio/InsertarIndiceOcupacionalEstudio");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un índice ocupacional de estudio",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Índice Ocupacional de Estudio:", IndiceOcupacionalEstudio.IdIndiceOcupacionalEstudio),
                    });
                    ViewData["Error"] = response.Message;
                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

                return PartialView("..//Indicadores//Createejemplo", IndiceOcupacionalEstudio);
              //  return View(IndiceOcupacionalEstudio);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando índice ocupacional de estudio",
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
                                                                  "/api/IndicesOcupacionalesDeEstudio");


                    respuesta.Resultado = JsonConvert.DeserializeObject<IndiceOcupacionalEstudio>(respuesta.Resultado.ToString());

                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

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
        public async Task<IActionResult> Edit(string id, IndiceOcupacionalEstudio IndiceOcupacionalEstudio)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, IndiceOcupacionalEstudio, new Uri(WebApp.BaseAddress),
                                                                 "/api/IndicesOcupacionalesDeEstudio");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Índice Ocupacional de Estudio", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un índice ocupacional de estudio",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

                    return View(IndiceOcupacionalEstudio);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un índice ocupacional de estudio",
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

            return View();
        }


        public ActionResult PartialViewModalIndicesOcupacionalesDeEstudio()
        {
           
            return PartialView();
        }



        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "/api/IndicesOcupacionalesDeEstudio");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de índice ocupacional de estudio",
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
                    Message = "Eliminar un índice ocupacional de estudio",
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