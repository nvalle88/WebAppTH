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
        public async Task<IActionResult> Index()
        {

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var lista = new List<DocumentoFAOViewModel>();
            try
            {
                lista = await apiServicio.Listar<DocumentoFAOViewModel>(NombreUsuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleados");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> AsignarEmpleadoFAO()
        {

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            var lista = new List<DocumentoFAOViewModel>();
            try
            {
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
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            var lista = new List<DocumentoFAOViewModel>();
            try
            {
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
                ExtPersJurídicasPubNivelNacional = false,
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

            ViewData["Error"] = response.Message;
            return View(id);



        }
    }
}