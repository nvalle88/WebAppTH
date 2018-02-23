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
    public class ProvinciaController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ProvinciaController(IApiServicio apiServicio)
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
            InicializarMensaje(mensaje);
            ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Provincia provincia)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                return View(provincia);
            }

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(provincia,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Provincia/InsertarProvincia");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una Provincia",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Provincia:", provincia.IdProvincia),
                    });

                    return RedirectToAction("Index");
                }
                ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                ViewData["Error"] = response.Message;
                return View(provincia);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Provincia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
            //ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/Provincia");


                    respuesta.Resultado = JsonConvert.DeserializeObject<Provincia>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
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
        public async Task<IActionResult> Edit(string id, Provincia provincia)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, provincia, new Uri(WebApp.BaseAddress),
                                                                 "api/Provincia");

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
                    ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                    ViewData["Error"] = response.Message;
                    return View(provincia);
                    

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una Provincia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<JsonResult> ListarProvinciaPorPais(int idPais)
        {
            var pais = new Pais { IdPais = idPais };

            var provincias = await apiServicio.Listar<Provincia>(pais,new Uri(WebApp.BaseAddress)
                                                                    , "api/Provincia/ListarProvinciaPorPais");
            return Json(provincias);

        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<Provincia>();
            try
            {
                lista = await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Provincia/ListarProvincia");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Provincia",
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
                                                               , "api/Provincia");
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
                    Message = "Eliminar Provincia",
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