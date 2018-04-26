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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class EvaluacionDesempenoController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IApiServicio apiServicio;
        public EvaluacionDesempenoController(IApiServicio apiServicio)
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
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var lista = new List<ViewModelEvaluacionDesempeno>();

                var usuario = new ViewModelEvaluacionDesempeno
                {
                    NombreUsuario = NombreUsuario

                };

                lista = await apiServicio.Listar<ViewModelEvaluacionDesempeno>(usuario, new Uri(WebApp.BaseAddress)
                                                                       , "api/EvaluacionDesempeno/ListarEmpleadosJefes");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {

                throw;
            }
            //var lista = new List<ViewModelEvaluacionDesempeno>();
            //try
            //{
            //    lista = await apiServicio.Listar<ViewModelEvaluacionDesempeno>(new Uri(WebApp.BaseAddress)
            //                                                        , "api/EvaluacionDesempeno/ListarEmpleados");

            //    InicializarMensaje(null);
            //    return View(lista);

        }

        public async Task<IActionResult> Evaluar(int idempleado)
        {

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            var usuario = new ViewModelEvaluador
            {
                IdEmpleado = idempleado,
                NombreUsuario = NombreUsuario

            };
            try
            {
                var lista = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluador>(usuario, new Uri(WebApp.BaseAddress)
                                                                   , "api/EvaluacionDesempeno/Evaluar");


                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task Cargarcombos()
        {
            try
            {
                ViewData["IdSucursal"] = new SelectList(await apiServicio.Listar<Sucursal>(new Uri(WebApp.BaseAddress), "api/Sucursal/ListarSucursal"), "IdSucursal", "Nombre");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        [HttpPost]
        public async Task<JsonResult> CargarComboDependencia(string idsucursal)
        {
            var sucursal = new Sucursal
            {
                IdSucursal = Convert.ToInt32(idsucursal)
            };
            var listadependencias = await apiServicio.ObtenerElementoAsync1<List<Dependencia>>(sucursal, new Uri(WebApp.BaseAddress)
                                                                   , "/api/Dependencias/ListarDependenciaporSucursal");
            return Json(listadependencias);
        }
        [HttpPost]
        public async Task<JsonResult> CargarEmpleado(string idDependecia)
        {
            var sucursal = new Empleado
            {
                IdDependencia = Convert.ToInt32(idDependecia)
            };
            var listadependencias = await apiServicio.ObtenerElementoAsync1<List<Dependencia>>(sucursal, new Uri(WebApp.BaseAddress)
                                                                   , "/api/Dependencias/ListarDependenciaporSucursal");
            return Json(listadependencias);
        }

    }
}