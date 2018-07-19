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
    public class TiposDiscapacidadesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TiposDiscapacidadesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoDiscapacidad TipoDiscapacidad)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(TipoDiscapacidad,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TiposDiscapacidades/InsertarTipoDiscapacidad");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "TiposDiscapacidades",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                    
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                
                return View(TipoDiscapacidad);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un tipo de discapacidad",
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
                                                                  "api/TiposDiscapacidades");


                    respuesta.Resultado = JsonConvert.DeserializeObject<TipoDiscapacidad>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
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
        public async Task<IActionResult> Edit(string id, TipoDiscapacidad TipoDiscapacidad)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, TipoDiscapacidad, new Uri(WebApp.BaseAddress),
                                                                 "api/TiposDiscapacidades");

                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                             "TiposDiscapacidades",
                             "Index",
                             $"{Mensaje.Success}|{response.Message}|{"7000"}"
                          );

                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                    return View(TipoDiscapacidad);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un tipo de discapacidad",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<TipoDiscapacidad>();
            try
            {
                lista = await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress)
                                                                    , "api/TiposDiscapacidades/ListarTiposDiscapacidades");
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
                                                               , "api/TiposDiscapacidades");
                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                              "TiposDiscapacidades",
                              "Index",
                              $"{Mensaje.Success}|{response.Message}|{"7000"}"
                           );
                }

                return this.RedireccionarMensajeTime(
                              "TiposDiscapacidades",
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