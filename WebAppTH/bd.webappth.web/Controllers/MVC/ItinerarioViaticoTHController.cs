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
using bd.webappth.entidades.Constantes;
using Microsoft.AspNetCore.Http;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class ItinerarioViaticoTHController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ItinerarioViaticoTHController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create(int IdSolicitudViatico)
        {


            ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
            var itinerarioViatico = new ItinerarioViatico
            {
                IdSolicitudViatico = IdSolicitudViatico
            };
            InicializarMensaje(null);
            return View(itinerarioViatico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItinerarioViatico itinerarioViatico)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(itinerarioViatico);
            }
            var solicitudViatico = new SolicitudViatico
            {
                IdSolicitudViatico = itinerarioViatico.IdSolicitudViatico
            };
            Response response = new Response();

            try
            {
                var solicitud = await apiServicio.ObtenerElementoAsync1<SolicitudViatico>(solicitudViatico, new Uri(WebApp.BaseAddress)
                                                                  , "api/SolicitudViaticos/ListarSolicitudesViaticosPorId");

                itinerarioViatico.FechaDesde = solicitud.FechaSalida;
                itinerarioViatico.HoraSalida = solicitud.HoraSalida;
                itinerarioViatico.FechaHasta = solicitud.FechaLlegada;
                itinerarioViatico.HoraLlegada = solicitud.HoraLlegada;


                response = await apiServicio.InsertarAsync(itinerarioViatico,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ItinerarioViatico/InsertarItinerarioViatico");
                if (response.IsSuccess)
                {
                    response = await apiServicio.InsertarAsync(solicitudViatico, new Uri(WebApp.BaseAddress),
                                                                "api/SolicitudViaticos/ActualizarValorTotalViatico");

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

        #region InformeVIaticos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actividades(InformeViatico informeViatico)
        {
            Response response = new Response();

            try
            {
                response = await apiServicio.InsertarAsync(informeViatico,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/InformeViaticos/Actividades");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico });
                }

                ViewData["Error"] = response.Message;
                return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico });
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarInforme(InformeViatico informeViatico)
        {
            Response response = new Response();

            try
            {
                var solicitud = new SolicitudViatico
                {
                    IdSolicitudViatico = informeViatico.IdSolicitudViatico

                };
                response = await apiServicio.ObtenerElementoAsync(solicitud,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/InformeViaticos/ActualizarEstadoInforme");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico });
                }

                ViewData["Error"] = response.Message;
                return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico });
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> Informe(int IdSolicitudViatico, string mensaje)
        {

            List<InformeViatico> lista = new List<InformeViatico>();

            try
            {
                var sol = new ViewModelsSolicitudViaticos
                {
                    IdSolicitudViatico = IdSolicitudViatico,

                };
                if (IdSolicitudViatico.ToString() != null)
                {
                    var respuestaSolicitudViatico = await apiServicio.ObtenerElementoAsync1<ViewModelsSolicitudViaticos>(sol, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudViaticos/ObtenerSolicitudesViaticosporId");

                    //InicializarMensaje(null);
                    if (respuestaSolicitudViatico != null)
                    {

                        lista = new List<InformeViatico>();
                        var itinerarioViatico = new InformeViatico
                        {
                            IdSolicitudViatico = IdSolicitudViatico
                        };
                        lista = await apiServicio.ObtenerElementoAsync1<List<InformeViatico>>(itinerarioViatico, new Uri(WebApp.BaseAddress)
                                                                 , "api/InformeViaticos/ListarInformeViaticos");

                        var facturas = new FacturaViatico()
                        {
                            IdSolicitudViatico = IdSolicitudViatico

                        };

                        var listaFacruras = await apiServicio.Listar<FacturaViatico>(facturas, new Uri(WebApp.BaseAddress)
                                                                 , "api/FacturaViatico/ListarFacturas");
                        HttpContext.Session.SetInt32(Constantes.IdSolicitudtinerario, IdSolicitudViatico);

                        //busca las actividades del informe
                        var informeViatico = new InformeViatico
                        {
                            IdSolicitudViatico = IdSolicitudViatico
                        };
                        var Actividades = await apiServicio.ObtenerElementoAsync1<InformeActividadViatico>(informeViatico, new Uri(WebApp.BaseAddress)
                                                                 , "api/InformeViaticos/ObtenerActividades");

                        respuestaSolicitudViatico.ListaInformeViatico = lista;
                        respuestaSolicitudViatico.ListaFacturaViatico = listaFacruras;
                        respuestaSolicitudViatico.InformeActividadViatico = Actividades;
                        InicializarMensaje(mensaje);
                        return View(respuestaSolicitudViatico);
                    }
                }
                InicializarMensaje(null);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> CreateInforme()
        {

            var idIrininario = HttpContext.Session.GetInt32(Constantes.IdItinerario);
            var IdSolicitudtinerario = HttpContext.Session.GetInt32(Constantes.IdSolicitudtinerario);
            ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
            ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            var itinerarioViatico = new InformeViatico
            {
                IdSolicitudViatico = Convert.ToInt32(IdSolicitudtinerario)

            };
            InicializarMensaje(null);
            return View(itinerarioViatico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInforme(InformeViatico informeViatico)
        {
            Response response = new Response();

            try
            {
                response = await apiServicio.InsertarAsync(informeViatico,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/InformeViaticos/InsertarInformeViatico");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico });
                }

                ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
                ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                ViewData["Error"] = response.Message;
                return View(informeViatico);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> EditInforme(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/InformeViaticos");


                    respuesta.Resultado = JsonConvert.DeserializeObject<InformeViatico>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
                        ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                        ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                        return View(respuesta.Resultado);
                    }

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInforme(string id, InformeViatico informeViatico)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, informeViatico, new Uri(WebApp.BaseAddress),
                                                                 "api/InformeViaticos");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("Informe", new { IdItinerario = informeViatico.IdSolicitudViatico });
                    }
                    ViewData["Error"] = response.Message;
                    return View(informeViatico);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {


                return BadRequest();
            }
        }
        public async Task<IActionResult> DeleteInforme(string id)
        {
            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/InformeViaticos");
                if (response.IsSuccess)
                {
                    var idIrininario = HttpContext.Session.GetInt32(Constantes.IdItinerario);
                    return RedirectToAction("Informe", new { IdItinerario = idIrininario });
                }
                return RedirectToAction("Informe", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {


                return BadRequest();
            }
        }
        #endregion

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ItinerarioViatico");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ItinerarioViatico>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
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
                                                                 "api/ItinerarioViatico");

                    if (response.IsSuccess)
                    {
                        var solicitudViatico = new SolicitudViatico
                        {
                            IdSolicitudViatico = itinerarioViatico.IdSolicitudViatico
                        };

                        response = await apiServicio.InsertarAsync(solicitudViatico, new Uri(WebApp.BaseAddress),
                                                                    "api/SolicitudViaticos/ActualizarValorTotalViatico");

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

        public async Task<IActionResult> Index(int IdSolicitudViatico, string mensaje)
        {
            try
            {
                var sol = new ViewModelsSolicitudViaticos
                {
                    IdSolicitudViatico = IdSolicitudViatico
                };
                if (IdSolicitudViatico.ToString() != null)
                {
                    var respuestaSolicitudViatico = await apiServicio.ObtenerElementoAsync1<ViewModelsSolicitudViaticos>(sol, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudViaticos/ObtenerSolicitudesViaticosporId");

                    
                    InicializarMensaje(mensaje);
                    return View(respuestaSolicitudViatico);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarReliquidacion(ReliquidacionViatico reliquidacionViatico)
        {
            Response response = new Response();

            try
            {
                if (reliquidacionViatico.ValorTotalRequlidacion > reliquidacionViatico.ValorRequlidacion)
                {
                    return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico }, $"{Mensaje.Error}|{"El valor supera al valor total de la reliquidacion"}|{"25000"}");

                }
                if (reliquidacionViatico.ValorTotalRequlidacion < reliquidacionViatico.ValorRequlidacion)
                {
                    return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico }, $"{Mensaje.Error}|{"El valor es inferior al valor de la reliquidacion"}|{"25000"}");
                }
                var presupuesto = new Presupuesto
                {
                    IdPresupuesto = reliquidacionViatico.IdPresupuesto,

                };
                var solicitudViatico = new SolicitudViatico
                {
                    IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico,

                };

                var solicitudViaticoViewModel = new SolicitudViaticoViewModel
                {
                    Presupuesto = presupuesto,
                    Valor = reliquidacionViatico.ValorTotalRequlidacion,
                    SolicitudViatico = solicitudViatico
                };

                response = await apiServicio.ObtenerElementoAsync1<Response>(solicitudViaticoViewModel, new Uri(WebApp.BaseAddress),
                                                                 "api/Presupuesto/ObtenerPresupuesto");

                if (response.IsSuccess)
                {
                    var solicitud = new SolicitudViatico
                    {
                        IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico

                    };
                    response = await apiServicio.ObtenerElementoAsync(solicitud,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/ReliquidacionViaticos/ActualizarEstadoReliquidacion");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("DetalleSolicitudViaticos", "SolicitudViaticosTH", new { id = reliquidacionViatico.IdEmpleado });
                    }
                    ViewData["Error"] = response.Message;
                    return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico }, $"{Mensaje.Error}|{response.Message}|{"25000"}");
                }
                ViewData["Error"] = response.Message;
                return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico }, $"{Mensaje.Error}|{response.Message}|{"25000"}");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                 "api/ItinerarioViatico");


                var itinerario = JsonConvert.DeserializeObject<ItinerarioViatico>(respuesta.Resultado.ToString());

                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/ItinerarioViatico");
                if (response.IsSuccess)
                {
                    var solicitudViatico = new SolicitudViatico
                    {
                        IdSolicitudViatico = itinerario.IdSolicitudViatico
                    };

                    response = await apiServicio.InsertarAsync(solicitudViatico, new Uri(WebApp.BaseAddress),
                                                                "api/SolicitudViaticos/ActualizarValorTotalViatico");
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
                //return BadRequest();
                return RedirectToAction("Index", new { mensaje = response.Message });
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

        #region Reliquidacion
        public async Task<IActionResult> Reliquidacion(int IdSolicitudViatico, string mensaje)
        {

            SolicitudViatico sol = new SolicitudViatico();
            ListaEmpleadoViewModel empleado = new ListaEmpleadoViewModel();
            List<ReliquidacionViatico> lista = new List<ReliquidacionViatico>();
            try
            {

                if (IdSolicitudViatico.ToString() != null)
                {
                    var respuestaSolicitudViatico = await apiServicio.SeleccionarAsync<Response>(IdSolicitudViatico.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "api/SolicitudViaticos");
                    //InicializarMensaje(null);
                    if (respuestaSolicitudViatico.IsSuccess)
                    {
                        sol = JsonConvert.DeserializeObject<SolicitudViatico>(respuestaSolicitudViatico.Resultado.ToString());
                        var solicitudViatico = new SolicitudViatico
                        {
                            IdEmpleado = sol.IdEmpleado,
                        };

                        var respuestaEmpleado = await apiServicio.SeleccionarAsync<Response>(solicitudViatico.IdEmpleado.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Empleados");

                        if (respuestaEmpleado.IsSuccess)
                        {
                            var emp = JsonConvert.DeserializeObject<Empleado>(respuestaEmpleado.Resultado.ToString());
                            var empleadoEnviar = new Empleado
                            {
                                NombreUsuario = emp.NombreUsuario,
                            };

                            empleado = await apiServicio.ObtenerElementoAsync1<ListaEmpleadoViewModel>(empleadoEnviar, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDatosCompletosEmpleado");


                            lista = new List<ReliquidacionViatico>();
                            var reliquidacionViatico = new ReliquidacionViatico
                            {
                                IdSolicitudViatico = IdSolicitudViatico
                            };
                            lista = await apiServicio.ObtenerElementoAsync1<List<ReliquidacionViatico>>(reliquidacionViatico, new Uri(WebApp.BaseAddress)
                                                                     , "api/ReliquidacionViaticos/ListarReliquidaciones");

                            var valortotaReliquidacion = lista.Sum(x => x.ValorEstimado);

                            var ItinerarioViatico = new ItinerarioViatico
                            {
                                IdSolicitudViatico = sol.IdSolicitudViatico
                            };

                            var listaitinirario = await apiServicio.ObtenerElementoAsync1<List<ItinerarioViatico>>(ItinerarioViatico, new Uri(WebApp.BaseAddress)
                                                                     , "api/ItinerarioViatico/ListarItinerariosViaticos");

                            //}
                            ///Valor total de Itinerario
                            var valortotalVIaticos = listaitinirario.Sum(x => x.SolicitudViatico.ValorEstimado);
                            ///Informe
                            var informeViatico = new InformeViatico
                            {
                                IdSolicitudViatico = IdSolicitudViatico
                            };

                            var listaintforme = await apiServicio.ObtenerElementoAsync1<List<InformeViatico>>(informeViatico, new Uri(WebApp.BaseAddress)
                                                                      , "api/InformeViaticos/ListarInformeViaticos");
                            ///Valor total de informe
                            var valortotaInforme = listaintforme.Sum(x => x.ValorEstimado);

                            ///total facturas
                            var facturas = new FacturaViatico()
                            {
                                IdSolicitudViatico = IdSolicitudViatico

                            };
                            var listaFacruras = await apiServicio.Listar<FacturaViatico>(facturas, new Uri(WebApp.BaseAddress)
                                                                     , "api/FacturaViatico/ListarFacturas");
                            ///Valor total de Itinerario
                            var valortotalfacturas = listaFacruras.Sum(x => x.ValorTotalFactura);

                            var valorCalculo = (valortotalVIaticos * 70) / 100;

                            var ValorReliquidacion = calculos(Convert.ToDecimal(valortotaInforme), valortotalfacturas, Convert.ToDecimal(valorCalculo), Convert.ToDecimal(valortotalVIaticos));


                            HttpContext.Session.SetInt32(Constantes.IdSolicitudtinerario, IdSolicitudViatico);
                            HttpContext.Session.SetInt32(Constantes.ValorReliquidacion, Convert.ToInt32(ValorReliquidacion.Valor));

                            //busca las actividades del informe
                            var actividades = new InformeViatico
                            {
                                IdSolicitudViatico = IdSolicitudViatico
                            };
                            var Actividades = await apiServicio.ObtenerElementoAsync1<InformeActividadViatico>(actividades, new Uri(WebApp.BaseAddress)
                                                                     , "api/InformeViaticos/ObtenerActividades");
                            var descri = "";
                            if (Actividades == null)
                            {
                                descri = "";
                            }
                            else
                            {
                                descri = Actividades.Descripcion;
                            }

                            var informeViaticoViewModel = new ReliquidacionViaticoViewModel
                            {
                                SolicitudViatico = sol,
                                ListaEmpleadoViewModel = empleado,
                                ReliquidacionViatico = lista,
                                //FacturaViatico = listaFacruras,
                                IdSolicitudViatico = sol.IdSolicitudViatico,
                                Descripcion = descri,
                                ValorReliquidacion = ValorReliquidacion.Valor,
                                EstadoReliquidacion = ValorReliquidacion.Reliquidacion,
                                ValorTotalReliquidacion = Convert.ToDecimal(valortotaReliquidacion)
                            };

                            //var respuestaPais = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdPais.ToString(), new Uri(WebApp.BaseAddress),
                            //                                         "api/Pais");
                            //var pais = JsonConvert.DeserializeObject<Pais>(respuestaPais.Resultado.ToString());
                            //var respuestaProvincia = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdProvincia.ToString(), new Uri(WebApp.BaseAddress),
                            //                                         "api/Provincia");
                            //var provincia = JsonConvert.DeserializeObject<Provincia>(respuestaProvincia.Resultado.ToString());
                            //var respuestaCiudad = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdCiudad.ToString(), new Uri(WebApp.BaseAddress),
                            //                                         "api/Ciudad");
                            //var ciudad = JsonConvert.DeserializeObject<Ciudad>(respuestaCiudad.Resultado.ToString());
                            //ViewData["IdPresupuesto"] = new SelectList(await apiServicio.Listar<Presupuesto>(new Uri(WebApp.BaseAddress), "api/Presupuesto/ListarPresupuesto"), "IdPresupuesto", "NumeroPartidaPresupuestaria");

                            //ViewData["Pais"] = pais.Nombre;
                            //ViewData["Provincia"] = provincia.Nombre;
                            //ViewData["Ciudad"] = ciudad.Nombre;
                            InicializarMensaje(mensaje);
                            return View(informeViaticoViewModel);
                        }
                    }

                }
                InicializarMensaje(null);
                return View();
            }
            catch (Exception exe)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> CreateReliquidacion()
        {

            var idIrininario = HttpContext.Session.GetInt32(Constantes.IdItinerario);
            var IdSolicitudtinerario = HttpContext.Session.GetInt32(Constantes.IdSolicitudtinerario);
            var ValorRequlidacion = HttpContext.Session.GetInt32(Constantes.ValorReliquidacion);
            ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
            ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdItemViatico"] = new SelectList(await apiServicio.Listar<ItemViatico>(new Uri(WebApp.BaseAddress), "api/ItemViaticos/ListarItemViaticosConReliquidacion"), "IdItemViatico", "Descripcion");
            var itinerarioViatico = new ReliquidacionViatico
            {
                IdSolicitudViatico = Convert.ToInt32(IdSolicitudtinerario),
                ValorRequlidacion = Convert.ToInt32(ValorRequlidacion)
            };
            InicializarMensaje(null);
            return View(itinerarioViatico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReliquidacion(ReliquidacionViatico reliquidacionViatico)
        {
            try
            {
                if (reliquidacionViatico.ValorRequlidacion == reliquidacionViatico.ValorEstimado)
                {

                    Response response = new Response();
                    var solicitudViatico = new SolicitudViatico
                    {
                        IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico
                    };


                    var solicitud = await apiServicio.ObtenerElementoAsync1<SolicitudViatico>(solicitudViatico, new Uri(WebApp.BaseAddress)
                                                                      , "api/SolicitudViaticos/ListarSolicitudesViaticosPorId");
                    if (solicitud != null)
                    {
                        reliquidacionViatico.FechaSalida = solicitud.FechaSalida;
                        reliquidacionViatico.HoraSalida = solicitud.HoraSalida;
                        reliquidacionViatico.FechaLlegada = solicitud.FechaLlegada;
                        reliquidacionViatico.HoraLlegada = solicitud.HoraLlegada;

                        response = await apiServicio.InsertarAsync(reliquidacionViatico,
                                                                     new Uri(WebApp.BaseAddress),
                                                                     "api/ReliquidacionViaticos/InsertarReliquidacionViatico");
                        if (response.IsSuccess)
                        {
                            return RedirectToAction("Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico });
                        }
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}";
                }
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"El valor ingresado es incorrecto al valor de la reliquidacion"}";
                ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
                ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                ViewData["IdItemViatico"] = new SelectList(await apiServicio.Listar<ItemViatico>(new Uri(WebApp.BaseAddress), "api/ItemViaticos/ListarItemViaticosConReliquidacion"), "IdItemViatico", "Descripcion");
                return View(reliquidacionViatico);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> EditReliquidacion(string id, int IdSolicitudViatico)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ReliquidacionViaticos");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ReliquidacionViatico>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        ViewData["IdSolicitud"] = IdSolicitudViatico;
                        ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
                        ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                        ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                        return View(respuesta.Resultado);
                    }

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReliquidacion(string id, ReliquidacionViatico reliquidacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, reliquidacion, new Uri(WebApp.BaseAddress),
                                                                 "api/ReliquidacionViaticos");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("Informe", new { IdSolicitudViatico = reliquidacion.IdSolicitudViatico });
                    }
                    ViewData["Error"] = response.Message;
                    return View(reliquidacion);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteReliquidacion(string id, int IdSolicitudViatico)
        {
            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/ReliquidacionViaticos");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Reliquidacion", new { IdSolicitudViatico = IdSolicitudViatico });
                }
                return RedirectToAction("Reliquidacion", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {


                return BadRequest();
            }
        }
        #endregion

        SolicitudViaticoViewModel calculos(decimal valortotaInforme, decimal valortotalfacturas, decimal valorCalculo, decimal valortotalVIaticos)
        {
            var solicitudViaticoViewModel = new SolicitudViaticoViewModel();


            if (valorCalculo >= valortotaInforme && valorCalculo >= valortotalfacturas)
            {
                solicitudViaticoViewModel.Reliquidacion = 0;
            }
            else if (valortotalfacturas > valortotaInforme)
            {
                solicitudViaticoViewModel.Reliquidacion = 1;
                solicitudViaticoViewModel.Valor = valortotalfacturas - valortotaInforme;
            }
            else if (valortotaInforme < valorCalculo && valortotalfacturas < valorCalculo)
            {
                solicitudViaticoViewModel.Reliquidacion = -1;
                solicitudViaticoViewModel.Valor = valortotalfacturas - valortotaInforme;
            }

            return solicitudViaticoViewModel;
        }
    }
}