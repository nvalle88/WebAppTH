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
    public class IngresoEgresoRMUController : Controller
    {
        private readonly IApiServicio apiServicio;


        public IngresoEgresoRMUController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
            ViewData["IdFormulaRMU"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormulasRMU>(new Uri(WebApp.BaseAddress), "api/FormulasRMU/ListarFormulasRMU"), "IdFormulaRMU", "Formula");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IngresoEgresoRMU IngresoEgresoRMU)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(IngresoEgresoRMU,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/IngresoEgresoRMU/InsertarIngresoEgresoRMU");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un ingreso egreso RMU",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Ingreso Egreso RMU:", IngresoEgresoRMU.IdIngresoEgresoRMU),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdFormulaRMU"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormulasRMU>(new Uri(WebApp.BaseAddress), "api/FormulasRMU/ListarFormulasRMU"), "IdFormulaRMU", "Formula");

                return View(IngresoEgresoRMU);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Ingreso Egreso RMU",
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
                                                                  "api/IngresoEgresoRMU");


                    respuesta.Resultado = JsonConvert.DeserializeObject<IngresoEgresoRMU>(respuesta.Resultado.ToString());

                    ViewData["IdFormulaRMU"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormulasRMU>(new Uri(WebApp.BaseAddress), "api/FormulasRMU/ListarFormulasRMU"), "IdFormulaRMU", "Formula");

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
        public async Task<IActionResult> Edit(string id, IngresoEgresoRMU IngresoEgresoRMU)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, IngresoEgresoRMU, new Uri(WebApp.BaseAddress),
                                                                 "api/IngresoEgresoRMU");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Ingreso Egreso RMU", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un ingreso egreso RMU",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdFormulaRMU"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FormulasRMU>(new Uri(WebApp.BaseAddress), "api/FormulasRMU/ListarFormulasRMU"), "IdFormulaRMU", "Formula");

                    return View(IngresoEgresoRMU);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una ingreso egreso RMU",
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

            var lista = new List<IngresoEgresoRMU>();
            try
            {
                lista = await apiServicio.Listar<IngresoEgresoRMU>(new Uri(WebApp.BaseAddress)
                                                                    , "api/IngresoEgresoRMU/ListarIngresoEgresoRMU");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando un ingreso egreso RMU",
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
                                                               , "api/IngresoEgresoRMU");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de ingreso egreso RMU",
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
                    Message = "Eliminar un ingreso egreso RMU",
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