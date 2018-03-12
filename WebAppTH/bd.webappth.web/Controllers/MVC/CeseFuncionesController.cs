using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using Newtonsoft.Json;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class CeseFuncionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public CeseFuncionesController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create(int id ,string mensaje)
        {
            try
            {
                if (id != 0)
                {
                    //var respuesta = await apiServicio.SeleccionarAsync<Response>(id.ToString(), new Uri(WebApp.BaseAddress),
                    //                                              "api/CeseFunciones");

                    //if (respuesta.IsSuccess && respuesta.Resultado != null)
                    //{
                    //    respuesta.Resultado = JsonConvert.DeserializeObject<CeseFuncion>(respuesta.Resultado.ToString());
                    //    ViewData["IdTipoCesacionFuncion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoCesacionFuncion>(new Uri(WebApp.BaseAddress), "api/TipoCesacionFunciones/ListarTipoCesacionFunciones"), "IdTipoCesacionFuncion", "Nombre");
                    //    return View(respuesta.Resultado);
                    //}
                    //else
                    //{
                    
                    var empleadoEnviar = new Empleado
                    {
                        IdEmpleado=id,
                    };
                    var empleado = await apiServicio.ObtenerElementoAsync1<EmpleadoSolicitudViewModel>(empleadoEnviar, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDatosEmpleadoSeleccionado");
                    ViewData["IdTipoCesacionFuncion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoCesacionFuncion>(new Uri(WebApp.BaseAddress), "api/TipoCesacionFunciones/ListarTipoCesacionFunciones"), "IdTipoCesacionFuncion", "Nombre");
                    ViewData["NombresApellidos"] = empleado.NombreApellido;
                    ViewData["Identificacion"] = empleado.Identificacion;

                    var ceseFuncion = new CeseFuncion
                        {
                            IdEmpleado = id,
                        };
                    InicializarMensaje(mensaje);
                        return View(ceseFuncion);
                    //}



                }



                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> SeleccionarDependencia()
        {
            await CargarCiudades();
            return View();
        }

        private async Task CargarCiudades()
        {
            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");

        }


        public async Task<JsonResult> ListarSucursalesPorCiudad(int idciudad)
        {
            try
            {
                var ciudad = new Ciudad
                {
                    IdCiudad = idciudad,
                };
                var listarsucursales= await apiServicio.Listar<Sucursal>(ciudad, new Uri(WebApp.BaseAddress), "api/Sucursal/ListarSucursalporCiudad");
                return Json(listarsucursales);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> ListarDependenciasporSucursal(int idsucursal)
        {
            try
            {
                var sucursal = new Sucursal
                {
                    IdSucursal = idsucursal,
                };
                var listardependencias = await apiServicio.Listar<Dependencia>(sucursal, new Uri(WebApp.BaseAddress), "api/Dependencias/ListarDependenciaporSucursal");
                return Json(listardependencias);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CeseFuncion ceseFuncion)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ceseFuncion);
            }
            Response response = new Response();
            try
            {
                ceseFuncion.Fecha = DateTime.Now;
                response = await apiServicio.InsertarAsync(ceseFuncion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/CeseFunciones/InsertarCeseFuncion");
                if (response.IsSuccess)
                {
                    var empleado = new Empleado
                    {
                        IdEmpleado = ceseFuncion.IdEmpleado,
                    };
                    response = await apiServicio.EditarAsync(ceseFuncion.IdEmpleado.ToString(), empleado, new Uri(WebApp.BaseAddress),
                                                                 "api/Empleados");

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un cese de funciones",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Cese de Funciones:", ceseFuncion.IdCeseFuncion),
                    });

                    return RedirectToAction("SeleccionarDependencia");
                }

                ViewData["Error"] = response.Message;
                return View(ceseFuncion);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Cese de Funciones",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }


        public async Task<JsonResult> ListarEmpleadosparaCesacion(int idDependencia)
        {

            //var listadoEmpleados = await ListarEmpleadosparaCesacion(idDependencia);
            return Json(new { result = "Redireccionar", url = Url.Action("Index", "CeseFunciones", new {idDependenciaEnviar= idDependencia }) });
        }

       


        public async Task<IActionResult> Index (int idDependenciaEnviar)
        {
            try
            {
                var empleado = new Empleado
                {
                    IdDependencia = idDependenciaEnviar,
                };
                var listaempleados = await apiServicio.Listar<EmpleadoSolicitudViewModel>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleadosdeDependenciaSinCesacion");
                InicializarMensaje(null);
                return View(listaempleados);
            }
            catch (Exception)
            {
                return View();
            }

        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/CeseFunciones");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de cese de funciones eliminado",
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
                    Message = "Eliminar Cese de Funciones",
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