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

namespace bd.webappth.web.Controllers.MVC
{
    public class GenerarFirmasController : Controller
    {
        private readonly IApiServicio apiServicio;


        public GenerarFirmasController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                var IdSucursal = 2;
                @ViewData["DependenciaActual"] = IdSucursal;

                return View();

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public async Task<IActionResult> SeleccionFirmas(int NumeroFirmas,int IdSucursal)
        {

            try {

                var filtro = new IdFiltrosViewModel { IdSucursal = IdSucursal };

                var lista = await apiServicio.ObtenerElementoAsync1<List<Dependencia>>(
                    filtro,
                    new Uri(WebApp.BaseAddress),
                    "api/GenerarFirmas/ObtenerDependenciasPorNumeroFirmas");

                ViewData["Dependencia"] = new SelectList(lista, "IdDependencia", "Nombre");
                ViewData["NumeroFirmas"] = NumeroFirmas;

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

        public async Task<IActionResult> EnviarDatosReporte(GenerarFirmasViewModel modelo)
        {
            return View();

        }


    }
}