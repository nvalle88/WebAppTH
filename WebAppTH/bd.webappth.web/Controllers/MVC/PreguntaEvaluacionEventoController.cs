using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class PreguntaEvaluacionEventoController : Controller 
    {
        private readonly IApiServicio apiServicio;


        public PreguntaEvaluacionEventoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }
        
        public async Task<IActionResult> IndexFacilitador()
        {
            var lista = new List<PreguntaEvaluacionEvento>();
            try
            {
                lista = await apiServicio.Listar<PreguntaEvaluacionEvento>(new Uri(WebApp.BaseAddress)
                                                                    , "api/PreguntaEvaluacionEvento/ListarFacilitador");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        public async Task<IActionResult> IndexOrganizador()
        {
            var lista = new List<PreguntaEvaluacionEvento>();
            try
            {
                lista = await apiServicio.Listar<PreguntaEvaluacionEvento>(new Uri(WebApp.BaseAddress)
                                                                    , "api/PreguntaEvaluacionEvento/ListarOrganizador");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        public async Task<IActionResult> IndexConocimiento()
        {
            var lista = new List<PreguntaEvaluacionEvento>();
            try
            {
                lista = await apiServicio.Listar<PreguntaEvaluacionEvento>(new Uri(WebApp.BaseAddress)
                                                                    , "api/PreguntaEvaluacionEvento/ListarConocimiento");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
       
        public IActionResult CreateFacilitador()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacilitador(PreguntaEvaluacionEvento generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/PreguntaEvaluacionEvento/InsertarPreguntas");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexFacilitador");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> EditFacilitador(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PreguntaEvaluacionEvento");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<PreguntaEvaluacionEvento>(respuesta.Resultado.ToString());
                        var ciuadad = new PreguntaEvaluacionEvento
                        {
                            IdPreguntaEvaluacionEvento = respuestas.IdPreguntaEvaluacionEvento,
                            Descripcion = respuestas.Descripcion,
                            Facilitador = respuestas.Facilitador,
                            Organizador = respuestas.Organizador,
                            ConocimientoObtenidos = respuestas.ConocimientoObtenidos
                        };
                        return View(ciuadad);
                    }

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFacilitador(string id, PreguntaEvaluacionEvento generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/PreguntaEvaluacionEvento");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexFacilitador");
                    }

                    return View(generalCapacitacion);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteFacilitador(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PreguntaEvaluacionEvento");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexFacilitador");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public IActionResult CreateOrganizador()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganizador(PreguntaEvaluacionEvento generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/PreguntaEvaluacionEvento/InsertarPreguntas");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexOrganizador");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> EditOrganizador(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PreguntaEvaluacionEvento");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<PreguntaEvaluacionEvento>(respuesta.Resultado.ToString());
                        var ciuadad = new PreguntaEvaluacionEvento
                        {
                            IdPreguntaEvaluacionEvento = respuestas.IdPreguntaEvaluacionEvento,
                            Descripcion = respuestas.Descripcion,
                            Facilitador = respuestas.Facilitador,
                            Organizador = respuestas.Organizador,
                            ConocimientoObtenidos = respuestas.ConocimientoObtenidos
                        };
                        return View(ciuadad);
                    }

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditOrganizador(string id, PreguntaEvaluacionEvento generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/PreguntaEvaluacionEvento");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexOrganizador");
                    }

                    return View(generalCapacitacion);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteOrganizador(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PreguntaEvaluacionEvento");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexOrganizador");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public IActionResult CreateConocimiento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateConocimiento(PreguntaEvaluacionEvento generalCapacitacion)
        {

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/PreguntaEvaluacionEvento/InsertarPreguntas");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexConocimiento");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> EditConocimiento(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PreguntaEvaluacionEvento");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<PreguntaEvaluacionEvento>(respuesta.Resultado.ToString());
                        var ciuadad = new PreguntaEvaluacionEvento
                        {
                            IdPreguntaEvaluacionEvento = respuestas.IdPreguntaEvaluacionEvento,
                            Descripcion = respuestas.Descripcion,
                            Facilitador = respuestas.Facilitador,
                            Organizador = respuestas.Organizador,
                            ConocimientoObtenidos = respuestas.ConocimientoObtenidos
                        };
                        return View(ciuadad);
                    }

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditConocimiento(string id, PreguntaEvaluacionEvento generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/PreguntaEvaluacionEvento");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexConocimiento");
                    }

                    return View(generalCapacitacion);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteConocimiento(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PreguntaEvaluacionEvento");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexConocimiento");
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