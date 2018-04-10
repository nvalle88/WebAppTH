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

namespace bd.webappth.web.Controllers.MVC
{
    public class ReportesProcesosBrechasController : Controller
    {

        private readonly IApiServicio apiServicio;


        public ReportesProcesosBrechasController(IApiServicio apiServicio)
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



        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);

            List<Dependencia> lista = new List<Dependencia>();
            
            try
            {
                lista = await apiServicio.Listar<Dependencia>(new Uri(WebApp.BaseAddress)
                                                                  , "api/ReportesProcesosBrechas/ListarDependencias");



                return View(lista);


            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;

                return View(lista);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index"); 
        }


            public async Task<IActionResult> CrearBrechasParaDependencia(string mensaje, string id)
        {
            var brechasPorDependencias = new BrechasPorDependenciaViewModel();

            brechasPorDependencias.IdDependencia = Convert.ToInt32(id);

            InicializarMensaje(mensaje);

            try
            {

                brechasPorDependencias = await apiServicio.ObtenerElementoAsync1<BrechasPorDependenciaViewModel>(brechasPorDependencias,new Uri(WebApp.BaseAddress)
                                                                  , "api/ReportesProcesosBrechas/CrearRolesParaAsignarBrechas");

                return View(brechasPorDependencias);

            } catch (Exception ex) {

                return View(brechasPorDependencias);
            }
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearBrechasParaDependencia( BrechasPorDependenciaViewModel modelo)
        {


            try
            {
                Response response = await apiServicio.InsertarAsync<BrechasPorDependenciaViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/ReportesProcesosBrechas/InsertarBrechasParaDependencia");

                if (response.IsSuccess) {
                    return RedirectToAction("Index", "ReportesProcesosBrechas",new { mensaje = response.Message});
                }

                return View("CrearBrechasParaDependencia", modelo);

            } catch (Exception ex) {
                return View(modelo);
            }
        }



    }
}