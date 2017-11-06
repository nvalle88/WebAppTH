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
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class ActividadesGestionCambioController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ActividadesGestionCambioController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create(int IdPlanGestionCambio)
        {
            var actividadesGestionCambio = new ActividadesGestionCambio
            {
                IdPlanGestionCambio = IdPlanGestionCambio,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now,
            };
            return View(actividadesGestionCambio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActividadesGestionCambio ActividadesGestionCambio)
        {
            Response response = new Response();
            try
            {
                
              
                response = await apiServicio.InsertarAsync(ActividadesGestionCambio,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/ActividadesGestionCambio/InsertarActividadesGestionCambio");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una actividad gestión de cambio",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Actividad gestión de cambio:", ActividadesGestionCambio.IdActividadesGestionCambio),
                    });
                    
                    return RedirectToAction("Index", new { IdPlanGestionCambio = ActividadesGestionCambio.IdPlanGestionCambio });
                }

                ViewData["Error"] = response.Message;
            
                return View(ActividadesGestionCambio);



            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando una actividad gestión de cambio",
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
                                                                  "/api/ActividadesGestionCambio");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ActividadesGestionCambio>(respuesta.Resultado.ToString());
                    
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
        public async Task<IActionResult> Edit(string id, ActividadesGestionCambio ActividadesGestionCambio)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, ActividadesGestionCambio, new Uri(WebApp.BaseAddress),
                                                                 "/api/ActividadesGestionCambio");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Actividad de gestión de cambio", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una actividad de gestión de cambio",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index", new { IdPlanGestionCambio = ActividadesGestionCambio.IdPlanGestionCambio });
                    }
                    ViewData["Error"] = response.Message;
                    
                    return View(ActividadesGestionCambio);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una actividad gestión de cambio",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(int IdPlanGestionCambio, int IdActividadesGestionCambio)
        {

            try
            {
                    if (IdPlanGestionCambio != 0 && IdActividadesGestionCambio == 0)
                    {

                        var actividadesGestionCambio = new ActividadesGestionCambioViewModel
                        {
                            IdPlanGestionCambio = Convert.ToInt32(IdPlanGestionCambio),

                        };

                        var viewModelActividadesGestionCambio = new ActividadesGestionCambioViewModel
                        {
                            IdPlanGestionCambio = Convert.ToInt32(IdPlanGestionCambio),
                            ListaActividadesGestionCambio = await apiServicio.Listar<ActividadesGestionCambioIndex>(actividadesGestionCambio, new Uri(WebApp.BaseAddress), "/api/ActividadesGestionCambio/ListarActividadesGestionCambioconIdPlan")
                        };

                        return View(viewModelActividadesGestionCambio);
                    }

                    if (IdPlanGestionCambio == 0 && IdActividadesGestionCambio != 0)
                    {
                        var actividadesGestionCambio = new ActividadesGestionCambio
                        {
                            IdActividadesGestionCambio = Convert.ToInt32(IdActividadesGestionCambio),
                        };


                        var respuesta = await apiServicio.ObtenerElementoAsync<ActividadesGestionCambio>(actividadesGestionCambio, new Uri(WebApp.BaseAddress),
                                                                         "/api/ActividadesGestionCambio/ActividadesGestionCambioconIdActividad");

                        var actividades = JsonConvert.DeserializeObject<ActividadesGestionCambio>(respuesta.Resultado.ToString());

                        var actividadesGestionCambioViewModel = new ActividadesGestionCambioViewModel
                        {
                            IdPlanGestionCambio = Convert.ToInt32(actividades.IdPlanGestionCambio),

                        };


                        var viewModelActividadesGestionCambio = new ActividadesGestionCambioViewModel
                        {
                            IdPlanGestionCambio = Convert.ToInt32(actividades.IdPlanGestionCambio),
                            ListaActividadesGestionCambio = await apiServicio.Listar<ActividadesGestionCambioIndex>(actividadesGestionCambioViewModel, new Uri(WebApp.BaseAddress), "/api/ActividadesGestionCambio/ListarActividadesGestionCambioconIdPlan")
                        };


                        return View(viewModelActividadesGestionCambio);

                    }


                        ViewData["Mensaje"] = "Ir a la página de Plan Gestión Cambio";
                        return View("NoExisteElemento");
                    

                }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una actividad de gestión de cambio",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        

        public async Task<IActionResult> Delete(string IdActividadesGestionCambio, string IdPlanGestionCambio)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(IdActividadesGestionCambio, new Uri(WebApp.BaseAddress)
                                                               , "/api/ActividadesGestionCambio");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", IdActividadesGestionCambio),
                        Message = "Registro de actividad de gestión de cambio",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index", new { IdPlanGestionCambio = IdPlanGestionCambio });
                }
                else
                {
                    ViewData["Mensaje"] = Mensaje.Error;
                    return View("NoExisteElemento");
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar un actiividad gestión de cambio",
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