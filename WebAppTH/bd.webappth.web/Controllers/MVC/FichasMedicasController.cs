using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ViewModels;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class FichasMedicasController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public FichasMedicasController(IApiServicio apiServicio)
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






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FichaMedicaViewModel fichaMedicaViewModel)
        {
            Response response = new Response();
            try
            {

                //string identificacion = fichaMedicaViewModel.DatosBasicosPersonaViewModel.Identificacion;

                //var a = new FichaMedicaViewModel();
                //a.DatosBasicosPersonaViewModel.Identificacion = fichaMedicaViewModel.DatosBasicosPersonaViewModel.Identificacion;


                response = await apiServicio.ObtenerElementoAsync1<Response>(fichaMedicaViewModel,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/ListarFichaMedicaViewModel");



                if (response.IsSuccess)
                    {
                        var fms = JsonConvert.DeserializeObject<FichaMedicaViewModel>(response.Resultado.ToString());
                    
                        return View(fms);
                    }


                ViewData["Error"] = response.Message;


                DatosBasicosPersonaViewModel dbvm = new DatosBasicosPersonaViewModel();

                dbvm.IdPersona = 0;

                FichaMedicaViewModel fmvm2 = new FichaMedicaViewModel();
                List<FichaMedica> listafm = new List<FichaMedica>();

                fmvm2.DatosBasicosPersonaViewModel = dbvm;
                fmvm2.FichasMedicas = listafm;

                return View(fmvm2);

                

            }
            catch (Exception ex)
            {
                /*
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = Mensaje.Excepcion,
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                */
                return BadRequest();

            }
        }




        public async Task<IActionResult> Create(string mensaje,string id)
        {
            
            InicializarMensaje(mensaje);
            var fichaMedica = new FichaMedica();

            int id2 = Convert.ToInt32(id);
            DateTime hoy = DateTime.Today;

            fichaMedica.IdPersona = id2;
            fichaMedica.FechaFichaMedica = hoy;

            fichaMedica.AntecedenteMedico = "";
            fichaMedica.AntecedenteQuirurgico = "";

            return View(fichaMedica);
            
            
        }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FichaMedica fichaMedica)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                return View(fichaMedica);
            }

                Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/InsertarFichasMedicas");
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
                        EntityID = string.Format("{0} {1}", "FichasMedicas:", fichaMedica.IdFichaMedica),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;

                return View(fichaMedica);

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
                return View(fichaMedica);


            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/FichasMedicas");


                    respuesta.Resultado = JsonConvert.DeserializeObject<FichaMedica>(respuesta.Resultado.ToString());

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
        public async Task<IActionResult> Edit(string id, FichaMedica FichaMedica)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, FichaMedica, new Uri(WebApp.BaseAddress),
                                                                 "api/FichasMedicas");

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
                    return View(FichaMedica);

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
            
            try
            {
                
                var a = new FichaMedicaViewModel();
                a.FichasMedicas = new List<FichaMedica>();
                a.DatosBasicosPersonaViewModel = new DatosBasicosPersonaViewModel();
                return View(a);
                
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
                                                               , "api/FichasMedicas");
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