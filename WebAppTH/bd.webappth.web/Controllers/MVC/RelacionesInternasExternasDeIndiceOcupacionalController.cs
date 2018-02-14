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
    public class RelacionesInternasExternasDeIndiceOcupacionalController : Controller
    {
        private readonly IApiServicio apiServicio;


        public RelacionesInternasExternasDeIndiceOcupacionalController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
            ViewData["IdRelacionesInternasExternas"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionesInternasExternas>(new Uri(WebApp.BaseAddress), "api/RelacionesInternasExternas/ListarRelacionesInternasExternas"), "IdRelacionesInternasExternas", "Descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RelacionesInternasExternasIndiceOcupacional RelacionesInternasExternasIndiceOcupacional)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(RelacionesInternasExternasIndiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/RelacionesInternasExternasDeIndiceOcupacional/InsertarRelacionesInternasExternasIndiceOcupacional");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una relación interna externa de índice ocupacional",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", " Relaciones Internas Externas De Índice Ocupacional:", RelacionesInternasExternasIndiceOcupacional.IdRelacionesInternasExternasIndiceOcupacional),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdRelacionesInternasExternas"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionesInternasExternas>(new Uri(WebApp.BaseAddress), "api/RelacionesInternasExternas/ListarRelacionesInternasExternas"), "IdRelacionesInternasExternas", "Descripcion");

                return View(RelacionesInternasExternasIndiceOcupacional);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando una relación interna externa de índice ocupacional",
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
                                                                  "api/RelacionesInternasExternasDeIndiceOcupacional");


                    respuesta.Resultado = JsonConvert.DeserializeObject<RelacionesInternasExternasIndiceOcupacional>(respuesta.Resultado.ToString());

                    ViewData["IdRelacionesInternasExternas"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionesInternasExternas>(new Uri(WebApp.BaseAddress), "api/RelacionesInternasExternas/ListarRelacionesInternasExternas"), "IdRelacionesInternasExternas", "Descripcion");

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
        public async Task<IActionResult> Edit(string id, RelacionesInternasExternasIndiceOcupacional RelacionesInternasExternasIndiceOcupacional)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, RelacionesInternasExternasIndiceOcupacional, new Uri(WebApp.BaseAddress),
                                                                 "api/RelacionesInternasExternasDeIndiceOcupacional");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Relaciones Internas Externas De Índice Ocupacional", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una relación interna externa de índice ocupacional",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdRelacionesInternasExternas"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RelacionesInternasExternas>(new Uri(WebApp.BaseAddress), "api/RelacionesInternasExternas/ListarRelacionesInternasExternas"), "IdRelacionesInternasExternas", "Descripcion");

                    return View(RelacionesInternasExternasIndiceOcupacional);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una relación interna externa de índice ocupacional",
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

            var lista = new List<RelacionesInternasExternasIndiceOcupacional>();
            try
            {
                lista = await apiServicio.Listar<RelacionesInternasExternasIndiceOcupacional>(new Uri(WebApp.BaseAddress)
                                                                    , "api/RelacionesInternasExternasDeIndiceOcupacional/ListarRelacionesInternasExternasDeIndiceOcupacional");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una relación interna externa de índice ocupacional",
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
                                                               , "api/RelacionesInternasExternasDeIndiceOcupacional");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de relación interna externa de índice ocupacional",
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
                    Message = "Eliminar una relación interna externa de índice ocupacional",
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