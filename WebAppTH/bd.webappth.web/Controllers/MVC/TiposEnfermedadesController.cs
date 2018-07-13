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
    public class TiposEnfermedadesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TiposEnfermedadesController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create(TipoEnfermedad TipoEnfermedad)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(TipoEnfermedad);
            }


            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(TipoEnfermedad,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TiposEnfermedades/InsertarTipoEnfermedad");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "TiposEnfermedades",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );

                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                return View(TipoEnfermedad);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un tipo de enfermedad",
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
                                                                  "api/TiposEnfermedades");


                    respuesta.Resultado = JsonConvert.DeserializeObject<TipoEnfermedad>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, TipoEnfermedad TipoEnfermedad)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, TipoEnfermedad, new Uri(WebApp.BaseAddress),
                                                                 "api/TiposEnfermedades");

                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                             "TiposEnfermedades",
                             "Index",
                             $"{Mensaje.Success}|{response.Message}|{"7000"}"
                          );

                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                    return View(TipoEnfermedad);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un tipo de enfermedad",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            var lista = new List<TipoEnfermedad>();
            try
            {
                lista = await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress)
                                                                    , "api/TiposEnfermedades/ListarTiposEnfermedades");
                InicializarMensaje(mensaje);

                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando tipos de enfermedades",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/TiposEnfermedades");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                              "TiposEnfermedades",
                              "Index",
                              $"{Mensaje.Success}|{response.Message}|{"7000"}"
                           );
                }

                return this.RedireccionarMensajeTime(
                              "TiposEnfermedades",
                              "Index",
                              $"{Mensaje.Error}|{response.Message}|{"10000"}"
                           );
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }

    }
}