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
using System.Security.Claims;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudesPermisosController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitudesPermisosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> RevisaPermisos()
        {

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            Empleado jefe = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");
           
            ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(jefe,new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadosAsuCargo"), "IdEmpleado", "NombreApellido", jefe);

            return View();
        }

        public async Task<IActionResult> AprobarPermisos(int id)
        {
            
            try
            {

                ViewData["Error"] = string.Empty;

                SolicitudPermisoViewModel solicitudPermisoViewModel = await apiServicio.ObtenerElementoAsync1<SolicitudPermisoViewModel>(id, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ObtenerInformacionSolicitudPermiso");

                ViewData["NombresApellidosEmpleado"] = solicitudPermisoViewModel.NombresApellidosEmpleado;
                ViewData["NombreDependencia"] = solicitudPermisoViewModel.NombreDependencia;
                ViewData["SolicitudPermiso.FechaDesde"] = solicitudPermisoViewModel.SolicitudPermiso.FechaDesde;
                ViewData["SolicitudPermiso.HoraDesde"] = solicitudPermisoViewModel.SolicitudPermiso.HoraDesde;
                ViewData["SolicitudPermiso.FechaHasta"] = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta;
                ViewData["SolicitudPermiso.HoraHasta"] = solicitudPermisoViewModel.SolicitudPermiso.HoraHasta;
                ViewData["SolicitudPermiso.TipoPermiso.Nombre"] = solicitudPermisoViewModel.SolicitudPermiso.TipoPermiso.Nombre;
                ViewData["SolicitudPermiso.Motivo"] = solicitudPermisoViewModel.SolicitudPermiso.Motivo;
                
                var item = new SolicitudPermisoViewModel
                {
                   NombresApellidosEmpleado =solicitudPermisoViewModel.NombresApellidosEmpleado,
                   NombreDependencia =solicitudPermisoViewModel.NombreDependencia,
                   IdTipoPermiso =solicitudPermisoViewModel.SolicitudPermiso.IdTipoPermiso,
                   EstadoLista = new List<ListaEstado>
                            {
                                new ListaEstado {Id = "-1", Nombre = "Denegado"},
                                new ListaEstado {Id ="0", Nombre = "Devuelto"},
                                new ListaEstado {Id = "1", Nombre = "Aprobado"}
                            },
                    SolicitudPermiso= solicitudPermisoViewModel.SolicitudPermiso
                };

                var i = ViewData["IdListaEstado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(item.EstadoLista, "Id", "Nombre");


                ViewData["IdListaEstado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(item.EstadoLista, "Id", "Nombre");

                return View(item);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una solicitud de permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudPermiso SolicitudPermiso)
        {
            Response response = new Response();
            try
            {



                response = await apiServicio.InsertarAsync(SolicitudPermiso,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/SolicitudesPermisos/InsertarSolicitudPermiso");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una solicitud permiso",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Solicitud Permiso:", SolicitudPermiso.IdSolicitudPermiso),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");
            
                return View(SolicitudPermiso);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando una solicitud permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }

        

        public async Task<IActionResult> PedirPermiso()
        {
            
            try
            {

                ViewData["Error"] = string.Empty;

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                Empleado empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");


                var respuesta = await apiServicio.SeleccionarAsync<Response>(empleado.IdEmpleado.ToString(), new Uri(WebApp.BaseAddress),
                                                                 "api/Empleados");


                respuesta.Resultado = JsonConvert.DeserializeObject<Empleado>(respuesta.Resultado.ToString());
               
                ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");


                var solicitudPermiso = new SolicitudPermiso
                {
                    Empleado = (Empleado)respuesta.Resultado,
                    FechaDesde = DateTime.Now,
                    FechaHasta= DateTime.Now

                };
                
                var solicitudPermisoViewModel = new SolicitudPermisoViewModel
                {
                    NombresApellidosEmpleado = solicitudPermiso.Empleado.Persona.Nombres + " " + solicitudPermiso.Empleado.Persona.Apellidos,
                    NombreDependencia = solicitudPermiso.Empleado.Dependencia.Nombre,
                    SolicitudPermiso = solicitudPermiso,

                    EstadoLista = new List<ListaEstado>
                            {
                                new ListaEstado {Id = "-1", Nombre = "Denegado"},
                                new ListaEstado {Id ="0", Nombre = "Devuelto"},
                                new ListaEstado {Id = "1", Nombre = "Aprobado"}
                            },
                };

                ViewData["IdListaEstado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(solicitudPermisoViewModel.EstadoLista, "Id", "Nombre");

                return View(solicitudPermisoViewModel);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Ingresar solicitud permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PedirPermiso(SolicitudPermisoViewModel solicitudPermisoViewModel)
        {
           
            try
            {
                string mensaje = string.Empty;

                if (solicitudPermisoViewModel.SolicitudPermiso.FechaDesde > solicitudPermisoViewModel.SolicitudPermiso.FechaHasta)
                {
                    
                    mensaje = "La fecha desde debe ser menor que la fecha hasta";
                    ViewData["Error"] = mensaje;
                    ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");

                    return View(solicitudPermisoViewModel);
                }

                string fechaDesde = solicitudPermisoViewModel.SolicitudPermiso.FechaDesde.DayOfWeek.ToString();
                string fechaHasta = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta.DayOfWeek.ToString();


                if (fechaDesde.Equals("Saturday") || fechaDesde.Equals("Sunday"))
                {
                    mensaje = "La fecha desde no puede ser ni Sábado, ni Domingo";
                    ViewData["Error"] = mensaje;
                    ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");

                    return View(solicitudPermisoViewModel);

                }

                if (fechaHasta.Equals("Saturday") || fechaHasta.Equals("Sunday"))
                {
                    mensaje = "La fecha hasta no puede ser ni Sábado, ni Domingo";
                    ViewData["Error"] = mensaje;
                    ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");

                    return View(solicitudPermisoViewModel);

                }

                // Diferencia en horas
                TimeSpan? diferenciaHoras = solicitudPermisoViewModel.SolicitudPermiso.HoraHasta - solicitudPermisoViewModel.SolicitudPermiso.HoraDesde;

                // Diferencia en dìas
                TimeSpan? tsDiferenciaDias = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta - solicitudPermisoViewModel.SolicitudPermiso.FechaDesde;
                
                var solicitudPermiso = new SolicitudPermiso
                {
                    IdEmpleado = solicitudPermisoViewModel.SolicitudPermiso.Empleado.IdEmpleado,
                    Motivo = solicitudPermisoViewModel.SolicitudPermiso.Motivo,
                    IdTipoPermiso = solicitudPermisoViewModel.IdTipoPermiso,
                    FechaSolicitud = DateTime.Now,
                    FechaDesde = solicitudPermisoViewModel.SolicitudPermiso.FechaDesde,
                    FechaHasta = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta,
                    HoraDesde = solicitudPermisoViewModel.SolicitudPermiso.HoraDesde,
                    HoraHasta = solicitudPermisoViewModel.SolicitudPermiso.HoraHasta,

                };

                var response = await apiServicio.InsertarAsync(solicitudPermiso, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/InsertarSolicitudPermiso");

                if (response.IsSuccess)
                {
                    LogEntryTranfer logEntryTranfer = new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una solicitud permiso",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = "Solicitud Permiso",
                        ObjectPrevious = "NULL",
                        ObjectNext = JsonConvert.SerializeObject(response.Resultado),
                    };

                    var responseLog = await GuardarLogService.SaveLogEntry(logEntryTranfer);

                    mensaje = Mensaje.Satisfactorio;

                    return RedirectToAction("IndexEmpleado");
                }
               

                ViewData["Error"] = response.Message;
                ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");
                ViewData["IdListaEstado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(solicitudPermisoViewModel.EstadoLista, "Id", "Nombre");

                return View(solicitudPermisoViewModel);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Error al crear una solicitud de permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
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
                                                                  "api/SolicitudesPermisos");


                    respuesta.Resultado = JsonConvert.DeserializeObject<SolicitudPermiso>(respuesta.Resultado.ToString());

                    ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");
                    
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
        public async Task<IActionResult> Edit(string id, SolicitudPermiso SolicitudPermiso)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, SolicitudPermiso, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudesPermisos");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Solicitud de Permiso", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una solicitud de permiso",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");
                    
                    return View(SolicitudPermiso);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una solicitud de permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }


        public async Task<IActionResult> EditarPedirPermiso(string id)
        {
            try
            {
                ViewData["Error"] = string.Empty;

                ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");
     
                SolicitudPermisoViewModel solicitudPermisoViewModel = await apiServicio.ObtenerElementoAsync1<SolicitudPermisoViewModel>(id, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ObtenerInformacionSolicitudPermiso");
                
                return View(solicitudPermisoViewModel);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una solicitud de permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPedirPermiso(string id, SolicitudPermisoViewModel solicitudPermisoViewModel)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    solicitudPermisoViewModel.SolicitudPermiso.Estado = 0;
                    response = await apiServicio.EditarAsync(id, solicitudPermisoViewModel.SolicitudPermiso, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudesPermisos");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Solicitud de Permiso", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una solicitud de permiso",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                  var a=  ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");


                    ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");
                    
                    return View(solicitudPermisoViewModel.SolicitudPermiso);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una solicitud de permiso",
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
        public async Task<IActionResult> AprobarPermisos(string id, SolicitudPermisoViewModel solicitudPermisoViewModel)
        {
            Response response = new Response();
            try
            {
                if (solicitudPermisoViewModel!=null)
                {

                    var solicitudPermiso = new SolicitudPermiso
                    {
                        IdSolicitudPermiso = solicitudPermisoViewModel.SolicitudPermiso.IdSolicitudPermiso,
                        IdEmpleado = solicitudPermisoViewModel.SolicitudPermiso.IdEmpleado,
                        FechaSolicitud = solicitudPermisoViewModel.SolicitudPermiso.FechaSolicitud,
                        HoraDesde = solicitudPermisoViewModel.SolicitudPermiso.HoraDesde,
                        HoraHasta = solicitudPermisoViewModel.SolicitudPermiso.HoraHasta,
                        FechaDesde = solicitudPermisoViewModel.SolicitudPermiso.FechaDesde,
                        FechaHasta = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta,
                        Motivo = solicitudPermisoViewModel.SolicitudPermiso.Motivo,
                        Estado = solicitudPermisoViewModel.Estado,
                        IdTipoPermiso = solicitudPermisoViewModel.SolicitudPermiso.IdTipoPermiso,
                        FechaAprobado = solicitudPermisoViewModel.SolicitudPermiso.FechaAprobado,
                        Observacion = solicitudPermisoViewModel.SolicitudPermiso.Observacion
                    };


                    var item = new SolicitudPermisoViewModel
                    {
                       
                        EstadoLista = new List<ListaEstado>
                            {
                                new ListaEstado {Id = "-1", Nombre = "Denegado"},
                                new ListaEstado {Id ="0", Nombre = "Devuelto"},
                                new ListaEstado {Id = "1", Nombre = "Aprobado"}
                            },
                        SolicitudPermiso= solicitudPermiso
                    };

                    response = await apiServicio.EditarAsync(id, item.SolicitudPermiso, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudesPermisos");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Solicitud de Permiso", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una solicitud de permiso",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexJefe");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdTipoPermiso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso"), "IdTipoPermiso", "Nombre");
                    ViewData["IdListaEstado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(solicitudPermisoViewModel.EstadoLista, "Id", "Nombre");

                    return View(item);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una solicitud de permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }
        

        public async Task<IActionResult> IndexEmpleado()
        {

            var lista = new List<SolicitudPermiso>();
            try
            {


                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                Empleado empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");
                
                lista = await apiServicio.Listar<SolicitudPermiso>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/SolicitudesPermisos/ListarSolicitudesPermisosEmpleado");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una solicitud de permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexJefe()
        {

            var lista = new List<ListaEmpleadoViewModel>();
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                Empleado empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");

                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/EmpleadosAsuCargoSolicitudPermiso");
                
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una solicitud de permiso",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        
        public async Task<IActionResult> ListarInformacionEmpleado(int id)
        {

            var lista = new List<SolicitudPermiso>();
            try
            {

                var solicitudPermiso = new SolicitudPermiso
                {
                    IdEmpleado = id,
                };

                lista = await apiServicio.Listar<SolicitudPermiso>(solicitudPermiso, new Uri(WebApp.BaseAddress)
                                                                    , "api/SolicitudesPermisos/ListarInformacionEmpleado");


                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una solicitud de permiso",
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
                                                               , "api/SolicitudesPermisos");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de solicitud de permiso",
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
                    Message = "Eliminar una solicitud de permiso",
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