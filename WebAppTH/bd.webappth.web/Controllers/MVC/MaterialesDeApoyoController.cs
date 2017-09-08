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
    public class MaterialesDeApoyoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public MaterialesDeApoyoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
            ViewData["IdFormularioDevengacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormularioDevengacion>(new Uri(WebApp.BaseAddress), "api/FormulariosDevengacion/ListarFormulariosDevengacion"), "IdFormularioDevengacion", "ModoSocial");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialApoyo MaterialApoyo)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(MaterialApoyo,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/MaterialesDeApoyo/InsertarMaterialesDeApoyo");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un material apoyo",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Material Apoyo:", MaterialApoyo.IdMaterialApoyo),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;

                ViewData["IdFormularioDevengacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormularioDevengacion>(new Uri(WebApp.BaseAddress), "api/FormulariosDevengacion/ListarFormulariosDevengacion"), "IdFormularioDevengacion", "ModoSocial");

                return View(MaterialApoyo);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando material de apoyo",
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
                                                                  "/api/MaterialesDeApoyo");


                    respuesta.Resultado = JsonConvert.DeserializeObject<MaterialApoyo>(respuesta.Resultado.ToString());

                    ViewData["IdFormularioDevengacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormularioDevengacion>(new Uri(WebApp.BaseAddress), "api/FormulariosDevengacion/ListarFormulariosDevengacion"), "IdFormularioDevengacion", "ModoSocial");

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
        public async Task<IActionResult> Edit(string id, MaterialApoyo MaterialApoyo)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, MaterialApoyo, new Uri(WebApp.BaseAddress),
                                                                 "/api/MaterialesDeApoyo");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Material de apoyo", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un material de apoyo",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdFormularioDevengacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormularioDevengacion>(new Uri(WebApp.BaseAddress), "api/FormulariosDevengacion/ListarFormulariosDevengacion"), "IdFormularioDevengacion", "ModoSocial");

                    return View(MaterialApoyo);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una material de apoyo",
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

            var lista = new List<MaterialApoyo>();
            try
            {
                lista = await apiServicio.Listar<MaterialApoyo>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/MaterialesDeApoyo/ListarMaterialesDeApoyo");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando un material de apoyo",
                    ExceptionTrace = ex,
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
                                                               , "/api/MaterialesDeApoyo");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro un material de apoyo",
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
                    Message = "Eliminar un material de apoyo",
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