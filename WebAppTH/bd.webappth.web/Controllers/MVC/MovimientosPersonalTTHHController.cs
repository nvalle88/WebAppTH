using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace bd.webappth.web.Controllers.MVC
{
    public class MovimientosPersonalTTHHController : Controller
    {
        private readonly IApiServicio apiServicio;


        public MovimientosPersonalTTHHController(IApiServicio apiServicio)
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

        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);

            return View();

        }

        public async Task<IActionResult> ListaMovimientos(string identificacion,string mensaje,int num)
        {
            if ( string.IsNullOrEmpty(identificacion) ) {
                return RedirectToAction("Index");
            }
            return await ListaMovimientos(mensaje, identificacion + " ");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaMovimientos(string mensaje, string identificacion)
        {
            InicializarMensaje(mensaje);

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var modeloEnviar = new AccionesPersonalPorEmpleadoViewModel
                    {

                        DatosBasicosEmpleadoViewModel = new DatosBasicosEmpleadoViewModel
                        {
                            Identificacion = identificacion
                        },

                        NombreUsuarioActual = NombreUsuario
                    };


                    var modelo = await apiServicio.ObtenerElementoAsync1<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/ListarAccionesPersonalPorEmpleado");

                    if (modelo.DatosBasicosEmpleadoViewModel.IdEmpleado == 0) {
                        return RedirectToAction("Index", new { mensaje = Mensaje.RegistroNoEncontrado});
                    }

                    return View("ListaMovimientos", modelo);

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }



        public async Task<IActionResult> Create(string mensaje, int id)
        {
            try
            {
                InicializarMensaje(mensaje);

                var datosBasicosEmpleado = new DatosBasicosEmpleadoViewModel { IdEmpleado = id };

                var respuesta = await apiServicio.ObtenerElementoAsync <DatosBasicosEmpleadoViewModel>(
                    datosBasicosEmpleado,
                    new Uri(WebApp.BaseAddress),
                    "api/Empleados/ObtenerDatosBasicosEmpleado");

                if (respuesta.IsSuccess)
                {
                    datosBasicosEmpleado = JsonConvert.DeserializeObject<DatosBasicosEmpleadoViewModel>(respuesta.Resultado.ToString());

                    var model = new AccionPersonalViewModel
                    {
                        DatosBasicosEmpleadoViewModel = datosBasicosEmpleado,
                        Numero = "0"
                    };

                    var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

                    ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");



                    var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionTTHH");

                    ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


                    return View(model);

                }

                return BadRequest();
                
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccionPersonalViewModel accionPersonalViewModel)
        {
            try {

                var modeloEnviar = new AccionPersonal{
                    IdEmpleado = accionPersonalViewModel.DatosBasicosEmpleadoViewModel.IdEmpleado,
                    Fecha = accionPersonalViewModel.Fecha,
                    FechaRige = accionPersonalViewModel.FechaRige,
                    FechaRigeHasta = accionPersonalViewModel.FechaRigeHasta,
                    Estado = accionPersonalViewModel.Estado,
                    Explicacion = accionPersonalViewModel.Explicacion,
                    NoDias = accionPersonalViewModel.NoDias,
                    Numero = accionPersonalViewModel.Numero,
                    Solicitud = accionPersonalViewModel.Solicitud,
                    IdTipoAccionPersonal = accionPersonalViewModel.TipoAccionPersonalViewModel.IdTipoAccionPersonal
                    
                };

                var respuesta = await apiServicio.InsertarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/InsertarAccionPersonal");

                //return await ListaMovimientos(respuesta.Message,respuesta.Resultado+" ");
                return RedirectToAction("ListaMovimientos", new { identificacion = respuesta.Resultado , mensaje = respuesta.Message});

            } catch (Exception){
                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(string mensaje, int id)
        {
            try
            {
                InicializarMensaje(mensaje);

                var modelo = new AccionPersonalViewModel { IdAccionPersonal = id };

                var respuesta = await apiServicio.ObtenerElementoAsync<AccionPersonalViewModel>(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerAccionPersonalViewModel");

                if (respuesta.IsSuccess)
                {
                    modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());
                    

                    var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

                    ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");



                    var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionTTHH");

                    ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


                    return View(modelo);

                }

                return BadRequest();

            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccionPersonalViewModel accionPersonalViewModel)
        {
            try
            {

                var modeloEnviar = new AccionPersonal
                {
                    IdAccionPersonal = accionPersonalViewModel.IdAccionPersonal,
                    IdEmpleado = accionPersonalViewModel.DatosBasicosEmpleadoViewModel.IdEmpleado,
                    Fecha = accionPersonalViewModel.Fecha,
                    FechaRige = accionPersonalViewModel.FechaRige,
                    FechaRigeHasta = accionPersonalViewModel.FechaRigeHasta,
                    Estado = accionPersonalViewModel.Estado,
                    Explicacion = accionPersonalViewModel.Explicacion,
                    NoDias = accionPersonalViewModel.NoDias,
                    Numero = accionPersonalViewModel.Numero,
                    Solicitud = accionPersonalViewModel.Solicitud,
                    IdTipoAccionPersonal = accionPersonalViewModel.TipoAccionPersonalViewModel.IdTipoAccionPersonal

                };

                var respuesta = await apiServicio.InsertarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/EditarAccionPersonalTTHH");

                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("ListaMovimientos", new { identificacion = respuesta.Resultado, mensaje = respuesta.Message });
                }

                return View(accionPersonalViewModel);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}