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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class IndiceOcupacionalComportamientosObservablesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public IndiceOcupacionalComportamientosObservablesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create()
        {
            //ViewData["IdIndiceOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<IndiceOcupacional>(new Uri(WebApp.BaseAddress), "api/IndicesOcupacionales/ListarIndicesOcupacionales"), "IdIndiceOcupacional", "Nombre");
            ViewData["IdComportamientoObservable"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ComportamientoObservable>(new Uri(WebApp.BaseAddress), "api/ComportamientosObservables/ListarComportamientosObservables"), "IdComportamientoObservable", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndiceOcupacionalComportamientoObservable indiceOcupacionalComportamientoObservable)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(indiceOcupacionalComportamientoObservable,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/IndiceOcupacionalComportamientosObservables/InsertarIndiceOcupacionalComportamientoObservable");
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
                        EntityID = string.Format("{0} {1}", "Rol de Brigada Salud y Seguridad Ocupacional:", indiceOcupacionalComportamientoObservable.IdIndiceOcupacionalComportamientoObservable),
                    });

                    return RedirectToAction("Index");
                }

                //ViewData["IdIndiceOcupacional"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<IndiceOcupacional>(new Uri(WebApp.BaseAddress), "api/IndicesOcupacionales/ListarIndicesOcupacionales"), "IdIndiceOcupacional", "Nombre");
                ViewData["IdComportamientoObservable"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ComportamientoObservable>(new Uri(WebApp.BaseAddress), "api/ComportamientosObservables/ListarComportamientosObservables"), "IdComportamientoObservable", "Nombre");
                return View(indiceOcupacionalComportamientoObservable);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Rol de Brigada Salud y Seguridad Ocupacional",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }


        public PartialViewResult GetPartialViewIndiceOcupacionalComportamientoObservable()
        {
            return PartialView("_PartialViewIndiceOcupacionalComportamientoObservable");
        }


        public async Task<IActionResult> CargarComportamientosObservables(string id)
        {

            var lista = new List<ComportamientoObservable>();

            try
            {
                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = Convert.ToInt32(id),
                };
                lista = await apiServicio.Listar<ComportamientoObservable>(indiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/IndiceOcupacionalComportamientosObservables/ListaFiltradaComportamientosObservables");
                //ViewData["IdComportamientoObservable"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(lista, "IdComportamientoObservable", "Nombre");

                ViewBag.listacap = new SelectList(lista, "IdComportamientoObservable", "Nombre");


                var Indice = new IndiceOcupacionalComportamientoObservable
                {
                    IdIndiceOcupacional = Convert.ToInt32(id),
                };

                return PartialView("_PartialViewIndiceOcupacionalComportamientoObservable");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Index()
        {

            var lista = new List<IndiceOcupacionalComportamientoObservable>();
            try
            {
                lista = await apiServicio.Listar<IndiceOcupacionalComportamientoObservable>(new Uri(WebApp.BaseAddress)
                                                                    , "api/IndiceOcupacionalComportamientosObservables/ListarIndiceOcupacionalComportamientosObservables");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Rol de Brigada Salud y Seguridad Ocupacional",
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
                                                               , "api/IndiceOcupacionalComportamientosObservables");
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