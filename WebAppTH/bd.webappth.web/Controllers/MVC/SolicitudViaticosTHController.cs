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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudViaticosTHController : Controller
    {
        public class ObtenerInstancia
        {
            private static SolicitudViaticoViewModel instance;

            private ObtenerInstancia() { }

            public static SolicitudViaticoViewModel Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new SolicitudViaticoViewModel();
                        instance.ItinerarioViatico = new List<ItinerarioViatico>();
                        instance.SolicitudTipoViatico = new List<SolicitudTipoViatico>();
                        instance.SolicitudViatico = new SolicitudViatico();


                    }
                    return instance;
                }
                set
                {
                    instance = null;
                }
            }
        }
        private readonly IApiServicio apiServicio;


        public SolicitudViaticosTHController(IApiServicio apiServicio)
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
       


        private async Task CargarCombos()
        {
            //Tabla Persona

            ViewData["FechaSolicitud"] = DateTime.Now;
            //ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");
            ViewData["IdPais"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdProvincia"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdTipoTransporte"] = new SelectList(await apiServicio.Listar<TipoTransporte>(new Uri(WebApp.BaseAddress), "api/TiposDeTransporte/ListarTiposDeTransporte"), "IdTipoTransporte", "Descripcion");


        }


        public async Task<IActionResult> AprobacionSolicitudViatico(int id, int idEmpleado)
        {
            var sol = new ViewModelsSolicitudViaticos
            {
                IdSolicitudViatico = id
            };
            try
            {
                await CargarCombos();
                if (id.ToString() != null)
                {
                    var respuestaSolicitudViatico = await apiServicio.ObtenerElementoAsync1<ViewModelsSolicitudViaticos>(sol, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudViaticos/ObtenerSolicitudesViaticosporId");


                    return View(respuestaSolicitudViatico);

                }
                return BadRequest();

            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }

        [HttpPost]
        public async Task<IActionResult> AprobacionSolicitudViatico(ViewModelsSolicitudViaticos viewModelsSolicitudViaticos)
        {

            try
            {
                var VerificarPresupuesto = await apiServicio.ObtenerElementoAsync1<Response>(viewModelsSolicitudViaticos, new Uri(WebApp.BaseAddress),
                                                                 "api/Presupuesto/ObtenerPresupuesto");

                if (VerificarPresupuesto.IsSuccess)
                {

                    var sol = new SolicitudViatico()
                    {
                        IdSolicitudViatico = viewModelsSolicitudViaticos.IdSolicitudViatico,
                        Estado = 3

                    };

                    var respuestaEmpleado = await apiServicio.EditarAsync<Response>(sol, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudViaticos/ActualizarEstadoSolicitudViatico");
                    if (respuestaEmpleado.IsSuccess)
                    {
                        return RedirectToAction("DetalleSolicitudViaticos", new { id = viewModelsSolicitudViaticos.IdEmpleado });
                    }
                }
                return this.RedireccionarMensajeTime("SolicitudViaticosTH", "AprobacionSolicitudViatico", new { id = viewModelsSolicitudViaticos.IdSolicitudViatico }, $"{Mensaje.Error}|{VerificarPresupuesto.Message}|{"25000"}");

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> Reliquidacion(int id ,int IdEmpleado)
        {

            try
            {
                var sol = new SolicitudViatico()
                {
                    IdSolicitudViatico = id,
                    Estado = 5

                };
                var respuestaEmpleado = await apiServicio.EditarAsync<Response>(sol, new Uri(WebApp.BaseAddress),
                                                             "api/SolicitudViaticos/ActualizarEstadoSolicitudViatico");
                if (respuestaEmpleado.IsSuccess)
                {
                    return RedirectToAction("DetalleSolicitudViaticos",new { id = IdEmpleado });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Detalle(int id)
        {
            SolicitudViatico sol = new SolicitudViatico();
            ListaEmpleadoViewModel empleado = new ListaEmpleadoViewModel();
            List<ItinerarioViatico> lista = new List<ItinerarioViatico>();
            try
            {
                if (id.ToString() != null)
                {
                    var respuestaSolicitudViatico = await apiServicio.SeleccionarAsync<Response>(id.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "api/SolicitudViaticos");
                    if (respuestaSolicitudViatico.IsSuccess)
                    {
                        var solicitudViaticoViewModel = new SolicitudViaticoViewModel();

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
                            
                            if (lista.Count != 0)
                            {
                                solicitudViaticoViewModel.SolicitudViatico = sol;
                                solicitudViaticoViewModel.ListaEmpleadoViewModel = empleado;
                                solicitudViaticoViewModel.ItinerarioViatico = lista;
                                
                            }
                            else
                            {
                                //return this.Redireccionar("SolicitudViaticosTH", "ListadoEmpleadosSolicitudViaticos", $"{Mensaje.ErrorItinerario}");
                                return this.RedireccionarMensajeTime("SolicitudViaticosTH", "DetalleSolicitudViaticos", new { id = sol.IdEmpleado }, $"{Mensaje.ErrorItinerario}|{"25000"}");

                            }

                        }


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

        public async Task<IActionResult> DetalleSolicitudViaticos(int id)
        {
            var empleado = new Empleado()
            {
                IdEmpleado = id
            };

            var lista = new List<SolicitudViatico>();
            try
            {
                lista = await apiServicio.Listar<SolicitudViatico>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/SolicitudViaticos/ListarSolicitudesViaticosPorEmpleado");

                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Solicitud Planificación Vacaciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> ListadoEmpleadosSolicitudViaticos()
        {
            try
            {
                var lista = new List<EmpleadoSolicitudViewModel>();
                lista = await apiServicio.Listar<EmpleadoSolicitudViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosTalentoHumanoconSolucitudesViaticos");
                return View(lista);
                //return usuariologueado;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return View();
            }

        }


        

        public async Task<ListaEmpleadoViewModel> ObtenerEmpleado(string nombreUsuario)
        {



            try
            {
                if (!string.IsNullOrEmpty(nombreUsuario))
                {
                    var emp = new Empleado
                    {
                        NombreUsuario = nombreUsuario
                    };
                    var empleado = await apiServicio.ObtenerElementoAsync1<ListaEmpleadoViewModel>(emp, new Uri(WebApp.BaseAddress),
                                                                  "api/Empleados/ObtenerDatosCompletosEmpleado");


                    //respuesta.Resultado = JsonConvert.DeserializeObject<SolicitudViatico>(respuesta.Resultado.ToString());
                    //if (respuesta.IsSuccess)
                    //{
                    return empleado;
                    //}

                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<JsonResult> ListarProvinciaPorPais(string pais)
        {
            var Pais = new Pais
            {
                IdPais = Convert.ToInt32(pais),
            };
            var listaProvincias = await apiServicio.Listar<Provincia>(Pais, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            return Json(listaProvincias);
        }

        public async Task<JsonResult> ListarCiudadPorProvincia(string provincia)
        {
            var Provincia = new Provincia
            {
                IdProvincia = Convert.ToInt32(provincia),
            };
            var listaCiudades = await apiServicio.Listar<Ciudad>(Provincia, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorProvincia");
            return Json(listaCiudades);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEstado(SolicitudViaticoViewModel solicitudViaticoViewModel)
        {
            Response response = new Response();
            try
            {

                response = await apiServicio.InsertarAsync(solicitudViaticoViewModel, new Uri(WebApp.BaseAddress),
                                                             "api/SolicitudViaticos/ActualizarEstadoSolicitudViatico");

                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", solicitudViaticoViewModel.SolicitudViatico.IdSolicitudViatico),
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        Message = "Se ha actualizado un solicitud viático",
                        UserName = "Usuario 1"
                    });

                    return RedirectToAction("DetalleSolicitudViaticos", new { id = solicitudViaticoViewModel.SolicitudViatico.IdEmpleado });
                }
                ViewData["Error"] = response.Message;
                return View(solicitudViaticoViewModel);



            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un solicitud viático",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

    }
}