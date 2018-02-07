using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ViewModels;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class ItinerarioViaticoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ItinerarioViaticoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Create(int IdSolicitudViatico)
        {
            ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "/api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
            var itinerarioViatico = new ItinerarioViatico
            {
                IdSolicitudViatico = IdSolicitudViatico
            };
            return View(itinerarioViatico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItinerarioViatico itinerarioViatico)
        {
            Response response = new Response();
          
            try
            {
                response = await apiServicio.InsertarAsync(itinerarioViatico,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/ItinerarioViatico/InsertarItinerarioViatico");
                if (response.IsSuccess)
                {

                    var solicitudViatico = new SolicitudViatico
                    {
                        IdSolicitudViatico = itinerarioViatico.IdSolicitudViatico
                    };

                    response = await apiServicio.InsertarAsync(solicitudViatico, new Uri(WebApp.BaseAddress),
                                                                "/api/SolicitudViaticos/ActualizarValorTotalViatico");

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un itinerario viático",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Itinerario Viático:", itinerarioViatico.IdItinerarioViatico),
                    });


                    return RedirectToAction("Index", new { IdSolicitudViatico = itinerarioViatico.IdSolicitudViatico });

                }

                ViewData["Error"] = response.Message;
                return View(itinerarioViatico);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Itinerario Viático",
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
                                                                  "/api/ItinerarioViatico");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ItinerarioViatico>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "/api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
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
        public async Task<IActionResult> Edit(string id, ItinerarioViatico itinerarioViatico)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, itinerarioViatico, new Uri(WebApp.BaseAddress),
                                                                 "/api/ItinerarioViatico");

                    if (response.IsSuccess)
                    {
                        var solicitudViatico = new SolicitudViatico
                        {
                            IdSolicitudViatico = itinerarioViatico.IdSolicitudViatico
                        };

                        response = await apiServicio.InsertarAsync(solicitudViatico, new Uri(WebApp.BaseAddress),
                                                                    "/api/SolicitudViaticos/ActualizarValorTotalViatico");

                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un itinerario viático",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index", new { IdSolicitudViatico = itinerarioViatico.IdSolicitudViatico });
                    }
                    ViewData["Error"] = response.Message;
                    return View(itinerarioViatico);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un itinerario viático",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(int IdSolicitudViatico)
        {

            SolicitudViatico sol = new SolicitudViatico();
            ListaEmpleadoViewModel empleado = new ListaEmpleadoViewModel();
            List<ItinerarioViatico> lista = new List<ItinerarioViatico>();
            try
            {

                if (IdSolicitudViatico.ToString() != null)
                {
                    var respuestaSolicitudViatico = await apiServicio.SeleccionarAsync<Response>(IdSolicitudViatico.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "/api/SolicitudViaticos");
                    if (respuestaSolicitudViatico.IsSuccess)
                    {
                        sol = JsonConvert.DeserializeObject<SolicitudViatico>(respuestaSolicitudViatico.Resultado.ToString());
                        var solicitudViatico = new SolicitudViatico
                        {
                            IdEmpleado = sol.IdEmpleado,
                        };

                        var respuestaEmpleado = await apiServicio.SeleccionarAsync<Response>(solicitudViatico.IdEmpleado.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "/api/Empleados");

                        if (respuestaEmpleado.IsSuccess)
                        {
                            var emp = JsonConvert.DeserializeObject<Empleado>(respuestaEmpleado.Resultado.ToString());
                            var empleadoEnviar = new Empleado
                            {
                                NombreUsuario = emp.NombreUsuario,
                            };

                            empleado = await apiServicio.ObtenerElementoAsync1<ListaEmpleadoViewModel>(empleadoEnviar, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDatosCompletosEmpleado");


                            lista = new List<ItinerarioViatico>();
                            var itinerarioViatico = new ItinerarioViatico
                            {
                                IdSolicitudViatico = sol.IdSolicitudViatico
                            };
                            lista = await apiServicio.ObtenerElementoAsync1<List<ItinerarioViatico>>(itinerarioViatico, new Uri(WebApp.BaseAddress)
                                                                     , "/api/ItinerarioViatico/ListarItinerariosViaticos");

                        }

                        var solicitudViaticoViewModel = new SolicitudViaticoViewModel
                        {
                            SolicitudViatico = sol,
                            ListaEmpleadoViewModel = empleado,
                            ItinerarioViatico = lista

                        };


                        var respuestaPais = await apiServicio.SeleccionarAsync<Response>(solicitudViaticoViewModel.SolicitudViatico.IdPais.ToString(), new Uri(WebApp.BaseAddress),
                                                                 "/api/Pais");
                        var pais = JsonConvert.DeserializeObject<Pais>(respuestaPais.Resultado.ToString());
                        var respuestaProvincia = await apiServicio.SeleccionarAsync<Response>(solicitudViaticoViewModel.SolicitudViatico.IdProvincia.ToString(), new Uri(WebApp.BaseAddress),
                                                                 "/api/Provincia");
                        var provincia = JsonConvert.DeserializeObject<Provincia>(respuestaProvincia.Resultado.ToString());
                        var respuestaCiudad = await apiServicio.SeleccionarAsync<Response>(solicitudViaticoViewModel.SolicitudViatico.IdCiudad.ToString(), new Uri(WebApp.BaseAddress),
                                                                 "/api/Ciudad");
                        var ciudad = JsonConvert.DeserializeObject<Ciudad>(respuestaCiudad.Resultado.ToString());



                        ViewData["FechaSolicitud"] = solicitudViaticoViewModel.SolicitudViatico.FechaSolicitud;
                        ViewData["Pais"] = pais.Nombre;
                        ViewData["Provincia"] = provincia.Nombre;
                        ViewData["Ciudad"] = ciudad.Nombre;

                        return View(solicitudViaticoViewModel);
                    }
                }


                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                 "/api/ItinerarioViatico");


                var itinerario = JsonConvert.DeserializeObject<ItinerarioViatico>(respuesta.Resultado.ToString());

                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "/api/ItinerarioViatico");
                if (response.IsSuccess)
                {
                    var solicitudViatico = new SolicitudViatico
                    {
                        IdSolicitudViatico = itinerario.IdSolicitudViatico
                    };

                    response = await apiServicio.InsertarAsync(solicitudViatico, new Uri(WebApp.BaseAddress),
                                                                "/api/SolicitudViaticos/ActualizarValorTotalViatico");
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de itinerario viático eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index", new { IdSolicitudViatico = itinerario.IdSolicitudViatico });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Itinerario Viático",
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