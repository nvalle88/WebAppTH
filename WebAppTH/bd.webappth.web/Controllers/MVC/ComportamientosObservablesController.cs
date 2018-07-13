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
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class ComportamientosObservablesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ComportamientosObservablesController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create( string mensaje)
        {
            ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
            ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
            InicializarMensaje(mensaje);
            return View();
        }

        public async Task<IActionResult> EliminarIndiceOcupacionalComportamiemtoObservable(int idComportamientoObservable, int idIndiceOcupacional)
        {
            try
            {

                var indiceOcupacionalComportamientoObservable = new IndiceOcupacionalComportamientoObservable
                {
                    IdComportamientoObservable = idComportamientoObservable,
                    IdIndiceOcupacional = idIndiceOcupacional
                };
                var response = await apiServicio.EliminarAsync(indiceOcupacionalComportamientoObservable, new Uri(WebApp.BaseAddress)
                                                               , "api/ComportamientosObservables/EliminarIndiceOcupacionalComportamiemtoObservable");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1} {2} {3}", "Compostamiento Observable ",
                                                                                        indiceOcupacionalComportamientoObservable.IdComportamientoObservable, "Índice Ocupacional", indiceOcupacionalComportamientoObservable.IdIndiceOcupacional),
                        Message = Mensaje.Satisfactorio,
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("ComportamientoObservable", "IndicesOcupacionales", new { id = indiceOcupacionalComportamientoObservable.IdIndiceOcupacional });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Area de Conocimiento",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComportamientoObservable comportamientoObservable)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(comportamientoObservable);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(comportamientoObservable,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ComportamientosObservables/InsertarComportamientoObservable");
                if (response.IsSuccess)
                {
                    
                    return this.RedireccionarMensajeTime(
                            "ComportamientosObservables",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );


                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{response.Message}|{"10000"}";

                ViewData["Error"] = response.Message;
                ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
                ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
                return View(comportamientoObservable);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ComportamientosObservables");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ComportamientoObservable>(respuesta.Resultado.ToString());
                    ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
                    ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        return View(respuesta.Resultado);
                    }

                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ComportamientoObservable comportamientoObservable)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, comportamientoObservable, new Uri(WebApp.BaseAddress),
                                                                 "api/ComportamientosObservables");

                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                              "ComportamientosObservables",
                              "Index",
                              $"{Mensaje.Success}|{response.Message}|{"7000"}"
                           );
                        
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                    ViewData["IdNivel"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Nivel>(new Uri(WebApp.BaseAddress), "api/Niveles/ListarNiveles"), "IdNivel", "Nombre");
                    ViewData["IdDenominacionCompetencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress), "api/DenominacionesCompetencias/ListarDenominacionesCompetencias"), "IdDenominacionCompetencia", "Nombre");
                    return View(comportamientoObservable);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            var lista = new List<ComportamientoObservable>();
            try
            {
                lista = await apiServicio.Listar<ComportamientoObservable>(new Uri(WebApp.BaseAddress)
                                                                    , "api/ComportamientosObservables/ListarComportamientosObservables");
                InicializarMensaje(mensaje);
                return View(lista);
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/ComportamientosObservables");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                              "ComportamientosObservables",
                              "Index",
                              $"{Mensaje.Success}|{response.Message}|{"7000"}"
                           );

                }
                
                
                return this.RedireccionarMensajeTime(
                              "ComportamientosObservables",
                              "Index",
                              $"{Mensaje.Aviso}|{response.Message}|{"7000"}"
                           );
            }
            catch (Exception ex)
            {
               
                return BadRequest();
            }
        }
    }
}