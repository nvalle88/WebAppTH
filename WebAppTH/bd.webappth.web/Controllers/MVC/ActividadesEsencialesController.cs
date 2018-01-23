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
    public class ActividadesEsencialesController : Controller
    {

        private readonly IApiServicio apiServicio;


        public ActividadesEsencialesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public IActionResult Create()
        {
            return View();
        }


        public async Task<IActionResult> EliminarIncideOcupacionalActividadesEsenciales(int IdActividadesEsenciales, int idIndiceOcupacional)
        {

            try
            {

                var IndiceOcupacionalAreaConocimiento = new IndiceOcupacionalActividadesEsenciales
                {
                    IdActividadesEsenciales = IdActividadesEsenciales,
                    IdIndiceOcupacional = idIndiceOcupacional
                };
                var response = await apiServicio.EliminarAsync(IndiceOcupacionalAreaConocimiento, new Uri(WebApp.BaseAddress)
                                                               , "/api/ActividadesEsenciales/EliminarIndiceOcupacionalActividadesEsenciales");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1} {2} {3}", "Actividad esencial",
                                                                                        IndiceOcupacionalAreaConocimiento.IdActividadesEsenciales, "Índice Ocupacional", IndiceOcupacionalAreaConocimiento.IdIndiceOcupacional),
                        Message = "Registro deActividades esenciales",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Detalles", "IndicesOcupacionales", new { id = IndiceOcupacionalAreaConocimiento.IdIndiceOcupacional });
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
        public async Task<IActionResult> Create(ActividadesEsenciales actividadesEsenciales)
        {
            Response response = new Response();
            ActividadesEsenciales actividades=new ActividadesEsenciales();

            try
            {
                response = await apiServicio.InsertarAsync(actividadesEsenciales,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/ActividadesEsenciales/InsertarActividadesEsenciales");


                if (response.IsSuccess)
                {


                    LogEntryTranfer logEntryTranfer = new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una actividad esencial",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = "Actividades Esenciales",
                        ObjectPrevious = "NULL",
                        ObjectNext = JsonConvert.SerializeObject(response.Resultado),
                    };

                    var responseLog = await GuardarLogService.SaveLogEntry(logEntryTranfer);

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                return View(actividadesEsenciales);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Actividad Esencial",
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
                                                                  "/api/ActividadesEsenciales");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ActividadesEsenciales>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, ActividadesEsenciales actividadesEsenciales)
        {
            Response response = new Response();
            try
            {
                
                if (!string.IsNullOrEmpty(id))
                {

                    var objetoAnterior = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "/api/ActividadesEsenciales");

                    response = await apiServicio.EditarAsync(id, actividadesEsenciales, new Uri(WebApp.BaseAddress),
                                                                 "/api/ActividadesEsenciales");

                    if (response.IsSuccess)
                    {

                        LogEntryTranfer logEntryTranfer = new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            ExceptionTrace = null,
                            Message = "Se ha actualizado una actividad esencial",
                            UserName = "Usuario 1",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            EntityID = "Actividades Esenciales",
                            ObjectPrevious = JsonConvert.SerializeObject(objetoAnterior.Resultado),
                            ObjectNext = JsonConvert.SerializeObject(response.Resultado),
                        };

                        var responseLog = await GuardarLogService.SaveLogEntry(logEntryTranfer);

                        
                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(actividadesEsenciales);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una actividad esencial",
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

            var lista = new List<ActividadesEsenciales>();
            try
            {
                lista = await apiServicio.Listar<ActividadesEsenciales>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/ActividadesEsenciales/ListarActividadesEsenciales");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando actividades esenciales",
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
                                                               , "/api/ActividadesEsenciales");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de actividad esencial eliminado",
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
                    Message = "Eliminar Actividad Esencial",
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