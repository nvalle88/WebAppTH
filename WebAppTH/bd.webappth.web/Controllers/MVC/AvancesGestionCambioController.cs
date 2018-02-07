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
    public class AvancesGestionCambioController : Controller
    {
        private readonly IApiServicio apiServicio;


        public AvancesGestionCambioController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create(int IdActividadesGestionCambio)
        {
            var AvanceGestionCambio = new AvanceGestionCambio
            {
                IdActividadesGestionCambio = IdActividadesGestionCambio,
                Fecha=DateTime.Now
            };
            return View(AvanceGestionCambio);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AvanceGestionCambio AvanceGestionCambio)
        {
            Response response = new Response();
            try
            {
                var actividadesGestionCambio = new ActividadesGestionCambio
                {
                    IdActividadesGestionCambio = AvanceGestionCambio.IdActividadesGestionCambio,
                };

                var respuestaActividades = await apiServicio.ObtenerElementoAsync<ActividadesGestionCambio>(actividadesGestionCambio, new Uri(WebApp.BaseAddress),
                                                                       "/api/ActividadesGestionCambio/ActividadesGestionCambioconIdActividad");

                var actividades = JsonConvert.DeserializeObject<ActividadesGestionCambio>(respuestaActividades.Resultado.ToString());

                var respuestaSuma = await apiServicio.ObtenerElementoAsync<ActividadesGestionCambio>(actividadesGestionCambio, new Uri(WebApp.BaseAddress),
                                                                         "/api/AvancesGestionCambio/AvanceGestionCambioSumaIndicadorReal");

                var suma = JsonConvert.DeserializeObject<AvanceGestionCambioModel>(respuestaSuma.Resultado.ToString());

                if ((suma.Suma + AvanceGestionCambio.Indicadorreal) <= actividades.Indicador)
                {
                    
                    response = await apiServicio.InsertarAsync(AvanceGestionCambio,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "/api/AvancesGestionCambio/InsertarAvanceGestionCambio");
                    if (response.IsSuccess)
                    {

                        var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            ExceptionTrace = null,
                            Message = "Se ha creado un avance gestión de cambio",
                            UserName = "Usuario 1",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            EntityID = string.Format("{0} {1}", "Avance gestión de cambio:", AvanceGestionCambio.IdAvanceGestionCambio),
                        });

                        return RedirectToAction("Index", new { IdActividadesGestionCambio = AvanceGestionCambio.IdActividadesGestionCambio });
                    }
                    else
                    {
                        ViewData["Error"] = response.Message;
                        return View(AvanceGestionCambio);
                    }
                }
                else
                {
                    string mensaje = "No se puede crear el avance de control cambio ya que el indicador de la actividad es " + actividades.Indicador +" y la suma del indicador real es de "+ suma.Suma + "  y al aumentarle "+AvanceGestionCambio.Indicadorreal+" sobrepasaría en " + ((suma.Suma + AvanceGestionCambio.Indicadorreal) - actividades.Indicador);
                    ViewData["Error"] = mensaje;
                    return View(AvanceGestionCambio);
                }
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un avance gestión de cambio",
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
                                                                  "/api/AvancesGestionCambio");


                    respuesta.Resultado = JsonConvert.DeserializeObject<AvanceGestionCambio>(respuesta.Resultado.ToString());

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
        public async Task<IActionResult> Edit(string id, AvanceGestionCambio AvanceGestionCambio)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, AvanceGestionCambio, new Uri(WebApp.BaseAddress),
                                                                 "/api/AvancesGestionCambio");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Avance de gestión de cambio", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un avance de gestión de cambio",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index", new { IdActividadesGestionCambio = AvanceGestionCambio.IdActividadesGestionCambio });
                    }
                    ViewData["Error"] = response.Message;

                    return View(AvanceGestionCambio);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una actividad gestión de cambio",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(int IdActividadesGestionCambio)
        {


            try
            {

                if (IdActividadesGestionCambio != 0)
                { 
                        var avancesGestionCambio = new AvanceGestionCambio
                        {
                            IdActividadesGestionCambio = Convert.ToInt32(IdActividadesGestionCambio),

                        };

                        var viewModelAvanceGestionCambio = new AvanceGestionCambioViewModel
                        {
                            IdActividadesGestionCambio = Convert.ToInt32(IdActividadesGestionCambio),
                            ListaAvancesGestionCambio = await apiServicio.Listar<AvanceGestionCambio>(avancesGestionCambio, new Uri(WebApp.BaseAddress), "/api/AvancesGestionCambio/ListarAvanceGestionCambioconIdActividad")
                        };

                        return View(viewModelAvanceGestionCambio);
                 }

                ViewData["Mensaje"] = "Ir a la página de Plan Gestión Cambio";
                return View("NoExisteElemento");
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una avance de gestión de cambio",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

       
        public async Task<IActionResult> Delete(string IdAvanceGestionCambio, string IdActividadesGestionCambio)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(IdAvanceGestionCambio, new Uri(WebApp.BaseAddress)
                                                               , "/api/AvancesGestionCambio");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", IdAvanceGestionCambio),
                        Message = "Registro de un avance de gestión de cambio",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index", new { IdActividadesGestionCambio = IdActividadesGestionCambio });
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
                    Message = "Eliminar un avance gestión de cambio",
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