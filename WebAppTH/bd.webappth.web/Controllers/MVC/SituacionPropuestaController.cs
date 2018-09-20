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
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;
using bd.webappth.entidades.ViewModels;
using System.Security.Claims;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SituacionPropuestaController : Controller
    {

        private readonly IApiServicio apiServicio;


        public SituacionPropuestaController(IApiServicio apiServicio)
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



        public async Task<IActionResult> Index()
        {

            var modelo = new RequerimientoRolPorDependenciaViewModel();
            modelo.RolesNivelJerarquicoSuperior = new RequerimientoRolPorGrupoOcupacionalViewModel();
            modelo.RolesNivelOperativo = new RequerimientoRolPorGrupoOcupacionalViewModel();
            modelo.RolesNivelJerarquicoSuperior.ListaRolesRequeridos = new List<RequerimientoRolViewModel>();
            modelo.RolesNivelOperativo.ListaRolesRequeridos = new List<RequerimientoRolViewModel>();

            modelo.IdDependencia = 0;


            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true) {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                    

                    modelo.NombreUsuario = NombreUsuario;


                    // Obtención de datos para generar pantalla
                    var respuesta = await apiServicio.ObtenerElementoAsync<RequerimientoRolPorDependenciaViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/SituacionPropuesta/ObtenerRequerimientoRolPorDependencia");

                    if (respuesta.IsSuccess)
                    {
                        respuesta.Resultado = JsonConvert.DeserializeObject<RequerimientoRolPorDependenciaViewModel>(respuesta.Resultado.ToString());

                        return View(respuesta.Resultado);
                    }
                    else
                    {
                        return RedirectToAction("AutorizacionError", "SituacionPropuesta", new { mensaje = respuesta.Message });
                    }
                    

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();

            }
        }

        public async Task<IActionResult> AutorizacionError(string mensaje)
        {
            InicializarMensaje(mensaje);
            return View();
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index"); 
        }
        */

        public async Task<IActionResult> CrearRolesNivelSuperior(string mensaje, string id)
        {
            var modelo = new RequerimientoRolPorDependenciaViewModel();
            modelo.IdDependencia = Convert.ToInt32(id);

            InicializarMensaje(mensaje);

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                    modelo.NombreUsuario = NombreUsuario;


                    // Obtención de datos para generar pantalla
                    var respuesta = await apiServicio.ObtenerElementoAsync<RequerimientoRolPorDependenciaViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/SituacionPropuesta/CrearRequerimientoRolPorDependencia");

                    if (respuesta.IsSuccess)
                    {
                        respuesta.Resultado = respuesta.Resultado = JsonConvert.DeserializeObject<RequerimientoRolPorDependenciaViewModel>(respuesta.Resultado.ToString()); ;

                        return View(respuesta.Resultado);
                    }
                    else
                    {
                        return RedirectToAction("AutorizacionError", "SituacionPropuesta", new { mensaje = respuesta.Message });
                    }

                }

                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "SituacionPropuesta", new { mensaje = Mensaje.ErrorServicio });
            }

        }

        public async Task<IActionResult> CrearRolesNivelOperativo(string mensaje, string id)
        {
            var modelo = new RequerimientoRolPorDependenciaViewModel();

            modelo.IdDependencia = Convert.ToInt32(id);
            InicializarMensaje(mensaje);
            

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                    modelo.NombreUsuario = NombreUsuario;


                    // Obtención de datos para generar pantalla
                    var respuesta = await apiServicio.ObtenerElementoAsync<RequerimientoRolPorDependenciaViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/SituacionPropuesta/CrearRequerimientoRolPorDependencia");

                    if (respuesta.IsSuccess)
                    {
                        respuesta.Resultado = respuesta.Resultado = JsonConvert.DeserializeObject<RequerimientoRolPorDependenciaViewModel>(respuesta.Resultado.ToString()); ;

                        return View(respuesta.Resultado);
                    }
                    else
                    {
                        return RedirectToAction("AutorizacionError", "SituacionPropuesta", new { mensaje = respuesta.Message });
                    }

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "SituacionPropuesta", new { mensaje = Mensaje.ErrorServicio });
            }

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearRolesNivelSuperior( RequerimientoRolPorGrupoOcupacionalViewModel modelo, int id, string nombre)
        {
            var requerimientoRolPorDependenciaViewModel = new RequerimientoRolPorDependenciaViewModel();
            requerimientoRolPorDependenciaViewModel.IdDependencia = id;
            requerimientoRolPorDependenciaViewModel.NombreDependencia = nombre;

            requerimientoRolPorDependenciaViewModel.RolesNivelJerarquicoSuperior = modelo;
            InicializarMensaje(null);

            try
            {
                if (!ModelState.IsValid)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"10000"}";
                    return View(requerimientoRolPorDependenciaViewModel);
                }
                
                Response response = await apiServicio.InsertarAsync<RequerimientoRolPorDependenciaViewModel>(requerimientoRolPorDependenciaViewModel, new Uri(WebApp.BaseAddress), "api/SituacionPropuesta/InsertarNivelesJerarquicos");

                if (response.IsSuccess) {

                    return this.RedireccionarMensajeTime(
                        "SituacionPropuesta",
                        "Index",
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );

                }
                


                return View(requerimientoRolPorDependenciaViewModel);

            } catch (Exception ex) {
                return View(requerimientoRolPorDependenciaViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearRolesNivelOperativo(RequerimientoRolPorGrupoOcupacionalViewModel modelo, int id, string nombre)
        {
            var requerimientoRolPorDependenciaViewModel = new RequerimientoRolPorDependenciaViewModel();
            requerimientoRolPorDependenciaViewModel.IdDependencia = id;
            requerimientoRolPorDependenciaViewModel.NombreDependencia = nombre;

            requerimientoRolPorDependenciaViewModel.RolesNivelOperativo = modelo;
            InicializarMensaje(null);

            try
            {
                if (!ModelState.IsValid)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"10000"}";

                }

                Response response = await apiServicio.InsertarAsync<RequerimientoRolPorDependenciaViewModel>(requerimientoRolPorDependenciaViewModel, new Uri(WebApp.BaseAddress), "api/SituacionPropuesta/InsertarNivelesOperativos");

                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                        "SituacionPropuesta",
                        "Index",
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                }



                return View(requerimientoRolPorDependenciaViewModel);

            }
            catch (Exception ex)
            {
                return View(requerimientoRolPorDependenciaViewModel);
            }
        }


        
        public async Task<IActionResult> Delete(int idDependencia, int idRol)
        {
            var modelo = new RequerimientoRolPorDependenciaViewModel();
            modelo.IdDependencia = idDependencia;

            var objeto = new RequerimientoRolPorGrupoOcupacionalViewModel();

            objeto.ListaRolesRequeridos = new List<RequerimientoRolViewModel>();
            objeto.ListaRolesRequeridos.Add(
                new RequerimientoRolViewModel
                {
                    IdRolPuesto = idRol
                }
            );

            modelo.RolesNivelJerarquicoSuperior = objeto;


            try
            {
                Response response = await apiServicio.EliminarAsync(modelo, new Uri(WebApp.BaseAddress), "api/SituacionPropuesta/EliminarRolPorDependencia");
                
                return this.RedireccionarMensajeTime(
                    "SituacionPropuesta",
                    "Index",
                    $"{Mensaje.Success}|{response.Message}|{"7000"}"
                );

            } catch (Exception ex) {

                return this.RedireccionarMensajeTime(
                    "SituacionPropuesta",
                    "Index",
                    $"{Mensaje.Success}|{Mensaje.BorradoNoSatisfactorio}|{"7000"}"
                );
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarProceso( int id)
        {

            try {
                var modelo = new RequerimientoRolPorDependenciaViewModel();
                modelo.IdDependencia = id;

                var respuesta = await apiServicio.ObtenerElementoAsync<RequerimientoRolPorDependenciaViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/SituacionPropuesta/CerrarProcesoRequerimientoPersona");


                if( respuesta.IsSuccess) {
                    return RedirectToAction("AutorizacionError", "SituacionPropuesta", new { mensaje = respuesta.Message });
                    
                }
                
                return this.RedireccionarMensajeTime(
                    "SituacionPropuesta",
                    "Index",
                    $"{Mensaje.Success}|{respuesta.Message}|{"7000"}"
                );

            }
            catch(Exception ex){
                return BadRequest();
            }

            
        }



    }
}