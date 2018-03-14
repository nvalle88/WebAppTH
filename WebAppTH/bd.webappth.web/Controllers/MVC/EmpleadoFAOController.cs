using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class EmpleadoFAOController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public EmpleadoFAOController(IApiServicio apiServicio)
        {

            this.apiServicio = apiServicio;


        }
        
        public async Task<IActionResult> Create()
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                
                var usuario = new DocumentoFAOViewModel
                {
                    NombreUsuario = NombreUsuario

                };
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ObtenerEncabezadoEmpleadosFao");

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
        [HttpPost]
        public async Task<IActionResult> Create(DocumentoFAOViewModel documentoFAOViewModel)
        {
           
            if (!ModelState.IsValid)
            {

                return View(documentoFAOViewModel);

            }
            Response response = new Response();

            var empleadoid = new DocumentoFAOViewModel
            {


                IdEmpleado = documentoFAOViewModel.IdEmpleado,
                InternoMismoProceso = documentoFAOViewModel.InternoMismoProceso,
                InternoOtroProceso = documentoFAOViewModel.InternoOtroProceso,
                ExternosCiudadania = documentoFAOViewModel.ExternosCiudadania,
                ExtPersJuridicasPubNivelNacional = documentoFAOViewModel.ExtPersJuridicasPubNivelNacional,
                FechaRegistro = DateTime.Now,
                Anio = DateTime.Now.Year,
                Mision = documentoFAOViewModel.Mision,
                actividad1=documentoFAOViewModel.actividad1,
                actividad2 = documentoFAOViewModel.actividad2,
                actividad3 = documentoFAOViewModel.actividad3,
                actividad4 = documentoFAOViewModel.actividad4,
                actividad5 = documentoFAOViewModel.actividad5,
                actividad6 = documentoFAOViewModel.actividad6,
                actividad7 = documentoFAOViewModel.actividad7,
                actividad8 = documentoFAOViewModel.actividad8,
                actividad9 = documentoFAOViewModel.actividad9,
                actividad10 = documentoFAOViewModel.actividad10,

            };

            response = await apiServicio.InsertarAsync(documentoFAOViewModel,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/FormularioAnalisisOcupacional/ActualizarFormularioAnalisisOcupacional");
            if (response.IsSuccess)
            {

                //return RedirectToAction("AsignarEmpleadoFAO");
                return View();
            }

            //ViewData["Error"] = response.Message;
            return View();
        }
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
        public async Task<IActionResult> Detalle()
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
    }
}