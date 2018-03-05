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
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class ExamenesComplementariosController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public ExamenesComplementariosController(IApiServicio apiServicio)
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
            ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

            DateTime hoy = DateTime.Today;

            ExamenComplementario ec = new ExamenComplementario();
            ec.Fecha = hoy;

            InicializarMensaje(mensaje);

            return View(ec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamenComplementario ExamenComplementario)
        {
            ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido + " verifique que haya seleccionado un: código de ficha médica y un: exámen complementario");

                ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

                return View(ExamenComplementario);
            }

                Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ExamenComplementario,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ExamenesComplementarios/InsertarExamenesComplementarios");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = Mensaje.Satisfactorio,
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "ExamenesComplementarios:", ExamenComplementario.IdExamenComplementario),
                    });

                    ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                    ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
                return View(ExamenComplementario);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = Mensaje.Excepcion,
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                InicializarMensaje(Mensaje.Error);
                return View(ExamenComplementario);


            }
        }

        public async Task<IActionResult> Edit(string id)
        {

            ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ExamenesComplementarios");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ExamenComplementario>(respuesta.Resultado.ToString());

                    //ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Empleado>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "Identificacion");


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
        public async Task<IActionResult> Edit(string id, ExamenComplementario ExamenComplementario)
        {

            ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, ExamenComplementario, new Uri(WebApp.BaseAddress),
                                                                 "api/ExamenesComplementarios");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Area de Conocimiento", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = Mensaje.Satisfactorio,
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    

                    //ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Empleado>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "Identificacion");

                    ViewData["Error"] = response.Message;

                    ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                    ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

                    return View(ExamenComplementario);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = Mensaje.Excepcion,
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
                return BadRequest();

            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            var lista = new List<ExamenComplementario>();
            try
            {
                lista = await apiServicio.Listar<ExamenComplementario>(new Uri(WebApp.BaseAddress)
                                                                    , "api/ExamenesComplementarios/ListarExamenesComplementarios");
                InicializarMensaje(mensaje);

                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = Mensaje.GenerandoListas,
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
                                                               , "api/ExamenesComplementarios");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Area de Conocimiento", id),
                        Message = Mensaje.Satisfactorio,
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = Mensaje.Excepcion,
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