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
    public class AntecedentesFamiliaresController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public AntecedentesFamiliaresController(IApiServicio apiServicio)
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
            
            InicializarMensaje(mensaje);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AntecedentesFamiliares AntecedentesFamiliares)
        {
            

            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido);
                ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                return View(AntecedentesFamiliares);
            }

                Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(AntecedentesFamiliares,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/AntecedentesFamiliares/InsertarAntecedentesFamiliares");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message + ", verifique que haya seleccionado un: código ficha médica";

                ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                return View(AntecedentesFamiliares);

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
                return View(AntecedentesFamiliares);


            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");


                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/AntecedentesFamiliares");


                    respuesta.Resultado = JsonConvert.DeserializeObject<AntecedentesFamiliares>(respuesta.Resultado.ToString());

                    //ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Empleado>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "Identificacion");


                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        return View(respuesta.Resultado);
                    }

                }

                ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                return View();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AntecedentesFamiliares AntecedentesFamiliares)
        {
            Response response = new Response();

            ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, AntecedentesFamiliares, new Uri(WebApp.BaseAddress),
                                                                 "api/AntecedentesFamiliares");

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
                    return View(AntecedentesFamiliares);

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

                return BadRequest();

            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            var lista = new List<AntecedentesFamiliares>();
            try
            {
                lista = await apiServicio.Listar<AntecedentesFamiliares>(new Uri(WebApp.BaseAddress)
                                                                    , "api/AntecedentesFamiliares/ListarAntecedentesFamiliares");
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
                                                               , "api/AntecedentesFamiliares");
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