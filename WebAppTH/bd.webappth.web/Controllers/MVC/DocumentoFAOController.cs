using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var lista = new List<DocumentoFAOViewModel>();
            try
            {
                lista = await apiServicio.Listar<DocumentoFAOViewModel>(new Uri(WebApp.BaseAddress)
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
            var lista = new List<DocumentoFAOViewModel>();
            try
            {
                lista = await apiServicio.Listar<DocumentoFAOViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleados");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> Detalle()
        {


            //var indiceOcupacionalDetalle = await apiServicio.ObtenerElementoAsync1<IndiceOcupacionalViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/IndicesOcupacionales/InformacionBasicaIndiceOcupacional");

            //return View(indiceOcupacionalDetalle);
            return View();
        }
    }
}