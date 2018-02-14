using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class DependenciasController : Controller
    {
        private readonly IApiServicio apiServicio;

        public DependenciasController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Index()
        {
            var lista = new List<Dependencia>();
            try
            {
                lista = await apiServicio.Listar<Dependencia>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Dependencias/ListarDependencias");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando dependencias",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }


        public async Task<IActionResult> Create(string mensaje)
        {

            await CargarListaCombox();
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dependencia dependencia)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    dependencia.IdDependenciaPadre = dependencia.IdDependencia;
                    dependencia.IdDependencia = 0;

                    response = await apiServicio.InsertarAsync(dependencia,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Dependencias/InsertarDependencia");
                    if (response.IsSuccess)
                    {

                        var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            ExceptionTrace = null,
                            Message = "Se ha creado un indice ocupacional",
                            UserName = "Usuario 1",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            EntityID = string.Format("{0} {1}", "Dependencia:", dependencia.IdDependencia),
                        });

                        return RedirectToAction("Index");
                    }
                }
                await CargarListaCombox();
                InicializarMensaje(response.Message);
                return View(dependencia);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }

        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }


        private async Task CargarListaCombox()
        {
            var listaSucursal = await apiServicio.Listar<Sucursal>(new Uri(WebApp.BaseAddress), "api/Sucursal/ListarSucursal");
            ViewData["IdSucursal"] = new SelectList(listaSucursal, "IdSucursal", "Nombre");
        }



        public async Task<JsonResult> ListarPadresPorSucursal(string idSucursal)
        {
            var sucursal = new Dependencia
            {
                IdSucursal = Convert.ToInt32(idSucursal),
            };
            var listaDependencia = await apiServicio.Listar<Dependencia>(sucursal, new Uri(WebApp.BaseAddress), "api/Dependencias/ListarPadresPorSucursal");
            return Json(listaDependencia);
        }
    }
}