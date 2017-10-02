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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class IndiceOcupacionalCapacitacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public IndiceOcupacionalCapacitacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
            //ViewData["IdIndiceOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<IndiceOcupacional>(new Uri(WebApp.BaseAddress), "/api/IndicesOcupacionales/ListarIndicesOcupacionales"), "IdIndiceOcupacional", "Nombre");
            ViewData["IdCapacitacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Capacitacion>(new Uri(WebApp.BaseAddress), "/api/Capacitaciones/ListarCapacitaciones"), "IdCapacitacion", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertarIndiceOcupacionalCapacitaciones(IndiceOcupacionalCapacitaciones indiceOcupacionalCapacitaciones)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(indiceOcupacionalCapacitaciones,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/IndiceOcupacionalCapacitaciones/InsertarIndiceOcupacionalCapacitacion");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un Rol de Brigada Salud y Seguridad Ocupacional",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Rol de Brigada Salud y Seguridad Ocupacional:", indiceOcupacionalCapacitaciones.IdIndiceOcupacionalCapacitaciones),
                    });

                    return RedirectToAction("Detalles", new { id= indiceOcupacionalCapacitaciones.IdIndiceOcupacional});
                }

                //ViewData["IdIndiceOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<IndiceOcupacional>(new Uri(WebApp.BaseAddress), "/api/IndicesOcupacionales/ListarIndicesOcupacionales"), "IdIndiceOcupacional", "Nombre");
                ViewData["IdCapacitacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Capacitacion>(new Uri(WebApp.BaseAddress), "/api/Capacitaciones/ListarCapacitaciones"), "IdCapacitacion", "Nombre");
                return View(indiceOcupacionalCapacitaciones);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Rol de Brigada Salud y Seguridad Ocupacional",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }





        public async Task<IActionResult> CargarCapacitaciones(string id)
        {

            var lista = new List<Capacitacion>();

            try
            {
                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional= Convert.ToInt32(id),
                };
                lista = await apiServicio.Listar<Capacitacion>(indiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/IndiceOcupacionalCapacitaciones/ListaFiltradaCapacitaciones");
                ViewData["IdCapacitacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(lista, "IdCapacitacion", "Nombre");

                //ViewBag.listacap = new SelectList(lista, "IdCapacitacion", "Nombre");


                var Indice = new IndiceOcupacionalCapacitaciones
                {
                    IdIndiceOcupacional=Convert.ToInt32(id),
                };

                return PartialView("..//Indicadores//Createejemplo", Indice);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Index()
        {

            var lista = new List<IndiceOcupacionalCapacitaciones>();
            try
            {
                lista = await apiServicio.Listar<IndiceOcupacionalCapacitaciones>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/IndiceOcupacionalCapacitaciones/ListarIndiceOcupacionalCapacitaciones");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Rol de Brigada Salud y Seguridad Ocupacional",
                    ExceptionTrace = ex,
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
                                                               , "/api/IndiceOcupacionalCapacitaciones");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de Rol de Brigada Salud y Seguridad Ocupacional eliminado",
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
                    Message = "Eliminar Brigada SSO Roles",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }
    }
}