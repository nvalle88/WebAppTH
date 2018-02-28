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

namespace bd.webappth.web.Controllers.MVC
{
    public class CiudadController : Controller
    {
        private readonly IApiServicio apiServicio;


        public CiudadController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create(string mensaje)
        {
            ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdProvincia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ciudad ciudad)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                ViewData["IdProvincia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
                return View(ciudad);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ciudad,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Ciudad/InsertarCiudad");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una Ciudad",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Ciudad:", ciudad.IdCiudad),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                ViewData["IdProvincia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
                return View(ciudad);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Ciudad",
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
                                                                  "api/Ciudad");
                    if (respuesta.IsSuccess)
                    {
                       
                       var respuestaCiudad = JsonConvert.DeserializeObject<Ciudad>(respuesta.Resultado.ToString());
                        var ciuadad = new Ciudad
                        {
                            IdCiudad=respuestaCiudad.IdCiudad,
                            IdProvincia=respuestaCiudad.Provincia.IdProvincia,
                            IdPais=respuestaCiudad.Provincia.Pais.IdPais,
                            Nombre=respuestaCiudad.Nombre,
                        };

                        ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre", respuestaCiudad.Provincia.Pais.IdPais);
                        var pais = new Pais {IdPais=ciuadad.IdPais};
                        ViewData["IdProvincia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Provincia>(pais,new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais"), "IdProvincia", "Nombre");
                        InicializarMensaje(null);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Ciudad ciudad)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, ciudad, new Uri(WebApp.BaseAddress),
                                                                 "api/Ciudad");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un registro sistema",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(ciudad);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una Ciudad",
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

            var lista = new List<Ciudad>();
            try
            {
                lista = await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Ciudad/ListarCiudad");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Ciudad",
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
                                                               , "api/Ciudad");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
               return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Ciudad",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

    }
}