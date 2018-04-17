using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class Singleton2
    {
        private static IndiceOcupacionalDetalle instance;

        private Singleton2() { }

        public static IndiceOcupacionalDetalle Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IndiceOcupacionalDetalle();
                    instance.ListaAreaConocimientos = new List<AreaConocimiento>();
                }
                return instance;
            }
        }
    }

    public class ManualPuestosIndicesOcupacionalesController : Controller
    {
        private readonly IApiServicio apiServicio;
        

        public ManualPuestosIndicesOcupacionalesController(IApiServicio apiServicio)
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
            var lista = new List<IndiceOcupacionalViewModel>();
            try
            {
                lista = await apiServicio.Listar<IndiceOcupacionalViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/IndicesOcupacionales/ListarIndicesOcupaciones");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        
        public async Task<IActionResult> ExperienciaLaboralRequerida(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var ListaExperienciaLaboralRequerida = await apiServicio.Listar<ExperienciaLaboralRequeridaViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/ExperienciaLaboralRequeridas/ListarExperienciaLaboralRequeridaPorIndiceOcupacional");

            return View(ListaExperienciaLaboralRequerida);
        }
        
        public async Task<IActionResult> ConocimientosAdicionales(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var ListaConocimientosAdicionales = await apiServicio.Listar<ConocimientosAdicionalesViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/ConocimientosAdicionales/ListarConocimientosAdicionalesPorIndiceOcupacional");

            return View(ListaConocimientosAdicionales);
        }

        public async Task<IActionResult> ComportamientoObservable(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var ListaEstudios = await apiServicio.Listar<ComportamientoObservableViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/ComportamientosObservables/ListarComportamientosObservablesPorIndiceOcupacional");

            return View(ListaEstudios);
        }

        public async Task<IActionResult> Estudios(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var ListaEstudios = await apiServicio.Listar<EstudioViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudiosPorIndiceOcupacional");

            return View(ListaEstudios);
        }

        public async Task<IActionResult> Capacitaciones(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var ListaActividadesEsenciales = await apiServicio.Listar<CapacitacionViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/Capacitaciones/ListarCapacitacionesPorIndiceOcupacional");

            return View(ListaActividadesEsenciales);
        }

        public async Task<IActionResult> AreaConocimiento(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var ListaActividadesEsenciales = await apiServicio.Listar<AreaConocimientoViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosPorIndiceOcupacional");

            return View(ListaActividadesEsenciales);
        }

        public async Task<IActionResult> ActividadesEsenciales(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var ListaActividadesEsenciales = await apiServicio.Listar<ActividadesEsencialesViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/ActividadesEsenciales/ListarActividadesEsencialesPorIndiceOcupacional");

            return View(ListaActividadesEsenciales);
        }

        public async Task<IActionResult> Detalle(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = id,
            };

            var indiceOcupacionalDetalle = await apiServicio.ObtenerElementoAsync1<IndiceOcupacionalViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/IndicesOcupacionales/InformacionBasicaIndiceOcupacional");

            return View(indiceOcupacionalDetalle);
        }

        public async Task<IActionResult> Detalles(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional=id,
            };

            var IndiOcupacionalDetalle = new IndiceOcupacionalDetalle
            {
                IndiceOcupacional=indiceOcupacional,
            };

            var indiceOcupacionalDetalle = await apiServicio.ObtenerElementoAsync1<IndiceOcupacionalDetalle>(IndiOcupacionalDetalle, new Uri(WebApp.BaseAddress), "api/IndicesOcupacionales/DetalleIndiceOcupacional");

            return View(indiceOcupacionalDetalle);
        }


    }
}