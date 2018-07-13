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
using Newtonsoft.Json;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class DenominacionesCompetenciasController : Controller
    {
        private readonly IApiServicio apiServicio;


        public DenominacionesCompetenciasController(IApiServicio apiServicio)
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
        public IActionResult Create(string mensaje)
        {
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DenominacionCompetencia denominacionCompetencia)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(denominacionCompetencia);

            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(denominacionCompetencia,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/DenominacionesCompetencias/InsertarDenominacionCompetencia");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "DenominacionesCompetencias",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                    
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{response.Message}|{"10000"}";
                return View(denominacionCompetencia);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Denominación Competencia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

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
                                                                  "api/DenominacionesCompetencias");


                    respuesta.Resultado = JsonConvert.DeserializeObject<DenominacionCompetencia>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, DenominacionCompetencia denominacionCompetencia)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, denominacionCompetencia, new Uri(WebApp.BaseAddress),
                                                                 "api/DenominacionesCompetencias");

                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                            "DenominacionesCompetencias",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );

                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{response.Message}|{"10000"}";
                    
                    return View(denominacionCompetencia);

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

            var lista = new List<DenominacionCompetencia>();
            try
            {
                lista = await apiServicio.Listar<DenominacionCompetencia>(new Uri(WebApp.BaseAddress)
                                                                    , "api/DenominacionesCompetencias/ListarDenominacionesCompetencias");
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
                                                               , "api/DenominacionesCompetencias");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "DenominacionesCompetencias",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }
                return this.RedireccionarMensajeTime("DenominacionesCompetencias","Index", new { mensaje = response.Message });
                //return BadRequest();
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }
    }
}