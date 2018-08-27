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
    public class GeneralCapacitacionController : Controller
    {
        private readonly IApiServicio apiServicio;


        public GeneralCapacitacionController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }
        public async Task<IActionResult> IndexTipoCapacitacion()
        {
            var lista = new List<GeneralCapacitacion>();
            try
            {
                lista = await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/GeneralCapacitacion/ListarGeneralCapacitacionTipoCapacitacion");
                
                return View(lista);
            }
            catch (Exception ex)            {
                
                return BadRequest();
            }
        }
        public async Task<IActionResult> IndexEstadoEvento()
        {
            var lista = new List<GeneralCapacitacion>();
            try
            {
                lista = await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/GeneralCapacitacion/ListarGeneralCapacitacionEstadoEvento");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        public async Task<IActionResult> IndexNombreEvento()
        {
            var lista = new List<GeneralCapacitacion>();
            try
            {
                lista = await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/GeneralCapacitacion/ListarGeneralCapacitacionNombreEvento");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        public async Task<IActionResult> IndexTipoEvento()
        {
            var lista = new List<GeneralCapacitacion>();
            try
            {
                lista = await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/GeneralCapacitacion/ListarGeneralCapacitacionTipoEvento");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        public async Task<IActionResult> IndexEvaluacionEvento()
        {
            var lista = new List<GeneralCapacitacion>();
            try
            {
                lista = await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/GeneralCapacitacion/ListarGeneralCapacitacionTipoEvaluacion");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        public async Task<IActionResult> IndexAmbitoCapacitacion()
        {
            var lista = new List<GeneralCapacitacion>();
            try
            {
                lista = await apiServicio.Listar<GeneralCapacitacion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/GeneralCapacitacion/ListarGeneralCapacitacionAmbitoCapacitacion");

                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public IActionResult CreateAmbitoCapacitacion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmbitoCapacitacion(GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/GeneralCapacitacion/InsertarGeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexAmbitoCapacitacion");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {    
                return BadRequest();
            }
        }
        public async Task<IActionResult> EditAmbitoCapacitacion(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/GeneralCapacitacion");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<GeneralCapacitacion>(respuesta.Resultado.ToString());
                        var ciuadad = new GeneralCapacitacion
                        {
                            IdGeneralCapacitacion = respuestas.IdGeneralCapacitacion,
                            Nombre = respuestas.Nombre,
                            Tipo = respuestas.Tipo
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
        public async Task<IActionResult> EditAmbitoCapacitacion(string id, GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/GeneralCapacitacion");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexAmbitoCapacitacion");
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

        public async Task<IActionResult> DeleteAmbitoCapacitacion(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               ,"api/GeneralCapacitacion");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexAmbitoCapacitacion");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {                
                return BadRequest();
            }
        }

        public IActionResult CreateEstadoEvento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstadoEvento(GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/GeneralCapacitacion/InsertarGeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexEstadoEvento");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> EditEstadoEvento(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/GeneralCapacitacion");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<GeneralCapacitacion>(respuesta.Resultado.ToString());
                        var ciuadad = new GeneralCapacitacion
                        {
                            IdGeneralCapacitacion = respuestas.IdGeneralCapacitacion,
                            Nombre = respuestas.Nombre,
                            Tipo = respuestas.Tipo
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
        public async Task<IActionResult> EditEstadoEvento(string id, GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/GeneralCapacitacion");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexEstadoEvento");
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

        public async Task<IActionResult> DeleteEstadoEvento(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/GeneralCapacitacion");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexEstadoEvento");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public IActionResult CreateEvaluacionEvento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvaluacionEvento(GeneralCapacitacion generalCapacitacion)
        {

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/GeneralCapacitacion/InsertarGeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexEvaluacionEvento");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> EditEvaluacionEvento(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/GeneralCapacitacion");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<GeneralCapacitacion>(respuesta.Resultado.ToString());
                        var ciuadad = new GeneralCapacitacion
                        {
                            IdGeneralCapacitacion = respuestas.IdGeneralCapacitacion,
                            Nombre = respuestas.Nombre,
                            Tipo = respuestas.Tipo
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
        public async Task<IActionResult> EditEvaluacionEvento(string id, GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/GeneralCapacitacion");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexEvaluacionEvento");
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

        public async Task<IActionResult> DeleteEvaluacionEvento(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/GeneralCapacitacion");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexEvaluacionEvento");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public IActionResult CreateNombreEvento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNombreEvento(GeneralCapacitacion generalCapacitacion)
        {

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/GeneralCapacitacion/InsertarGeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexNombreEvento");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> EditNombreEvento(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/GeneralCapacitacion");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<GeneralCapacitacion>(respuesta.Resultado.ToString());
                        var ciuadad = new GeneralCapacitacion
                        {
                            IdGeneralCapacitacion = respuestas.IdGeneralCapacitacion,
                            Nombre = respuestas.Nombre,
                            Tipo = respuestas.Tipo
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
        public async Task<IActionResult> EditNombreEvento(string id, GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/GeneralCapacitacion");
                    if (response.IsSuccess)
                    {

                        return RedirectToAction("IndexNombreEvento");
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

        public async Task<IActionResult> DeleteNombreEvento(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/GeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexNombreEvento");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public IActionResult CreateTipoCapacitacion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTipoCapacitacion(GeneralCapacitacion generalCapacitacion)
        {

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/GeneralCapacitacion/InsertarGeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexTipoCapacitacion");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> EditTipoCapacitacion(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/GeneralCapacitacion");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<GeneralCapacitacion>(respuesta.Resultado.ToString());
                        var ciuadad = new GeneralCapacitacion
                        {
                            IdGeneralCapacitacion = respuestas.IdGeneralCapacitacion,
                            Nombre = respuestas.Nombre,
                            Tipo = respuestas.Tipo
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
        public async Task<IActionResult> EditTipoCapacitacion(string id, GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/GeneralCapacitacion");
                    if (response.IsSuccess)
                    {

                        return RedirectToAction("IndexTipoCapacitacion");
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

        public async Task<IActionResult> DeleteTipoCapacitacion(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/GeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexTipoCapacitacion");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public IActionResult CreateTipoEvento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTipoEvento(GeneralCapacitacion generalCapacitacion)
        {

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(generalCapacitacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/GeneralCapacitacion/InsertarGeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexTipoEvento");
                }

                return View(generalCapacitacion);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> EditTipoEvento(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/GeneralCapacitacion");
                    if (respuesta.IsSuccess)
                    {

                        var respuestas = JsonConvert.DeserializeObject<GeneralCapacitacion>(respuesta.Resultado.ToString());
                        var ciuadad = new GeneralCapacitacion
                        {
                            IdGeneralCapacitacion = respuestas.IdGeneralCapacitacion,
                            Nombre = respuestas.Nombre,
                            Tipo = respuestas.Tipo
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
        public async Task<IActionResult> EditTipoEvento(string id, GeneralCapacitacion generalCapacitacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, generalCapacitacion, new Uri(WebApp.BaseAddress),
                                                                 "api/GeneralCapacitacion");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexTipoEvento");
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

        public async Task<IActionResult> DeleteTipoEvento(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/GeneralCapacitacion");
                if (response.IsSuccess)
                {

                    return RedirectToAction("IndexTipoEvento");
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