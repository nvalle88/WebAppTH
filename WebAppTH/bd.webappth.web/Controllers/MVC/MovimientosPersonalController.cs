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
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class MovimientosPersonalController : Controller
    {
        private readonly IApiServicio apiServicio;


        public MovimientosPersonalController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        


        public async Task<IActionResult> Index()
        {

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var modeloEnviar = new AccionesPersonalPorEmpleadoViewModel
                    {
                        NombreUsuarioActual = NombreUsuario
                    };


                    var modelo = await apiServicio.Listar<AccionPersonalViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/ListarEmpleadosConAccionPersonal");

                    return View(modelo);

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        //public async Task<IActionResult> Edit(int id)
        //{
        //    try
        //    {
        //        var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

        //        if (claim.IsAuthenticated == true)
        //        {

        //            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

        //            var modelo = new AccionPersonalViewModel { IdAccionPersonal = id, NombreUsuarioAprobador = NombreUsuario };

        //            var respuesta = await apiServicio.ObtenerElementoAsync<AccionPersonalViewModel>(
        //                modelo,
        //                new Uri(WebApp.BaseAddress),
        //                "api/AccionesPersonal/ObtenerAccionPersonalViewModel");

        //            if (respuesta.IsSuccess)
        //            {
        //                modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());

        //                await InicializarCombos();


        //                var situacionActualViewModel = new SituacionActualEmpleadoViewModel { IdEmpleado = modelo.DatosBasicosEmpleadoViewModel.IdEmpleado };

        //                var situacionActualEmpleadoViewModelResponse = await apiServicio.ObtenerElementoAsync<SituacionActualEmpleadoViewModel>(situacionActualViewModel, new Uri(WebApp.BaseAddress),
        //                "api/Empleados/ObtenerSituacionActualEmpleadoViewModel");

        //                if (respuesta.IsSuccess)
        //                {
        //                    situacionActualViewModel = JsonConvert.DeserializeObject<SituacionActualEmpleadoViewModel>(situacionActualEmpleadoViewModelResponse.Resultado.ToString());
        //                }

        //                modelo.SituacionActualEmpleadoViewModel = situacionActualViewModel;


        //                var listaIOMP = await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(
        //                new Uri(WebApp.BaseAddress),
        //                "api/IndicesOcupacionalesModalidadPartida/ListarIndicesOcupacionalesModalidadPartidaViewModel");

        //                modelo.ListaIndicesOcupacionalesModalidadPartida = listaIOMP;


        //                return View(modelo);

        //            }

        //            return BadRequest();
        //        }
        //        else {
        //            return RedirectToAction("Login", "Login");

        //        }
                

        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //        throw;
        //    }

        //}

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccionPersonalViewModel accionPersonalViewModel)
        {
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                    var modeloEnviar = new AccionPersonal
                    {
                        IdAccionPersonal = accionPersonalViewModel.IdAccionPersonal,
                        IdEmpleado = accionPersonalViewModel.DatosBasicosEmpleadoViewModel.IdEmpleado,

                        Estado = accionPersonalViewModel.Estado,
                        NombreUsuario = NombreUsuario

                    };

                    var respuesta = await apiServicio.EditarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/EditarAccionPersonal");

                    if (respuesta.IsSuccess)
                    {

                        return this.RedireccionarMensajeTime(
                                "MovimientosPersonal",
                                "Index",
                                 new { identificacion = respuesta.Resultado },
                                $"{Mensaje.Success}|{respuesta.Message}|{"10000"}"
                             );
                    }

                    return View(accionPersonalViewModel);

                }
                else {
                    return RedirectToAction("Login", "Login");

                }


            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        */

        public async Task InicializarCombos()
        {
            // Carga de listas para combos

            // ** Tipos de acciones
            var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

            ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");


            //** Estados de aprobación
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacion");

            ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");

        }

    }
}