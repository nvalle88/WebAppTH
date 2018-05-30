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
    public class ConsultaEvaluacionDesempenoController : Controller
    {
        
        private readonly IApiServicio apiServicio;

        public ConsultaEvaluacionDesempenoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                HttpContext.Session.SetInt32(Constantes.idEmpleadoSession, 0);
                HttpContext.Session.SetInt32(Constantes.idEval011Session, 0);

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var lista = new List<ViewModelEvaluacionDesempeno>();

                var usuario = new ViewModelEvaluacionDesempeno
                {
                    NombreUsuario = NombreUsuario

                };

                lista = await apiServicio.Listar<ViewModelEvaluacionDesempeno>(usuario, new Uri(WebApp.BaseAddress)
                                                                       , "api/EvaluacionDesempeno/ListarEmpleadosJefes");
                
                return View(lista);
            }
            catch (Exception ex)
            {
                return View();
                throw;
            }

        }


        public async Task<IActionResult> InformacionGeneral(int idempleado)
        {
            try
            {
                if (idempleado != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEmpleadoSession, idempleado);
                    
                }
                else {

                    idempleado = Convert.ToInt32(Constantes.idEmpleadoSession);

                }
                

                if (idempleado != 0)
                {
                    var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                    var nombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var filtro = new IdFiltrosViewModel
                    {
                        NombreUsuario = nombreUsuario,
                        IdEmpleadoEvaluado = idempleado

                    };


                    var modelo = await apiServicio.ObtenerElementoAsync1<EmpleadoListaEval001ViewModel>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/EvaluacionDesempeno/EmpleadoListaEval001ViewModel");

                    return View(modelo);
                    
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> ActividadesEsenciales(int idEval001) {

            try {

                if (idEval001 != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, idEval001);

                }
                else
                {
                    idEval001 = Convert.ToInt32(Constantes.idEval011Session);
                }


                var filtro = new IdFiltrosViewModel { IdEval001 = idEval001};

                var lista = await apiServicio.Listar<ActividadesEsencialesPorEval001ViewModel>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/EvaluacionDesempeno/ListarActividadesEsencialesViewModelPorEval001");


                return View(lista);

            } catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> ConocimientosEsenciales(int idEval001)
        {

            try
            {

                if (idEval001 != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, idEval001);

                }
                else
                {
                    idEval001 = Convert.ToInt32(Constantes.idEval011Session);
                }

                var filtro = new IdFiltrosViewModel { IdEval001 = idEval001 };

                var lista = await apiServicio.Listar<EvaluacionConocimiento>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/EvaluacionDesempeno/ListarEvaluacionConocimientoPorEval001");


                return View(lista);

            }
            catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> CompetenciasTecnicas(int idEval001)
        {

            try
            {

                if (idEval001 != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, idEval001);

                }
                else
                {
                    idEval001 = Convert.ToInt32(Constantes.idEval011Session);
                }

                var filtro = new IdFiltrosViewModel { IdEval001 = idEval001 };

                var lista = await apiServicio.Listar<EvaluacionCompetenciasTecnicasPuesto>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/EvaluacionDesempeno/ListarEvaluacionCompetenciasTecnicasPuestoPorEval001");


                return View(lista);

            }
            catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> CompetenciasUniversales(int idEval001)
        {

            try
            {
                if (idEval001 != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, idEval001);

                }

                var filtro = new IdFiltrosViewModel { IdEval001 = idEval001 };

                var lista = await apiServicio.Listar<EvaluacionCompetenciasUniversales>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/EvaluacionDesempeno/ListarCompetenciasUniversalesPorEval001");


                return View(lista);

            }
            catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> TrabajoEquipoIniciativaLiderazgo(int idEval001)
        {

            try
            {
                if (idEval001 != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, idEval001);

                }

                var filtro = new IdFiltrosViewModel { IdEval001 = idEval001 };

                var lista = await apiServicio.Listar<EvaluacionTrabajoEquipoIniciativaLiderazgo>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/EvaluacionDesempeno/ListarTrabajoEquipoIniciativaLiderazgoPorEval001");


                return View(lista);

            }
            catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Observaciones(int idEval001)
        {

            try
            {
                if (idEval001 != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, idEval001);

                }

                var filtro = new IdFiltrosViewModel { IdEval001 = idEval001 };

                var modelo = await apiServicio.ObtenerElementoAsync1<Eval001>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/EvaluacionDesempeno/ObtenerEval001");


                return View(modelo);

            }
            catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> CalificacionFinal(int idEval001)
        {
            try
            {
                if (idEval001 != 0)
                {
                    HttpContext.Session.SetInt32(Constantes.idEval011Session, idEval001);

                    var envio = new ViewModelEvaluador()
                    {
                        IdEval001 = idEval001

                    };

                    var modelo = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluador>(envio, new Uri(WebApp.BaseAddress), "api/EvaluacionDesempeno/CalcularTotales");

                    return View(modelo);

                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SessionCaducada}|{"10000"}";
                return RedirectToAction("Index");
            }


        }



    }
}