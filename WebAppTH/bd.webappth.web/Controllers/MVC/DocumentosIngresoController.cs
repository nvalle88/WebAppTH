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
    public class DocumentosIngresoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public DocumentosIngresoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Search()
        {
            Empleado empleado = new Empleado();
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentosIngreso documentosIngreso)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(documentosIngreso,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/DocumentosIngreso/InsertarDocumentosIngreso");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un documento de ingreso",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Documento Ingreso:", documentosIngreso.IdDocumentosIngreso),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                return View(documentosIngreso);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Documento Ingreso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DocumentoEntregado(ViewModelDocumentoIngresoEmpleado viewModelDocumentoIngresoEmpleado)
        {
            Response response = new Response();
            var listaDocumentosEntregados = new List<DocumentosIngresoEmpleado>();
            try
            {
                var empleado = new Empleado
                {
                    IdEmpleado= viewModelDocumentoIngresoEmpleado.empleadoViewModel.IdEmpleado
                };

                listaDocumentosEntregados = await apiServicio.ObtenerElementoAsync1<List<DocumentosIngresoEmpleado>>(empleado, new Uri(WebApp.BaseAddress)
                                                                  , "/api/DocumentosIngreso/ListarDocumentosIngresoEmpleado");

                if (listaDocumentosEntregados.Count == 0)
                {
                    response = await apiServicio.InsertarAsync(viewModelDocumentoIngresoEmpleado,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/DocumentosIngreso/InsertarDocumentosIngresoEmpleado");
                }
                else
                {
                    response = await apiServicio.ObtenerElementoAsync1<Response>(viewModelDocumentoIngresoEmpleado, new Uri(WebApp.BaseAddress),
                                                                                     "/api/DocumentosIngreso/EditarCheckListDocumentos");
                }
                
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un documento de ingreso empleado",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Documento Ingreso Empleado:", viewModelDocumentoIngresoEmpleado.empleadoViewModel.IdEmpleado),
                    });

                    return RedirectToAction("Search");
                }

                ViewData["Error"] = response.Message;
                return RedirectToAction("Search");

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Documento Ingreso",
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
                                                                  "/api/DocumentosIngreso");


                    respuesta.Resultado = JsonConvert.DeserializeObject<DocumentosIngreso>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, DocumentosIngreso documentosIngreso)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, documentosIngreso, new Uri(WebApp.BaseAddress),
                                                                 "/api/DocumentosIngreso");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un documento de ingreso",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(documentosIngreso);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un documento de ingreso",
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

            var lista = new List<DocumentosIngreso>();
            try
            {
                lista = await apiServicio.Listar<DocumentosIngreso>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/DocumentosIngreso/ListarDocumentosIngreso");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando documentos de ingreso",
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
                                                               , "/api/DocumentosIngreso");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de documento de ingreso eliminado",
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
                    Message = "Eliminar Documento Ingreso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }


        public async Task<IActionResult> CheckListDocumentosEmpleado(Empleado empl)
        {
            var listaDocumentos = new List<DocumentosIngreso>();
            var listaDocumentosEntregados = new List<DocumentosIngresoEmpleado>();
            try
            {
                var empleado = new Empleado
                {
                    Persona = new Persona
                    {
                        Identificacion = empl.Persona.Identificacion
                    }
                };
                var emp = await apiServicio.ObtenerElementoAsync1<ListaEmpleadoViewModel>(empleado, new Uri(WebApp.BaseAddress),
                                                              "/api/Empleados/ObtenerDatosCompletosEmpleado");

                var empleadoConsulta = new Empleado
                {
                    IdEmpleado = emp.IdEmpleado
                };
                //var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(empleadoConsulta, new Uri(WebApp.BaseAddress), "/api/DocumentosIngreso/GetDocumentoIngresoEmpleado");
                //if (!respuesta.IsSuccess)
                //{
                    listaDocumentos = await apiServicio.Listar<DocumentosIngreso>(new Uri(WebApp.BaseAddress)
                                                                                      , "/api/DocumentosIngreso/ListarDocumentosIngreso");
                    listaDocumentosEntregados = await apiServicio.ObtenerElementoAsync1<List<DocumentosIngresoEmpleado>>(empleadoConsulta,new Uri(WebApp.BaseAddress)
                                                                                      , "/api/DocumentosIngreso/ListarDocumentosIngresoEmpleado");

                    var documentoingresoViewModel = new ViewModelDocumentoIngresoEmpleado
                    {
                        empleadoViewModel = emp,
                        listadocumentosingreso = listaDocumentos,
                        listadocumentosingresoentregado = listaDocumentosEntregados

                    };

                    return View(documentoingresoViewModel);
                //}

            }
            catch (Exception)
            {
                return View(new ViewModelDocumentoIngresoEmpleado());
            }
        }

        //public async Task<IActionResult> CheckListDocumentosEmpleados(ListaEmpleadoViewModel empleado)
        //{

        //    var lista = new List<DocumentosIngreso>();
         
        //    try
        //    {
        //        var emp = new Empleado
        //        {
        //            IdEmpleado = empleado.IdEmpleado
        //        };
        //        var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(emp, new Uri(WebApp.BaseAddress), "/api/DocumentosIngreso/GetDocumentoIngresoEmpleado");
        //        if (!respuesta.IsSuccess)
        //        {
        //            lista = await apiServicio.Listar<DocumentosIngreso>(new Uri(WebApp.BaseAddress)
        //                                                                              , "/api/DocumentosIngreso/ListarDocumentosIngreso");
        //            var documentoingresoViewModel = new ViewModelDocumentoIngresoEmpleado
        //            {
        //                empleadoViewModel = empleado,
        //                listadocumentosingreso = lista
        //            };

        //            return View(documentoingresoViewModel);
        //        }
                
        //        return View (new ViewModelDocumentoIngresoEmpleado());
        //    }
        //    catch (Exception ex)
        //    {
        //        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
        //        {
        //            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
        //            Message = "Listando documentos de ingreso",
        //            ExceptionTrace = ex.Message,
        //            LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
        //            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
        //            UserName = "Usuario APP webappth"
        //        });
        //        return BadRequest();
        //    }
        //}
    }
}