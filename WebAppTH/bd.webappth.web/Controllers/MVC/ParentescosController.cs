using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.Negocio;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class ParentescosController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ParentescosController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create(Parentesco Parentesco)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(Parentesco);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(Parentesco,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Parentescos/InsertarParentescos");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "Parentescos",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                return View(Parentesco);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Parentesco",
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
                                                                  "api/Parentescos");


                    respuesta.Resultado = JsonConvert.DeserializeObject<Parentesco>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, Parentesco Parentesco)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, Parentesco, new Uri(WebApp.BaseAddress),
                                                                 "api/Parentescos");

                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                            "Parentescos",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
               
                    return View(Parentesco);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un Parentesco",
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

            var lista = new List<Parentesco>();
            try
            {
                lista = await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Parentescos/ListarParentescos");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Parentescos",
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
                                                               , "api/Parentescos");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "Parentescos",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }

                return this.RedireccionarMensajeTime(
                            "Parentescos",
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