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
            Response response = new Response();

            try
            {
                response = await apiServicio.InsertarAsync(itinerarioViatico,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ItinerarioViatico/InsertarItinerarioViatico");
                if (response.IsSuccess)
                {

                    var solicitudViatico = new SolicitudViatico
                    {
                        IdSolicitudViatico = itinerarioViatico.IdSolicitudViatico
                    };

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
                    return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico, IdItinerarioViatico = informeViatico.IdItinerarioViatico });
                }

                ViewData["Error"] = response.Message;
                return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico, IdItinerarioViatico = informeViatico.IdItinerarioViatico });
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
                    return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico, IdItinerarioViatico = informeViatico.IdItinerarioViatico });
                }

                ViewData["Error"] = response.Message;
                return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico, IdItinerarioViatico = informeViatico.IdItinerarioViatico });
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        //public async Task<IActionResult> Informe(int IdItinerario, string mensaje)
        //{

        //    var informe = new InformeViatico()
        //    {
        //        IdItinerarioViatico = IdItinerario

        //    };
        //    var lista = new List<InformeViatico>();
        //    try
        //    {
        //        lista = await apiServicio.Listar<InformeViatico>(informe, new Uri(WebApp.BaseAddress)
        //                                                            , "api/InformeViaticos/ListarInformeViaticos");
        //        HttpContext.Session.SetInt32(Constantes.IdItinerario, IdItinerario);

        //        InicializarMensaje(null);
        //        return View(lista);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}
        public async Task<IActionResult> Informe(int IdSolicitudViatico, int IdItinerarioViatico, string mensaje)
        {

            SolicitudViatico sol = new SolicitudViatico();
            ListaEmpleadoViewModel empleado = new ListaEmpleadoViewModel();
            List<InformeViatico> lista = new List<InformeViatico>();
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


                            lista = new List<InformeViatico>();
                            var itinerarioViatico = new InformeViatico
                            {
                                IdItinerarioViatico = IdItinerarioViatico
                            };
                            lista = await apiServicio.ObtenerElementoAsync1<List<InformeViatico>>(itinerarioViatico, new Uri(WebApp.BaseAddress)
                                                                     , "api/InformeViaticos/ListarInformeViaticos");

                            var facturas = new FacturaViatico()
                            {
                                IdItinerarioViatico = IdItinerarioViatico

                            };

                            var listaFacruras = await apiServicio.Listar<FacturaViatico>(facturas, new Uri(WebApp.BaseAddress)
                                                                     , "api/FacturaViatico/ListarFacturas");
                            HttpContext.Session.SetInt32(Constantes.IdItinerario, IdItinerarioViatico);
                            HttpContext.Session.SetInt32(Constantes.IdSolicitudtinerario, IdSolicitudViatico);

                            //busca las actividades del informe
                            var informeViatico = new InformeViatico
                            {
                                IdItinerarioViatico = IdItinerarioViatico
                            };
                            var Actividades = await apiServicio.ObtenerElementoAsync1<InformeActividadViatico>(informeViatico, new Uri(WebApp.BaseAddress)
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
                            var informeViaticoViewModel = new InformeViaticoViewModel
                            {
                                SolicitudViatico = sol,
                                ListaEmpleadoViewModel = empleado,
                                InformeViatico = lista,
                                FacturaViatico = listaFacruras,
                                IdItinerarioViatico = IdItinerarioViatico,
                                IdSolicitudViatico = sol.IdSolicitudViatico,
                                Descripcion = descri
                            };

                            var respuestaPais = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdPais.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Pais");
                            var pais = JsonConvert.DeserializeObject<Pais>(respuestaPais.Resultado.ToString());
                            var respuestaProvincia = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdProvincia.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Provincia");
                            var provincia = JsonConvert.DeserializeObject<Provincia>(respuestaProvincia.Resultado.ToString());
                            var respuestaCiudad = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdCiudad.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Ciudad");
                            var ciudad = JsonConvert.DeserializeObject<Ciudad>(respuestaCiudad.Resultado.ToString());


                            ViewData["Pais"] = pais.Nombre;
                            ViewData["Provincia"] = provincia.Nombre;
                            ViewData["Ciudad"] = ciudad.Nombre;
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

        public async Task<IActionResult> CreateInforme()
        {

            var idIrininario = HttpContext.Session.GetInt32(Constantes.IdItinerario);
            var IdSolicitudtinerario = HttpContext.Session.GetInt32(Constantes.IdSolicitudtinerario);
            ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
            ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            var itinerarioViatico = new InformeViatico
            {
                IdItinerarioViatico = Convert.ToInt32(idIrininario),
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
                    return RedirectToAction("Informe", new { IdSolicitudViatico = informeViatico.IdSolicitudViatico, IdItinerarioViatico = informeViatico.IdItinerarioViatico });
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
                        return RedirectToAction("Informe", new { IdItinerario = informeViatico.IdItinerarioViatico });
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

            SolicitudViatico sol = new SolicitudViatico();
            ListaEmpleadoViewModel empleado = new ListaEmpleadoViewModel();
            List<ItinerarioViatico> lista = new List<ItinerarioViatico>();
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


                            lista = new List<ItinerarioViatico>();
                            var itinerarioViatico = new ItinerarioViatico
                            {
                                IdSolicitudViatico = sol.IdSolicitudViatico
                            };

                            lista = await apiServicio.ObtenerElementoAsync1<List<ItinerarioViatico>>(itinerarioViatico, new Uri(WebApp.BaseAddress)
                                                                     , "api/ItinerarioViatico/ListarItinerariosViaticos");

                            //}
                            ///Valor total de Itinerario
                            var valortotalVIaticos = lista.Sum(x => x.Valor);
                            ///Informe
                            var informeViatico = new InformeViatico
                            {
                                IdItinerarioViatico = lista.FirstOrDefault().IdItinerarioViatico
                            };

                            var listaintforme = await apiServicio.ObtenerElementoAsync1<List<InformeViatico>>(informeViatico, new Uri(WebApp.BaseAddress)
                                                                      , "api/InformeViaticos/ListarInformeViaticos");
                            ///Valor total de informe
                            var valortotaInforme = listaintforme.Sum(x => x.ValorEstimado);

                            ///total facturas
                            var facturas = new FacturaViatico()
                            {
                                IdItinerarioViatico = lista.FirstOrDefault().IdItinerarioViatico

                            };
                            var listaFacruras = await apiServicio.Listar<FacturaViatico>(facturas, new Uri(WebApp.BaseAddress)
                                                                     , "api/FacturaViatico/ListarFacturas");
                            ///Valor total de Itinerario
                            var valortotalfacturas = listaFacruras.Sum(x => x.ValorTotalFactura);

                            var valorCalculo = (valortotalVIaticos * 70) / 100;

                            var EstadoReliquidacion = new SolicitudViaticoViewModel();

                            if ((listaFacruras.Count() > 0 || listaintforme.Count > 0) && sol.Estado == 4)
                            {
                                EstadoReliquidacion = calculos(Convert.ToDecimal(valortotaInforme), valortotalfacturas, valorCalculo, valortotalVIaticos);
                            }

                            // metodo de activacion de Reliquidacion                            

                            var solicitudViaticoViewModel = new SolicitudViaticoViewModel
                            {
                                SolicitudViatico = sol,
                                ListaEmpleadoViewModel = empleado,
                                ItinerarioViatico = lista,
                                Reliquidacion = EstadoReliquidacion.Reliquidacion,
                                //Valor = EstadoReliquidacion.Valor
                            };


                            var respuestaPais = await apiServicio.SeleccionarAsync<Response>(solicitudViaticoViewModel.SolicitudViatico.IdPais.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Pais");
                            var pais = JsonConvert.DeserializeObject<Pais>(respuestaPais.Resultado.ToString());
                            var respuestaProvincia = await apiServicio.SeleccionarAsync<Response>(solicitudViaticoViewModel.SolicitudViatico.IdProvincia.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Provincia");
                            var provincia = JsonConvert.DeserializeObject<Provincia>(respuestaProvincia.Resultado.ToString());
                            var respuestaCiudad = await apiServicio.SeleccionarAsync<Response>(solicitudViaticoViewModel.SolicitudViatico.IdCiudad.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Ciudad");
                            var ciudad = JsonConvert.DeserializeObject<Ciudad>(respuestaCiudad.Resultado.ToString());



                            // ViewData["FechaSolicitud"] = solicitudViaticoViewModel.SolicitudViatico.FechaSolicitud;
                            ViewData["Pais"] = pais.Nombre;
                            ViewData["Provincia"] = provincia.Nombre;
                            ViewData["Ciudad"] = ciudad.Nombre;
                            InicializarMensaje(mensaje);
                            return View(solicitudViaticoViewModel);
                        }
                    }
                    //return RedirectToAction("Index", new { mensaje = respuestaEmpleado.Message });
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
        public async Task<IActionResult> FinalizarReliquidacion(ReliquidacionViatico reliquidacionViatico)
        {
            Response response = new Response();

            try
            {
                if (reliquidacionViatico.ValorTotalRequlidacion > reliquidacionViatico.ValorRequlidacion)
                {
                    return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico, IdItinerarioViatico = reliquidacionViatico.IdItinerarioViatico }, $"{Mensaje.Error}|{"El valor supera al valor total de la reliquidacion"}|{"25000"}");

                }
                if(reliquidacionViatico.ValorTotalRequlidacion < reliquidacionViatico.ValorRequlidacion) {
                    return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico, IdItinerarioViatico = reliquidacionViatico.IdItinerarioViatico }, $"{Mensaje.Error}|{"El valor es inferior al valor de la reliquidacion"}|{"25000"}");
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
                    return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico, IdItinerarioViatico = reliquidacionViatico.IdItinerarioViatico }, $"{Mensaje.Error}|{response.Message}|{"25000"}");
                }
                ViewData["Error"] = response.Message;
                return this.RedireccionarMensajeTime("ItinerarioViaticoTH", "Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico, IdItinerarioViatico = reliquidacionViatico.IdItinerarioViatico }, $"{Mensaje.Error}|{response.Message}|{"25000"}");

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
        public async Task<IActionResult> Reliquidacion(int IdSolicitudViatico, int IdItinerarioViatico, string mensaje)
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
                                IdItinerarioViatico = IdItinerarioViatico
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
                            var valortotalVIaticos = listaitinirario.Sum(x => x.Valor);
                            ///Informe
                            var informeViatico = new InformeViatico
                            {
                                IdItinerarioViatico = lista.FirstOrDefault().IdItinerarioViatico
                            };

                            var listaintforme = await apiServicio.ObtenerElementoAsync1<List<InformeViatico>>(informeViatico, new Uri(WebApp.BaseAddress)
                                                                      , "api/InformeViaticos/ListarInformeViaticos");
                            ///Valor total de informe
                            var valortotaInforme = listaintforme.Sum(x => x.ValorEstimado);

                            ///total facturas
                            var facturas = new FacturaViatico()
                            {
                                IdItinerarioViatico = lista.FirstOrDefault().IdItinerarioViatico

                            };
                            var listaFacruras = await apiServicio.Listar<FacturaViatico>(facturas, new Uri(WebApp.BaseAddress)
                                                                     , "api/FacturaViatico/ListarFacturas");
                            ///Valor total de Itinerario
                            var valortotalfacturas = listaFacruras.Sum(x => x.ValorTotalFactura);

                            var valorCalculo = (valortotalVIaticos * 70) / 100;

                            var ValorReliquidacion = calculos(Convert.ToDecimal(valortotaInforme), valortotalfacturas, valorCalculo, valortotalVIaticos);


                            //if ((listaFacruras.Count() > 0 || listaintforme.Count > 0) && sol.Estado == 4)
                            //{

                            //}

                            HttpContext.Session.SetInt32(Constantes.IdItinerario, IdItinerarioViatico);
                            HttpContext.Session.SetInt32(Constantes.IdSolicitudtinerario, IdSolicitudViatico);

                            //busca las actividades del informe
                            var actividades = new InformeViatico
                            {
                                IdItinerarioViatico = IdItinerarioViatico
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
                                IdItinerarioViatico = IdItinerarioViatico,
                                IdSolicitudViatico = sol.IdSolicitudViatico,
                                Descripcion = descri,
                                ValorReliquidacion = ValorReliquidacion.Valor,
                                ValorTotalReliquidacion = Convert.ToDecimal(valortotaReliquidacion)
                            };

                            var respuestaPais = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdPais.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Pais");
                            var pais = JsonConvert.DeserializeObject<Pais>(respuestaPais.Resultado.ToString());
                            var respuestaProvincia = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdProvincia.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Provincia");
                            var provincia = JsonConvert.DeserializeObject<Provincia>(respuestaProvincia.Resultado.ToString());
                            var respuestaCiudad = await apiServicio.SeleccionarAsync<Response>(informeViaticoViewModel.SolicitudViatico.IdCiudad.ToString(), new Uri(WebApp.BaseAddress),
                                                                     "api/Ciudad");
                            var ciudad = JsonConvert.DeserializeObject<Ciudad>(respuestaCiudad.Resultado.ToString());
                            ViewData["IdPresupuesto"] = new SelectList(await apiServicio.Listar<Presupuesto>(new Uri(WebApp.BaseAddress), "api/Presupuesto/ListarPresupuesto"), "IdPresupuesto", "NumeroPartidaPresupuestaria");

                            ViewData["Pais"] = pais.Nombre;
                            ViewData["Provincia"] = provincia.Nombre;
                            ViewData["Ciudad"] = ciudad.Nombre;
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
            ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
            ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            var itinerarioViatico = new ReliquidacionViatico
            {
                IdItinerarioViatico = Convert.ToInt32(idIrininario),
                IdSolicitudViatico = Convert.ToInt32(IdSolicitudtinerario)

            };
            InicializarMensaje(null);
            return View(itinerarioViatico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReliquidacion(ReliquidacionViatico reliquidacionViatico)
        {
            Response response = new Response();

            try
            {
                response = await apiServicio.InsertarAsync(reliquidacionViatico,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ReliquidacionViaticos/InsertarReliquidacionViatico");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Reliquidacion", new { IdSolicitudViatico = reliquidacionViatico.IdSolicitudViatico, IdItinerarioViatico = reliquidacionViatico.IdItinerarioViatico });
                }

                ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");
                ViewData["IdCiudadDestino"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                ViewData["IdCiudadOrigen"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                ViewData["Error"] = response.Message;
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
                        return RedirectToAction("Informe", new { IdSolicitudViatico = reliquidacion.IdSolicitudViatico, IdItinerarioViatico = reliquidacion.IdItinerarioViatico });
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
                    var idIrininario = HttpContext.Session.GetInt32(Constantes.IdItinerario);
                    return RedirectToAction("Reliquidacion", new { IdSolicitudViatico = IdSolicitudViatico, IdItinerarioViatico = idIrininario });
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

            decimal ValorReportado = 0;
            int estado = 0;


            if (valortotaInforme == valortotalfacturas)
            {
                ValorReportado = Convert.ToDecimal(valortotalfacturas);
            }
            if (valortotaInforme > valortotalfacturas)
            {
                ValorReportado = Convert.ToDecimal(valortotaInforme - valortotalfacturas);
            }
            if (valortotaInforme < valortotalfacturas)
            {
                ValorReportado = Convert.ToDecimal(valortotalfacturas - valortotaInforme);
            }
            ///  
            if (ValorReportado >= valorCalculo && ValorReportado <= valortotalVIaticos)
            {
                solicitudViaticoViewModel.Reliquidacion = 0;
            }
            if (ValorReportado <= valorCalculo)
            {
                solicitudViaticoViewModel.Valor = valorCalculo - ValorReportado;
                solicitudViaticoViewModel.Reliquidacion = 1;
            }
            if (ValorReportado > valortotalVIaticos)
            {
                solicitudViaticoViewModel.Valor = ValorReportado - valortotalVIaticos;
                solicitudViaticoViewModel.Reliquidacion = 2;
            }

            return solicitudViaticoViewModel;
        }
    }
}