using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class FlujosAprobacionController : Controller
    {
        private readonly IApiServicio apiServicio;


        public FlujosAprobacionController(IApiServicio apiServicio)
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


        public async Task<IActionResult> Index()
        {

            var lista = new List<FlujoAprobacion>();
            try
            {
                lista = await apiServicio.Listar<FlujoAprobacion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/FlujosAprobacion/ListarFlujosAprobacion");
                InicializarMensaje(null);
                return View(lista);
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
                                                                  "api/FlujosAprobacion");


                    respuesta.Resultado = JsonConvert.DeserializeObject<FlujoAprobacion>(respuesta.Resultado.ToString());

                    
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);

                        await InicializarCombos();
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


        /*

        public async Task<IActionResult> Create()
        {
            InicializarMensaje(null);
            ViewData["IdTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales"), "IdTipoAccionPersonal", "Nombre");
            ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "NombreApellido");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlujoAprobacion FlujoAprobacion)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(FlujoAprobacion);

            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(FlujoAprobacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FlujosAprobacion/InsertarFlujoAprobacion");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un flujo de aprobación",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "flujo de Aprobación:", FlujoAprobacion.IdFlujoAprobacion),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales"), "IdTipoAccionPersonal", "Nombre");
                ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "NombreApellido");


                return View(FlujoAprobacion);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un flujo de aprobación",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, FlujoAprobacion FlujoAprobacion)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, FlujoAprobacion, new Uri(WebApp.BaseAddress),
                                                                 "api/FlujosAprobacion");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Flujo de Aprobación", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un flujo de aprobación",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales"), "IdTipoAccionPersonal", "Nombre");
                    ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "NombreApellido");


                    return View(FlujoAprobacion);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un flujo de aprobación",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
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
                                                               , "api/FlujosAprobacion");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de un flujo de aprobación",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                //return BadRequest();
                return RedirectToAction("Index", new { mensaje = response.Message });

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar un flujo de aprobación",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }
        */


        public async Task InicializarCombos()
        {

            try
            {
                // Combo de sucursales
                var listaSucursal = await apiServicio.Listar<Sucursal>(
                    new Uri(WebApp.BaseAddress),
                    "api/Sucursal/ListarSucursal");
                
                ViewData["Sucursal"] = new SelectList(listaSucursal, "IdSucursal", "Nombre");


                // Combo de sucursales
                var listaTipoAccionPersonal = await apiServicio.Listar<TipoAccionPersonal>(
                    new Uri(WebApp.BaseAddress),
                    "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

                ViewData["TipoAccionPersonal"] = new SelectList(listaTipoAccionPersonal, "IdTipoAccionPersonal", "Nombre");
                
            }
            catch (Exception ex)
            {

            }

        }

        public async Task<IActionResult> ObtenerManualPuestosPorSucursal(int IdSucursal)
        {

            var lista = new List<ManualPuesto>();

            try
            {
                var filtro = new IdFiltrosViewModel { IdSucursal = IdSucursal };

                lista = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                    filtro,
                    new Uri(WebApp.BaseAddress),
                    "api/ManualPuestos/ListarManualPuestoPorSucursal");

                return Json(lista);
            }
            catch (Exception)
            {

                return Json(lista);
            }

        }


    }
}