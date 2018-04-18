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
    public class ManualPuestosController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ManualPuestosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }
        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }
        public async Task<IActionResult> Create(string mensaje)
        {
            
            var listarie = await apiServicio.Listar<RelacionesInternasExternas>(new Uri(WebApp.BaseAddress)
                                                                   , "api/RelacionesInternasExternas/ListarRelacionesInternasExternas");
            
            var manualpuestoViewModel = new ViewModelManualPuesto
            {
                
                RelacionesInternasExternas = listarie
            };
            InicializarMensaje(mensaje);
            return View(manualpuestoViewModel);
        }

        [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModelManualPuesto viewModelManualPuesto)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(viewModelManualPuesto);
            }
            Response response = new Response();
            try
            {
                var manualPuesto = new ManualPuesto
                {
                    Nombre = viewModelManualPuesto.ManualPuesto.Nombre,
                    Descripcion = viewModelManualPuesto.ManualPuesto.Descripcion,
                    Mision = viewModelManualPuesto.ManualPuesto.Mision,
                    IdRelacionesInternasExternas = viewModelManualPuesto.ManualPuesto.IdRelacionesInternasExternas,
                };
                response = await apiServicio.InsertarAsync(manualPuesto,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ManualPuestos/InsertarManualPuesto");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un Manual Puesto",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "ManualPuesto:", viewModelManualPuesto.ManualPuesto.IdManualPuesto),
                    });

                    return RedirectToAction("Index");
                }
                var listarie = await apiServicio.Listar<RelacionesInternasExternas>(new Uri(WebApp.BaseAddress)
                                                                   , "api/RelacionesInternasExternas/ListarRelacionesInternasExternas");

                viewModelManualPuesto.RelacionesInternasExternas = listarie;
                ViewData["Error"] = response.Message;
                return View(viewModelManualPuesto);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Manual Puesto",
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
                                                                  "api/ManualPuestos");

                    InicializarMensaje(null);
                    var manualpuesto = JsonConvert.DeserializeObject<ManualPuesto>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        var listarie = await apiServicio.Listar<RelacionesInternasExternas>(new Uri(WebApp.BaseAddress)
                                                                   , "api/RelacionesInternasExternas/ListarRelacionesInternasExternas");

                        var viewmodelmanualpuesto = new ViewModelManualPuesto
                        {
                            
                            ManualPuesto = manualpuesto,
                            RelacionesInternasExternas = listarie

                        };

                        return View(viewmodelmanualpuesto);
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
        public async Task<IActionResult> Edit(string id, ViewModelManualPuesto viewModelManualPuesto)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var manualpuesto = new ManualPuesto
                    {
                        Nombre = viewModelManualPuesto.ManualPuesto.Nombre,
                        Descripcion = viewModelManualPuesto.ManualPuesto.Descripcion,
                        Mision = viewModelManualPuesto.ManualPuesto.Mision,
                        IdRelacionesInternasExternas = viewModelManualPuesto.ManualPuesto.IdRelacionesInternasExternas,
                        IdManualPuesto = viewModelManualPuesto.ManualPuesto.IdManualPuesto
                    };
                    response = await apiServicio.EditarAsync(id, manualpuesto, new Uri(WebApp.BaseAddress),
                                                                 "api/ManualPuestos");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un registro sistema",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(viewModelManualPuesto);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un Manual Puesto",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            var lista = new List<ManualPuesto>();
            try
            {
                lista = await apiServicio.Listar<ManualPuesto>(new Uri(WebApp.BaseAddress)
                                                                    , "api/ManualPuestos/ListarManualPuestos");
                InicializarMensaje(mensaje);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Manual Puestoes",
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
                                                               , "api/ManualPuestos");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = response.Message });
                // return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Manual Puestos",
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