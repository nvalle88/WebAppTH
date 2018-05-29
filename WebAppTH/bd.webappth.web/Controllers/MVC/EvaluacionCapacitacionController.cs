using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class EvaluacionCapacitacionController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IApiServicio apiServicio;


        public EvaluacionCapacitacionController(IApiServicio apiServicio)
        {

            this.apiServicio = apiServicio;


        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                //var lista = new List<ViewModelEvaluacionCapacitaciones>();
                var envia = new ViewModelEvaluacionCapacitaciones
                {
                    NombreUsuario = NombreUsuario
                };

                var lista = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluacionCapacitaciones>(envia, new Uri(WebApp.BaseAddress)
                                                                    , "api/EvaluacionCapacitacion/ObtenerEvaluacionesEmpleado");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
       
        
        public async Task<IActionResult> Evaluacion(int id)
        {
            try
            {
                var a = new ViewModelEvaluacionCapacitaciones();
                var envia = new ViewModelEvaluacionCapacitaciones
                {
                    IdPlanCapacitacion = id,

                };
                var datos = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluacionCapacitaciones>(envia, new Uri(WebApp.BaseAddress)
                                                                    , "api/EvaluacionCapacitacion/ObtenerDatosEvaluacion");
                if (datos != null)
                {
                    a.ListaPreguntaEvaluacionFacilitador = datos.ListaPreguntaEvaluacionEvento.Where(x => x.Facilitador == true).ToList();
                    a.ListaPreguntaOrganizador = datos.ListaPreguntaEvaluacionEvento.Where(x => x.Organizador == true).ToList();
                    a.ListaPreguntaEvaluacionConocimiento = datos.ListaPreguntaEvaluacionEvento.Where(x => x.ConocimientoObtenidos == true).ToList();
                    var recibe = new ViewModelEvaluacionCapacitaciones
                    {
                        ListaPreguntaEvaluacionFacilitador = a.ListaPreguntaEvaluacionFacilitador,
                        ListaPreguntaOrganizador = a.ListaPreguntaOrganizador,
                        ListaPreguntaEvaluacionConocimiento = a.ListaPreguntaEvaluacionConocimiento,
                        ListaPreguntaEvaluacionEvento = datos.ListaPreguntaEvaluacionEvento,
                        IdPlanCapacitacion = id,
                        NombreEvento = datos.NombreEvento,
                        Institucion = datos.Institucion,
                        LugarFecha = datos.LugarFecha
                    };

                    return View(recibe);
                }
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Evaluacion(ViewModelEvaluacionCapacitaciones viewModelEvaluacionCapacitaciones)
        {
            try
            {

                var response = await apiServicio.InsertarAsync(viewModelEvaluacionCapacitaciones, new Uri(WebApp.BaseAddress)
                                                                    , "api/EvaluacionCapacitacion/InsertarEvaluacion");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                return View(viewModelEvaluacionCapacitaciones);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var a = new ViewModelEvaluacionCapacitaciones();
                var envia = new ViewModelEvaluacionCapacitaciones
                {
                    IdPlanCapacitacion = id,

                };
                var datos = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluacionCapacitaciones>(envia, new Uri(WebApp.BaseAddress)
                                                                    , "api/EvaluacionCapacitacion/ObtenerDatosEvaluacionEvento");
                if (datos != null)
                {
                    a.ListaPreguntaEvaluacionFacilitador = datos.ListaPreguntaEvaluacionEvento.Where(x => x.Facilitador == true).ToList();
                    a.ListaPreguntaOrganizador = datos.ListaPreguntaEvaluacionEvento.Where(x => x.Organizador == true).ToList();
                    a.ListaPreguntaEvaluacionConocimiento = datos.ListaPreguntaEvaluacionEvento.Where(x => x.ConocimientoObtenidos == true).ToList();
                    var recibe = new ViewModelEvaluacionCapacitaciones
                    {
                        ListaPreguntaEvaluacionFacilitador = a.ListaPreguntaEvaluacionFacilitador,
                        ListaPreguntaOrganizador = a.ListaPreguntaOrganizador,
                        ListaPreguntaEvaluacionConocimiento = a.ListaPreguntaEvaluacionConocimiento,
                        ListaPreguntaEvaluacionEvento = datos.ListaPreguntaEvaluacionEvento,
                        IdPlanCapacitacion = id,
                        NombreEvento = datos.NombreEvento,
                        Institucion = datos.Institucion,
                        LugarFecha = datos.LugarFecha
                    };

                    return View(recibe);
                }
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}