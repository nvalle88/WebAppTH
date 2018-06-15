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
    public class GenerarFirmasController : Controller
    {
        private readonly IApiServicio apiServicio;


        public GenerarFirmasController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> AgregarPiePagina(string NombreReporteConParametros)
        {

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    // Obtención de la sucursal actual del usuario
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var filtro = new IdFiltrosViewModel { NombreUsuario = NombreUsuario};

                    var sucursal = await apiServicio.ObtenerElementoAsync1<Sucursal>(
                    filtro,
                    new Uri(WebApp.BaseAddress),
                    "api/Sucursal/ObtenerSucursalPorEmpleado");
                    
                    
                    // Redireccionar a selección de firmas con la IdSucursal actual
                    return RedirectToAction("SeleccionFirmas",
                        new {
                            IdSucursal = sucursal.IdSucursal,
                            UrlReporte = NombreReporteConParametros
                        });

                }

                return RedirectToAction("Login", "Login");
                

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public async Task<IActionResult> SeleccionFirmas(int IdSucursal,string UrlReporte)
        {

            try {

                int NumeroFirmas = 5;

                var filtro = new IdFiltrosViewModel { IdSucursal = IdSucursal };

                var lista = await apiServicio.ObtenerElementoAsync1<List<Dependencia>>(
                    filtro,
                    new Uri(WebApp.BaseAddress),
                    "api/GenerarFirmas/ObtenerDependenciasPorNumeroFirmas");

                ViewData["Dependencia"] = new SelectList(lista, "IdDependencia", "Nombre");
                ViewData["NumeroFirmas"] = NumeroFirmas;
                ViewData["UrlReporte"] = UrlReporte;

                return View();

            } catch (Exception ex)
            {
                return View();
            }

        }

        public async Task<IActionResult> ObtenerEmpleadosPorDependencia(int IdDependencia) {

            var lista = new List<IndiceOcupacionalModalidadPartida>();

            try
            {
                var filtro = new IdFiltrosViewModel { IdDependencia = IdDependencia };

                lista = await apiServicio.ObtenerElementoAsync1<List<IndiceOcupacionalModalidadPartida>>(
                    filtro,
                    new Uri(WebApp.BaseAddress),
                    "api/GenerarFirmas/ObtenerEmpleadosPorDependencias");

                return Json(lista);
            }
            catch (Exception)
            {

                return Json(lista);
            }

        }

        

    }
}