using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bd.webappth.entidades.Constantes;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class QuejasController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IApiServicio apiServicio;
        public QuejasController(IApiServicio apiServicio)
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
                var lista = new List<ViewModelEvaluacionDesempeno>();
                lista = await apiServicio.Listar<ViewModelEvaluacionDesempeno>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Quejas/ListarEmpleados");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                InicializarMensaje(null);
                return View();
            }

        }
        public async Task<IActionResult> IndexQuejas(int idEmpleado, int ideval)
        {
            try
            {
                if (idEmpleado == 0 || ideval==0)
                {
                    idEmpleado= Convert.ToInt32( HttpContext.Session.GetInt32(Constantes.idEmpleadoSession));
                    ideval= Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idEval011Session));
                }
                if (idEmpleado != 0 || ideval != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEmpleadoSession, Convert.ToInt32(idEmpleado));
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, Convert.ToInt32(ideval));
                    //var lista = new List<ViewModelQuejas>();
                    var usuario = new ViewModelQuejas
                    {
                        IdEmpleado = Convert.ToInt32(idEmpleado),
                        IdEval001 = Convert.ToInt32(ideval)

                    };
                    var lista = await apiServicio.Listar<ViewModelQuejas>(usuario, new Uri(WebApp.BaseAddress)
                                                                       , "api/Quejas/ListaQuejas");
                    InicializarMensaje(null);
                    return View(lista);
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                InicializarMensaje(null);
                return View();
            }

        }
        public async Task<IActionResult> Create()
        {
            try
            {
                InicializarMensaje(null);
                return View();
            }
            catch (Exception ex)
            {
                InicializarMensaje(null);
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create(ViewModelQuejas viewModelQuejas)
        {
            try
            {
                var envio = new ViewModelQuejas()
                {
                    IdEval001 = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idEval011Session)),
                    IdEmpleado = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idEmpleadoSession)),
                    NombreCiudadano = viewModelQuejas.NombreCiudadano,
                    ApellidoCiudadano =viewModelQuejas.ApellidoCiudadano,
                    Descripcion = viewModelQuejas.Descripcion,
                    NumeroFormulario = viewModelQuejas.NumeroFormulario,
                    AplicaDescuento = viewModelQuejas.AplicaDescuento

                };
                if (envio.IdEval001 != 0 || envio.IdEmpleado !=0)
                {
                    Response response = new Response();
                    response = await apiServicio.InsertarAsync<Response>(envio, new Uri(WebApp.BaseAddress)
                                                                           , "api/Quejas/InsertarQuejas");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexQuejas");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }


        }
                
        public async Task<IActionResult> CalificacionFinal()
        {
            var envio = new ViewModelQuejas()
            {
                IdEval001 = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idEval011Session))

            };
            if (envio.IdEval001 != 0)
            {
                var ListaObservacion = await apiServicio.ObtenerElementoAsync1<ViewModelQuejas>(envio, new Uri(WebApp.BaseAddress), "api/Quejas/CalcularTotales");
                if (ListaObservacion != null)
                {
                    InicializarMensaje(null);
                    return View(ListaObservacion);
                }
                InicializarMensaje(null);
                return View();
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/Quejas");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexQuejas");
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