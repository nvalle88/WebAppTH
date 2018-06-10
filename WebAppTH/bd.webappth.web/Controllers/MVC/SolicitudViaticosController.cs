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
    public class SolicitudViaticosController : Controller
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


        public SolicitudViaticosController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create()
        {
            InicializarMensaje(null);
            ObtenerInstancia.Instance = null;

            await CargarCombos();

            var lista = new List<TipoViatico>();
            lista = await apiServicio.Listar<TipoViatico>(new Uri(WebApp.BaseAddress)
                                                                   , "api/TiposDeViatico/ListarTiposDeViatico");

            var SolicitudViaticoViewModel = new SolicitudViaticoViewModel
            {
                ListaTipoViatico = lista
            };

            return View(SolicitudViaticoViewModel);
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

        public async Task<ActionResult> ListadoEmpleadosSolicitudViaticos()
        {
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var listadoEmpleados = await ListarEmpleadosPertenecientesaDependenciaconViaticos(NombreUsuario);
            return View(listadoEmpleados);
        }



        public async Task<IActionResult> AprobacionSolicitudViatico(int id)
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

                        }

                        var solicitudViaticoViewModel = new SolicitudViaticoViewModel
                        {
                            SolicitudViatico = sol,
                            ListaEmpleadoViewModel = empleado,
                            ItinerarioViatico = lista

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



                        //ViewData["FechaSolicitud"] = solicitudViaticoViewModel.SolicitudViatico.FechaSolicitud;
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

                        }

                        var solicitudViaticoViewModel = new SolicitudViaticoViewModel
                        {
                            SolicitudViatico = sol,
                            ListaEmpleadoViewModel = empleado,
                            ItinerarioViatico = lista

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



                        //ViewData["FechaSolicitud"] = solicitudViaticoViewModel.SolicitudViatico.FechaSolicitud;
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
                    Message = "Listando Solicitud Planificaci�n Vacaciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<List<EmpleadoSolicitudViewModel>> ListarEmpleadosPertenecientesaDependenciaconViaticos(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.Listar<EmpleadoSolicitudViewModel>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeJefeconSolucitudesViaticos");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new List<EmpleadoSolicitudViewModel>();
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudViaticoViewModel solicitudViaticoViewModel)
        {
            try
            {
                Response response = new Response();
                var lista = new List<TipoViatico>();
                if (solicitudViaticoViewModel.SolicitudViatico.FechaSalida <= solicitudViaticoViewModel.SolicitudViatico.FechaLlegada)
                {
                    if (ModelState.IsValid)
                    {
                        InicializarMensaje(null);
                        return View(solicitudViaticoViewModel);
                    }
                    
                    var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                    var empleado = await ObtenerEmpleado(NombreUsuario);

                    solicitudViaticoViewModel.SolicitudViatico.IdEmpleado = empleado.IdEmpleado;
                    solicitudViaticoViewModel.SolicitudViatico.IdConfiguracionViatico = empleado.IdConfiguracionViatico;
                    solicitudViaticoViewModel.SolicitudViatico.FechaSolicitud = DateTime.Now.Date;

                    response = await apiServicio.InsertarAsync(solicitudViaticoViewModel,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudViaticos/InsertarSolicitudViatico");
                    if (response.IsSuccess)
                    {
                        var respuesta = JsonConvert.DeserializeObject<SolicitudViatico>(response.Resultado.ToString());                    
                        return RedirectToAction("Create", "ItinerarioViatico", new { IdSolicitudViatico = respuesta.IdSolicitudViatico });
                    }
                    
                    lista = await apiServicio.Listar<TipoViatico>(new Uri(WebApp.BaseAddress)
                                                                           , "api/TiposDeViatico/ListarTiposDeViatico");
                    solicitudViaticoViewModel.ListaTipoViatico = lista;
                    await CargarCombos();
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    return View(solicitudViaticoViewModel);


                }
                lista = new List<TipoViatico>();
                await CargarCombos();
                lista = await apiServicio.Listar<TipoViatico>(new Uri(WebApp.BaseAddress)
                                                                       , "api/TiposDeViatico/ListarTiposDeViatico");
                solicitudViaticoViewModel.ListaTipoViatico = lista;
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Fechas Incorrectas"}";
                return View(solicitudViaticoViewModel);
            }
            catch (Exception ex)
            {


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
                                                                  "api/SolicitudViaticos");


                    var solicitudViatico = JsonConvert.DeserializeObject<SolicitudViatico>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        await CargarCombos();

                        var listaTiposViaticos = new List<TipoViatico>();
                        var listaSolicitudTiposViaticos = new List<SolicitudTipoViatico>();
                        listaTiposViaticos = await apiServicio.Listar<TipoViatico>(new Uri(WebApp.BaseAddress)
                                                                               , "api/TiposDeViatico/ListarTiposDeViatico");
                        var solicitudTipoViatico = new SolicitudTipoViatico
                        {
                            IdSolicitudViatico = Convert.ToInt32(id)
                        };

                        listaSolicitudTiposViaticos = await apiServicio.ObtenerElementoAsync1<List<SolicitudTipoViatico>>(solicitudTipoViatico, new Uri(WebApp.BaseAddress)
                                                                              , "api/TiposDeViatico/ListarSolicitudesTiposViaticos");

                        var SolicitudViaticoViewModel = new SolicitudViaticoViewModel
                        {
                            SolicitudViatico = solicitudViatico,
                            ListaTipoViatico = listaTiposViaticos,
                            SolicitudTipoViatico = listaSolicitudTiposViaticos

                        };

                        return View(SolicitudViaticoViewModel);
                    }

                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
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
        public async Task<IActionResult> Edit(string id, SolicitudViatico solicitudViatico)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, solicitudViatico, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudViaticos");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un solicitud vi�tico",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(solicitudViatico);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un solicitud vi�tico",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEstado(SolicitudViaticoViewModel solicitudViaticoViewModel)
        {
            Response response = new Response();
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var empleado = await ObtenerEmpleado(NombreUsuario);

                var soli = new SolicitudViatico
                {
                    Estado = solicitudViaticoViewModel.SolicitudViatico.Estado,
                    IdSolicitudViatico = solicitudViaticoViewModel.SolicitudViatico.IdSolicitudViatico,
                    IdEmpleadoAprobador = empleado.IdEmpleado
                };

                response = await apiServicio.InsertarAsync(soli, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudViaticos/ActualizarEstadoSolicitudViatico");

                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", solicitudViaticoViewModel.SolicitudViatico.IdSolicitudViatico),
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        Message = "Se ha actualizado un solicitud vi�tico",
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
                    Message = "Editando un solicitud vi�tico",
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

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var empleadoJson = ObtenerEmpleado(NombreUsuario);
            var idEmpleado = empleadoJson.Result.IdEmpleado;

            var empleado = new Empleado()
            {
                IdEmpleado = idEmpleado
            };

            var lista = new List<SolicitudViatico>();
            try
            {
                lista = await apiServicio.Listar<SolicitudViatico>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/SolicitudViaticos/ListarSolicitudesViaticosPorEmpleado");

                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando estados civiles",
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
                                                               , "api/SolicitudViaticos");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de solicitud vi�tico eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}", "Index");
                //return RedirectToAction("Index", new { mensaje = response.Message });
                
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Solicitud Vi�tico",
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