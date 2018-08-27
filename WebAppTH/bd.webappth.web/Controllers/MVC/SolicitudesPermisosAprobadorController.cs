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
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudesPermisosAprobadorController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitudesPermisosAprobadorController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }
        
        

        public async Task<IActionResult> PedirPermiso()
        {
            
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var usuario = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");


                var respuesta = await apiServicio.SeleccionarAsync<Response>(usuario.IdEmpleado.ToString(), new Uri(WebApp.BaseAddress),"api/Empleados");


                var empleado = JsonConvert.DeserializeObject<Empleado>(respuesta.Resultado.ToString());
               
                

                var solicitudPermisoViewModel = new SolicitudPermisoViewModel
                {
                    NombresApellidosEmpleado = empleado.Persona.Nombres + " " + empleado.Persona.Apellidos,
                    NombreDependencia = empleado.Dependencia.Nombre,

                    SolicitudPermiso = new SolicitudPermiso {
                        FechaDesde = DateTime.Now.Date,
                        FechaHasta = DateTime.Now.Date,
                        Empleado = empleado,
                        Observacion = "Sin Observación"
                    }

                };

                await CargarCombos();
                
                return View(solicitudPermisoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PedirPermiso(SolicitudPermisoViewModel solicitudPermisoViewModel)
        {
           
            try
            {

                if (!ModelState.IsValid) {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}";

                    await CargarCombos();
                    return View(solicitudPermisoViewModel);
                }
                
                if (solicitudPermisoViewModel.SolicitudPermiso.FechaDesde > solicitudPermisoViewModel.SolicitudPermiso.FechaHasta)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ErrorFechaDesdeHasta}|{"12000"}";

                    await CargarCombos();

                    return View(solicitudPermisoViewModel);
                }
                
                string fechaDesde = solicitudPermisoViewModel.SolicitudPermiso.FechaDesde.DayOfWeek.ToString();
                string fechaHasta = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta.DayOfWeek.ToString();

                
                // Diferencia en horas
                TimeSpan? diferenciaHoras = solicitudPermisoViewModel.SolicitudPermiso.HoraHasta - solicitudPermisoViewModel.SolicitudPermiso.HoraDesde;

                // Diferencia en dìas
                TimeSpan? tsDiferenciaDias = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta - solicitudPermisoViewModel.SolicitudPermiso.FechaDesde;
                
                var solicitudPermiso = new SolicitudPermiso
                {
                    IdEmpleado = solicitudPermisoViewModel.SolicitudPermiso.Empleado.IdEmpleado,
                    IdTipoPermiso = solicitudPermisoViewModel.SolicitudPermiso.IdTipoPermiso,

                    Motivo = solicitudPermisoViewModel.SolicitudPermiso.Motivo,
                    Observacion = solicitudPermisoViewModel.SolicitudPermiso.Observacion,
                    Estado = solicitudPermisoViewModel.SolicitudPermiso.Estado,

                    FechaSolicitud = DateTime.Now,
                    FechaDesde = solicitudPermisoViewModel.SolicitudPermiso.FechaDesde,
                    FechaHasta = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta,
                    HoraDesde = solicitudPermisoViewModel.SolicitudPermiso.HoraDesde,
                    HoraHasta = solicitudPermisoViewModel.SolicitudPermiso.HoraHasta,

                    CargoVacaciones = solicitudPermisoViewModel.SolicitudPermiso.CargoVacaciones

                };

                var response = await apiServicio.InsertarAsync(solicitudPermiso, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/InsertarSolicitudPermiso");

                if (response.IsSuccess)
                {
                    
                    return this.RedireccionarMensajeTime(
                            "SolicitudesPermisosAprobador",
                            "IndexEmpleado",
                            $"{Mensaje.Success}|{response.Message}|{"6000"}"
                    );
                    
                }

                await CargarCombos();

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"12000"}";

                return View(solicitudPermisoViewModel);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        
        


        public async Task<IActionResult> EditarPedirPermiso(string id)
        {
            try
            {
                await CargarCombos();
     
                SolicitudPermisoViewModel solicitudPermisoViewModel = await apiServicio.ObtenerElementoAsync1<SolicitudPermisoViewModel>(id, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ObtenerInformacionSolicitudPermiso");
                
                return View(solicitudPermisoViewModel);
            }
            catch (Exception ex)
            {
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

                    if (!ModelState.IsValid)
                    {
                        this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}";

                        await CargarCombos();
                        return View(solicitudPermisoViewModel);
                    }

                    if (solicitudPermisoViewModel.SolicitudPermiso.FechaDesde > solicitudPermisoViewModel.SolicitudPermiso.FechaHasta)
                    {
                        this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ErrorFechaDesdeHasta}|{"12000"}";

                        await CargarCombos();

                        return View(solicitudPermisoViewModel);
                    }

                    string fechaDesde = solicitudPermisoViewModel.SolicitudPermiso.FechaDesde.DayOfWeek.ToString();
                    string fechaHasta = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta.DayOfWeek.ToString();


                    // Diferencia en horas
                    TimeSpan? diferenciaHoras = solicitudPermisoViewModel.SolicitudPermiso.HoraHasta - solicitudPermisoViewModel.SolicitudPermiso.HoraDesde;

                    // Diferencia en dìas
                    TimeSpan? tsDiferenciaDias = solicitudPermisoViewModel.SolicitudPermiso.FechaHasta - solicitudPermisoViewModel.SolicitudPermiso.FechaDesde;

                    
                    
                    response = await apiServicio.EditarAsync(id, solicitudPermisoViewModel.SolicitudPermiso, new Uri(WebApp.BaseAddress),"api/SolicitudesPermisos");

                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                            "SolicitudesPermisosAprobador",
                            "IndexEmpleado",
                            $"{Mensaje.Success}|{response.Message}|{"6000"}"
                        );
                        
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"12000"}";
                    await CargarCombos();
                    return View(solicitudPermisoViewModel.SolicitudPermiso);

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
        public async Task<IActionResult> AprobarPermisos(SolicitudPermisoViewModel solicitudPermisoViewModel)
        {
            Response response = new Response();
            try
            {
                if (solicitudPermisoViewModel!=null)
                {

                    response = await apiServicio.EditarAsync<SolicitudPermisoViewModel>(solicitudPermisoViewModel, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/EditarEstadoSolicitudPermiso");


                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                            "SolicitudesPermisosAprobador",
                            "IndexJefe",
                            $"{Mensaje.Success}|{response.Message}|{"6000"}"
                        );
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"12000"}";

                    await CargarCombos();

                    return View(solicitudPermisoViewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }
        


        public async Task<IActionResult> AprobarPermisos(string id)
        {
            try
            {
                await CargarCombosJefe();

                SolicitudPermisoViewModel solicitudPermisoViewModel = await apiServicio.ObtenerElementoAsync1<SolicitudPermisoViewModel>(id, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ObtenerInformacionSolicitudPermiso");

                return View(solicitudPermisoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> IndexEmpleado()
        {

            var lista = new List<SolicitudPermisoViewModel>();

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    Empleado empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");

                    lista = await apiServicio.Listar<SolicitudPermisoViewModel>(empleado, new Uri(WebApp.BaseAddress)
                                  , "api/SolicitudesPermisos/ListarSolicitudesPermisosEmpleado");

                    return View(lista);
                }
                else {

                    return RedirectToAction("Login", "Login");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexJefe()
        {

            var lista = new List<SolicitudPermisoViewModel>();
            try
            {
                
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                Empleado empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");

                if (empleado.EsJefe == true)
                {
                    Dependencia dependencia = new Dependencia { IdDependencia = (int)empleado.IdDependencia };

                    lista = await apiServicio.Listar<SolicitudPermisoViewModel>(dependencia,
                        new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ListarSolicitudesPermisosPorDependenciaViewModel");

                    return View(lista);
                }
                else {

                    return this.RedireccionarMensajeTime(
                            "SolicitudesPermisosAprobador",
                            "IndexEmpleado",
                            $"{Mensaje.Informacion}|{Mensaje.AccesoNoAutorizado}|{"6000"}"
                    );

                }

                
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Visualizar(string id)
        {
            try
            {
                await CargarCombosJefe();

                SolicitudPermisoViewModel solicitudPermisoViewModel = await apiServicio.ObtenerElementoAsync1<SolicitudPermisoViewModel>(id, new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ObtenerInformacionSolicitudPermiso");

                return View(solicitudPermisoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> BorrarMiPermiso(int id) {

            try
            {
                var respuesta = await apiServicio.EliminarAsync(
                    id,
                    new Uri(WebApp.BaseAddress),
                    "api/SolicitudesPermisos/BorrarSolicitudPorId");

                if (respuesta.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                            "SolicitudesPermisosAprobador",
                            "IndexEmpleado",
                            $"{Mensaje.Success}|{respuesta.Message}|{"6000"}"
                         );
                }

                return this.RedireccionarMensajeTime(
                            "SolicitudesPermisosAprobador",
                            "IndexEmpleado",
                            $"{Mensaje.Error}|{respuesta.Message}|{"10000"}"
                         );

            }
            catch (Exception ex) {
                return BadRequest();
            }
        }


        public async Task CargarCombos() { 

            //** Estados de aprobación AprobacionMovimientoInternoViewModel
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ListarEstadosAprobacionEmpleado");

            ViewData["IdListaEstado"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


            //** Tipos de permisos **
            var listaTiposPermisos = await apiServicio.Listar<TipoPermiso>(
                new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso");

            ViewData["IdTipoPermiso"] = new SelectList(listaTiposPermisos, "IdTipoPermiso", "Nombre");
        }

        public async Task CargarCombosJefe()
        {

            //** Estados de aprobación AprobacionMovimientoInternoViewModel
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionAprobador");

            ViewData["IdListaEstado"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


            //** Tipos de permisos **
            var listaTiposPermisos = await apiServicio.Listar<TipoPermiso>(
                new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso");

            ViewData["IdTipoPermiso"] = new SelectList(listaTiposPermisos, "IdTipoPermiso", "Nombre");
        }

    }
}