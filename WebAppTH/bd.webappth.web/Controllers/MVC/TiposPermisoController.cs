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
    public class TiposPermisoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TiposPermisoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoPermiso TipoPermiso)
        {

            if (!ModelState.IsValid)
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}";
                return View(TipoPermiso);
            }

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(TipoPermiso,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TiposPermiso/InsertarTipoPermiso");
                if (response.IsSuccess)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Success}|{response.Message}|{"7000"}";
                    return RedirectToAction("Index");
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"7000"}";
                return View(TipoPermiso);

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
                                                                  "api/TiposPermiso");


                    respuesta.Resultado = JsonConvert.DeserializeObject<TipoPermiso>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, TipoPermiso TipoPermiso)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, TipoPermiso, new Uri(WebApp.BaseAddress),
                                                                 "api/TiposPermiso");

                    if (response.IsSuccess)
                    {
                        this.TempData["MensajeTimer"] = $"{Mensaje.Success}|{response.Message}|{"7000"}";
                        return RedirectToAction("Index");
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                    return View(TipoPermiso);

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

            var lista = new List<TipoPermiso>();
            try
            {
                lista = await apiServicio.Listar<TipoPermiso>(new Uri(WebApp.BaseAddress)
                                                                    , "api/TiposPermiso/ListarTiposPermiso");

                //InicializarMensaje(mensaje);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando un tipo de permiso",
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
                                                               , "api/TiposPermiso");
                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                            "TiposPermiso",
                            "Index",
                             new { identificacion = response.Resultado },
                            $"{Mensaje.Success}|{response.Message}|{"10000"}"
                         );
                }

                return this.RedireccionarMensajeTime(
                            "TiposPermiso",
                            "Index",
                             new { identificacion = response.Resultado },
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