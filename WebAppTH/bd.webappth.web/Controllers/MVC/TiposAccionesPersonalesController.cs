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
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class TiposAccionesPersonalesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TiposAccionesPersonalesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
           
            var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
            {

                MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },

            };

            ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");
            ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoAccionPersonal TipoAccionPersonal)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(TipoAccionPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/TiposAccionesPersonales/InsertarTipoAccionPersonal");

                var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
                {

                    MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}                            },

                };



                if (response.IsSuccess)
                {

                    LogEntryTranfer logEntryTranfer = new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un tipo  de accion personal",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = "Tipo de Accion de Personal",
                        ObjectPrevious = "NULL",
                        ObjectNext = JsonConvert.SerializeObject(response.Resultado),
                    };

                    var responseLog = await GuardarLogService.SaveLogEntry(logEntryTranfer);


                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");
                ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");

                return View(tipoAccionPersonalViewmodel);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando tipo de Acción de Peronal",
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
                                                                  "/api/TiposAccionesPersonales");


                    respuesta.Resultado = JsonConvert.DeserializeObject<TipoAccionPersonal>(respuesta.Resultado.ToString());

                    var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
                    {

                        MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },


                        TipoAccionPersonal = (TipoAccionPersonal)respuesta.Resultado
                    };

                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");
                    ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");


                    if (respuesta.IsSuccess)
                    {
                        return View(tipoAccionPersonalViewmodel);
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
        public async Task<IActionResult> Edit(string id, TipoAccionPersonal TipoAccionPersonal)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    
                    var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
                    {

                        MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },
                        
                    };

                    response = await apiServicio.EditarAsync(id, TipoAccionPersonal, new Uri(WebApp.BaseAddress),
                                                                 "/api/TiposAccionesPersonales");
                    

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Nacionalidad Indígena", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una nacionalidad indígena",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;


                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");
                    ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");


                    return View(tipoAccionPersonalViewmodel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un tipo de acción de personal",
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

            var lista = new List<TipoAccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/TiposAccionesPersonales/ListarTiposAccionesPersonales");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando un tipo de acción de personal",
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
                                                               , "/api/TiposAccionesPersonales");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de tipo de acción de personal",
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
                    Message = "Eliminar una tipo de acción de personal",
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