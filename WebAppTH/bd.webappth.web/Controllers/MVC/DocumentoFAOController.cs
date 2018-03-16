using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class DocumentoFAOController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IApiServicio apiServicio;


        public DocumentoFAOController(IApiServicio apiServicio)
        {

            this.apiServicio = apiServicio;


        }
        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
        //        var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
        //        var lista = new List<DocumentoFAOViewModel>();

        //        lista = await apiServicio.Listar<DocumentoFAOViewModel>(NombreUsuario, new Uri(WebApp.BaseAddress)
        //                                                            , "api/Empleados/ListarEmpleados");
        //        return View(lista);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}
        public async Task<IActionResult> AsignarEmpleadoFAO()
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var lista = new List<DocumentoFAOViewModel>();

                var usuario = new DocumentoFAOViewModel
                {
                    NombreUsuario = NombreUsuario

                };
                lista = await apiServicio.Listar<DocumentoFAOViewModel>(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleadosSinFAO");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> Asignados()
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var lista = new List<DocumentoFAOViewModel>();

                var usuario = new DocumentoFAOViewModel
                {
                    NombreUsuario = NombreUsuario

                };
                lista = await apiServicio.Listar<DocumentoFAOViewModel>(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleadosConFAO");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> Detalle(int id, int IdActividad)
        {
            try
            {

                var usuario = new DocumentoFAOViewModel
                {
                    IdEmpleado = id,
                    IdFormularioAnalisisOcupacional = IdActividad

                };
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ObtenerEncabezadoEmpleadosFaoValidarConValidacionRH");

                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<DocumentoFAOViewModel>(response.Resultado.ToString());
                    return View(empleado);
                }
                ViewData["Error"] = response.Message;
                return View();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> EmpleadoFAOAsignar(int id)
        {

            if (!ModelState.IsValid)
            {

                return View(id);

            }
            Response response = new Response();
            var empleadoid = new FormularioAnalisisOcupacional
            {
                IdEmpleado = id,
                InternoMismoProceso = false,
                InternoOtroProceso = false,
                ExternosCiudadania = false,
                ExtPersJuridicasPubNivelNacional = false,
                FechaRegistro = DateTime.Now,
                Anio = DateTime.Now.Year,
                MisionPuesto = "Debe Introducir misión del puesto",
                Estado = -1

            };

            response = await apiServicio.InsertarAsync(empleadoid,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/FormularioAnalisisOcupacional/InsertarFormularioAnalisisOcupacional");
            if (response.IsSuccess)
            {

                return RedirectToAction("AsignarEmpleadoFAO");
            }

            //ViewData["Error"] = response.Message;
            return View();

        }
        public async Task<IActionResult> Create(int id, int IdActividad)
        {
            try
            {

                var usuario = new DocumentoFAOViewModel
                {
                    IdEmpleado = id,
                    IdFormularioAnalisisOcupacional = IdActividad

                };
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ObtenerEncabezadoEmpleadosFaoValidarConExepciones");

                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<DocumentoFAOViewModel>(response.Resultado.ToString());
                    return View(empleado);
                }
                ViewData["Error"] = response.Message;
                return View();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        public async Task<IActionResult> ValidarTalentoHumano(DocumentoFAOViewModel documentoFAOViewModel)
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var usuario = new DocumentoFAOViewModel
                {
                    NombreUsuario = NombreUsuario,
                    ListaRolPUesto = documentoFAOViewModel.ListaRolPUesto,
                    Descripcionpuesto = documentoFAOViewModel.Descripcionpuesto,
                    Cumple = documentoFAOViewModel.Cumple,
                    aplicapolitica = documentoFAOViewModel.aplicapolitica,
                    IdEmpleado = documentoFAOViewModel.IdEmpleado
                };
                
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/FormularioAnalisisOcupacional/InsertarAdministracionTalentoHumano");

                if (response.IsSuccess)
                {
                    
                    return RedirectToAction("Asignados");
                }

                ViewData["Error"] = response.Message;
                return View();

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
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/FormularioAnalisisOcupacional");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Asignados");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}