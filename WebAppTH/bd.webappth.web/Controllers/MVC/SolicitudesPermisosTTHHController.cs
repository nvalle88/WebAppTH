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
    public class SolicitudesPermisosTTHHController : Controller
    {
        private readonly IApiServicio apiServicio;


        public SolicitudesPermisosTTHHController(IApiServicio apiServicio)
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
                            "SolicitudesPermisosTTHH",
                            "IndexTTHH",
                            $"{Mensaje.Success}|{response.Message}|{"6000"}"
                        );
                    }

                    this.TempData["MensajeTime"] = $"{Mensaje.Error}|{response.Message}|{"12000"}";

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


        public async Task<IActionResult> Index()
        {

            var lista = new List<SolicitudPermisoViewModel>();
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                Empleado empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");

                Dependencia dependencia = new Dependencia { IdDependencia = (int) empleado.IdDependencia};

                lista = await apiServicio.Listar<SolicitudPermisoViewModel>(dependencia, 
                    new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ListarSolicitudesPermisosTTHHViewModel");
                
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexTTHH()
        {

            var lista = new List<SolicitudPermisoViewModel>();
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                Empleado empleado = await apiServicio.ObtenerElementoAsync1<Empleado>(NombreUsuario, new Uri(WebApp.BaseAddress), "api/Empleados/EmpleadoSegunNombreUsuario");

                Dependencia dependencia = new Dependencia { IdDependencia = (int)empleado.IdDependencia };

                lista = await apiServicio.Listar<SolicitudPermisoViewModel>(dependencia,
                    new Uri(WebApp.BaseAddress), "api/SolicitudesPermisos/ListarSolicitudesPermisosJefesTTHHViewModel");

                return View(lista);
            }
            catch (Exception ex)
            {
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
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacion");

            ViewData["IdListaEstado"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


            //** Tipos de permisos **
            var listaTiposPermisos = await apiServicio.Listar<TipoPermiso>(
                new Uri(WebApp.BaseAddress), "api/TiposPermiso/ListarTiposPermiso");

            ViewData["IdTipoPermiso"] = new SelectList(listaTiposPermisos, "IdTipoPermiso", "Nombre");
        }

    }
}