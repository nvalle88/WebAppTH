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
                InicializarMensaje(null);
                return View();
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
        public async Task<IActionResult> InformacionGeneral(int idempleado)
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var usuario = new ViewModelEvaluador
                {
                    IdEmpleado = idempleado,
                    NombreUsuario = NombreUsuario

                };
                var lista = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluador>(usuario, new Uri(WebApp.BaseAddress)
                                                                   , "api/EvaluacionDesempeno/Evaluar");
                InicializarMensaje(null);
                HttpContext.Session.SetInt32(Constantes.idEmpleadoSession, lista.IdEmpleado);
                HttpContext.Session.SetInt32(Constantes.idIndiceOcupacionalSession, lista.IdIndiceOcupacional);
                HttpContext.Session.SetInt32(Constantes.idEvaluadorSession, lista.IdJefe);
                return View(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> InformacionGeneral(ViewModelEvaluador viewModelEvaluador)
        {
            Response response = new Response();
            response = await apiServicio.InsertarAsync<Response>(viewModelEvaluador, new Uri(WebApp.BaseAddress)
                                                                   , "api/EvaluacionDesempeno/InsertarEval001");
            //response.Resultado
            if (response.IsSuccess)
            {
                 var a = JsonConvert.DeserializeObject<int>(response.Resultado.ToString());
                HttpContext.Session.SetInt32(Constantes.idEval011Session, a);
                return RedirectToAction("ActividadesEnsenciales");
            }
            ViewData["Error"] = response.Message;
            return View(viewModelEvaluador);


        }
        public async Task<IActionResult> ActividadesEnsenciales(int idIndiceOcupacional)
        {
            if (idIndiceOcupacional==0) {
                idIndiceOcupacional = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idIndiceOcupacionalSession));
            }
            var usuario = new VIewCompetencias
            {
                IdIndiceOcupacional = idIndiceOcupacional
            };
            var lista = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluador>(usuario, new Uri(WebApp.BaseAddress)
                                                                   , "api/EvaluacionDesempeno/Actividades");

            var totalacti = lista.ListaActividad.Count();
            lista.totalactividades = totalacti;
            InicializarMensaje(null);
            lista.ListaActividad = lista.ListaActividad;
            lista.IdEmpleado = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idEmpleadoSession));
            lista.IdJefe = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idEvaluadorSession));
            lista.IdEval001= Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idEval011Session));
            return View(lista);

        }
        [HttpPost]
        public async Task<IActionResult> ActividadesEnsenciales(ViewModelEvaluador viewModelEvaluador)
        {
            Response response = new Response();
            response = await apiServicio.InsertarAsync<Response>(viewModelEvaluador, new Uri(WebApp.BaseAddress)
                                                                   , "api/EvaluacionDesempeno/Insertarctividades");

            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
                return View();
           

        }
        public async Task<IActionResult> ConocimientoEsenciales(int idIndiceOcupacional)
        {
            var lista = new ViewModelEvaluador();
            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = idIndiceOcupacional,

            };
            var ListaConocimientos = await apiServicio.Listar<AreaConocimientoViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosPorIndiceOcupacional");
            InicializarMensaje(null);
            lista.ListaConocimientos = ListaConocimientos;
            ViewData["IdNivelConocimiento"] = new SelectList(await apiServicio.Listar<NivelConocimiento>(new Uri(WebApp.BaseAddress), "api/NivelesConocimiento/ListarNivelesConocimiento"), "IdNivelConocimiento", "Nombre");
            return View(lista);

        }
        public async Task<IActionResult> CompetenciasTecnicas(int idIndiceOcupacional)
        {
            var lista = new ViewModelEvaluador();
            var valor = new VIewCompetencias
            {
                IdIndiceOcupacional = idIndiceOcupacional,
                CompetenciaTecnica = true
            };
            var CompetenciasTecnicas = await apiServicio.Listar<ComportamientoObservableViewModel>(valor, new Uri(WebApp.BaseAddress), "api/ComportamientosObservables/ListarComportamientosObservablesPorIndiceOcupacionalEstado");

            InicializarMensaje(null);
            lista.ListaCompetenciasTecnicas = CompetenciasTecnicas;
            ViewData["IdNivelDesarrollo"] = new SelectList(await apiServicio.Listar<NivelDesarrollo>(new Uri(WebApp.BaseAddress), "api/NivelesDesarrollo/ListarNivelesDesarrollo"), "IdNivelDesarrollo", "Nombre");
            return View(lista);

        }
        public async Task<IActionResult> CompetenciasUniversales(int idIndiceOcupacional)
        {
            var lista = new ViewModelEvaluador();
            var valor = new VIewCompetencias
            {
                IdIndiceOcupacional = idIndiceOcupacional,
                CompetenciaTecnica = false
            };
            var CompetenciasUniversales = await apiServicio.Listar<ComportamientoObservableViewModel>(valor, new Uri(WebApp.BaseAddress), "api/ComportamientosObservables/ListarComportamientosObservablesPorIndiceOcupacionalEstado");

            InicializarMensaje(null);
            lista.ListaCompetenciasUniversales = CompetenciasUniversales;
            ViewData["IdFrecuenciaAplicacion"] = new SelectList(await apiServicio.Listar<FrecuenciaAplicacion>(new Uri(WebApp.BaseAddress), "api/FrecuenciaAplicaciones/ListarFrecuenciaAplicaciones"), "IdFrecuenciaAplicacion", "Nombre");
            return View(lista);

        }
        

        
        public async Task<IActionResult> Evaluar(int idempleado)
        {

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var usuario = new ViewModelEvaluador
                {
                    IdEmpleado = idempleado,
                    NombreUsuario = NombreUsuario

                };
                var lista = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluador>(usuario, new Uri(WebApp.BaseAddress)
                                                                   , "api/EvaluacionDesempeno/Evaluar");
                var totalacti = lista.ListaActividad.Count();
                lista.totalactividades = totalacti;

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = lista.IdIndiceOcupacional,
                    
                };
                var ListaConocimientos= await apiServicio.Listar<AreaConocimientoViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosPorIndiceOcupacional");

                var valor = new VIewCompetencias
                {
                    IdIndiceOcupacional = lista.IdIndiceOcupacional,
                    CompetenciaTecnica = true
                };
                var CompetenciasTecnicas = await apiServicio.Listar<ComportamientoObservableViewModel>(valor, new Uri(WebApp.BaseAddress), "api/ComportamientosObservables/ListarComportamientosObservablesPorIndiceOcupacionalEstado");

                var competenciasUniversales = new VIewCompetencias
                {
                    IdIndiceOcupacional = lista.IdIndiceOcupacional,
                    CompetenciaTecnica = false
                };
                var CompetenciasUniversales = await apiServicio.Listar<ComportamientoObservableViewModel>(competenciasUniversales, new Uri(WebApp.BaseAddress), "api/ComportamientosObservables/ListarComportamientosObservablesPorIndiceOcupacionalEstado");

                lista.ListaConocimientos = ListaConocimientos;
                lista.ListaCompetenciasTecnicas = CompetenciasTecnicas;
                lista.ListaCompetenciasUniversales = CompetenciasUniversales;
                InicializarMensaje(null);
                ViewData["IdNivelConocimiento"] = new SelectList(await apiServicio.Listar<NivelConocimiento>(new Uri(WebApp.BaseAddress), "api/NivelesConocimiento/ListarNivelesConocimiento"), "IdNivelConocimiento", "Nombre");
                ViewData["IdNivelDesarrollo"] = new SelectList(await apiServicio.Listar<NivelDesarrollo>(new Uri(WebApp.BaseAddress), "api/NivelesDesarrollo/ListarNivelesDesarrollo"), "IdNivelDesarrollo", "Nombre");
                ViewData["IdFrecuenciaAplicacion"] = new SelectList(await apiServicio.Listar<FrecuenciaAplicacion>(new Uri(WebApp.BaseAddress), "api/FrecuenciaAplicaciones/ListarFrecuenciaAplicaciones"), "IdFrecuenciaAplicacion", "Nombre");
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