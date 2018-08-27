using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using System.Security.Claims;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.webappth.entidades.Enumeradores;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudVacacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitudVacacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }



        public async Task<ActionResult> ListadoEmpleadosSolicitudVacaciones()
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == false) {
                    return RedirectToAction("Login", "Login");
                }

                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var listadoEmpleados = await ListarEmpleadosPertenecientesaDependenciaconVacaciones(NombreUsuario);

                ViewData["IdPersona"] = new SelectList(await apiServicio.Listar<Persona>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeJefe"), "IdPersona", "Nombres");

                return View(listadoEmpleados);

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> AprobacionSolicitudVacacion(int id)
        {
            try
            {
                if (id.ToString() != null)
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "api/SolicitudVacaciones");


                    var a = JsonConvert.DeserializeObject<SolicitudVacaciones>(respuesta.Resultado.ToString());
                    var solicitudVacaciones = new SolicitudVacaciones
                    {
                        IdEmpleado = a.IdEmpleado,
                    };

                    if (respuesta.IsSuccess)
                    {
                        var empleadoEnviar = new Empleado
                        {
                            IdEmpleado = a.IdEmpleado,
                        };
                        var empleado = await apiServicio.ObtenerElementoAsync1<EmpleadoSolicitudViewModel>(empleadoEnviar, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDatosEmpleadoSeleccionado");
                        ViewData["FechaDesde"] = a.FechaDesde;
                        ViewData["FechaHasta"] = a.FechaHasta;
                        ViewData["PlanAnual"] = a.PlanAnual;
                        ViewData["FechaSolicitud"] = a.FechaSolicitud;
                        ViewData["NombresApellidos"] = empleado.NombreApellido;
                        ViewData["Identificacion"] = empleado.Identificacion;

                        await CargarCombos();

                        return View(a);
                    }

                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<Empleado> ObtenerEmpleadoLogueado(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.ObtenerElementoAsync1<Empleado>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerEmpleadoLogueado");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new Empleado();
            }

        }

        public async Task<List<EmpleadoSolicitudViewModel>> ListarEmpleadosPertenecientesaDependenciaconVacaciones(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.Listar<EmpleadoSolicitudViewModel>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeJefeconSolucitudesVacaciones");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new List<EmpleadoSolicitudViewModel>();
            }

        }




        public async Task<IActionResult> DetalleSolicitudVacaciones(int id)
        {
            var empleado = new Empleado()
            {
                IdEmpleado = id
            };

            var lista = new List<SolicitudVacaciones>();
            try
            {
                lista = await apiServicio.Listar<SolicitudVacaciones>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/SolicitudVacaciones/ListarSolicitudesVacaciones");

                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SolicitudVacaciones solicitudVacaciones)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    solicitudVacaciones.FechaSolicitud = DateTime.Now;
                    response = await apiServicio.EditarAsync(id, solicitudVacaciones, new Uri(WebApp.BaseAddress),
                                                                 "api/SolicitudVacaciones");

                    if (response.IsSuccess)
                    {

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    //ViewData["IdBrigadaSSO"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<BrigadaSSO>(new Uri(WebApp.BaseAddress), "api/BrigadasSSO/ListarBrigadasSSO"), "IdBrigadaSSO", "Nombre");
                    return View(solicitudVacaciones);

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
        public async Task<IActionResult> AprobarSolicitudVacaciones(string id, SolicitudVacaciones solicitudVacaciones)
        {
            //var dias = (solicitudVacaciones.FechaHasta.Date - solicitudVacaciones.FechaDesde.Date).Days;
            Response response = new Response();
            try
            {
                if (
                    solicitudVacaciones.RequiereReemplazo == true 
                    && solicitudVacaciones.IdEmpleadoReemplazo < 1
                    )
                {
                    return this.Redireccionar(
                            "SolicitudVacaciones",
                            "AprobacionSolicitudVacacion",
                            new { id = solicitudVacaciones.IdSolicitudVacaciones},
                            $"{Mensaje.Error}|{Mensaje.EscogerEmpleadoReemplazo}"
                         );
                }

                solicitudVacaciones.FechaRespuesta = DateTime.Now;
                response = await apiServicio.EditarAsync(solicitudVacaciones.IdEmpleado.ToString(), solicitudVacaciones, new Uri(WebApp.BaseAddress),"api/SolicitudVacaciones");

                if (response.IsSuccess)
                {
                    return this.Redireccionar(
                            "SolicitudVacaciones",
                            "DetalleSolicitudVacaciones",
                            new { id = solicitudVacaciones.IdEmpleado },
                            $"{Mensaje.Success}|{response.Message}"
                         );
                    
                }
                
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task CargarCombos()
        {
            var listaEmpleados = new List<DatosBasicosEmpleadoViewModel>();

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

            if (claim.IsAuthenticated == true)
            {
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var enviar = new IdFiltrosViewModel { NombreUsuario = NombreUsuario };

                listaEmpleados = await apiServicio.Listar<DatosBasicosEmpleadoViewModel>(enviar, new Uri(WebApp.BaseAddress), "api/Empleados/ListarMisEmpleados");

                foreach (var item in listaEmpleados)
                {
                    item.Nombres = item.Nombres + " " + item.Apellidos;
                }
            }

            ViewData["EmpleadoReemplazo"] = new SelectList(listaEmpleados, "IdEmpleado", "Nombres");

        }

        public async Task<IActionResult> ReporteSolicitudVacacionesAprobadas()
        {

            return RedirectToAction("AgregarPiePagina", "GenerarFirmas", new { NombreReporteConParametros = "RepSolicitudVacacion" });
           
        }

    }
}