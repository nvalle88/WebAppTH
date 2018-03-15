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
using bd.webappth.entidades.ViewModels;

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(FichaMedica fichaMedica)
        {
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
            ExamenComplementario antLab = new ExamenComplementario();
            antLab.IdFichaMedica = fichaMedica.IdFichaMedica;
            antLab.Fecha =  DateTime.Today;
            antLab.Resultado = "Esperando resultado";

            InicializarMensaje("");

            return View("Create", antLab);
        }

        

        public async Task<IActionResult> Create(string mensaje, FichaMedica fichaMedica)
        {
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
            ExamenComplementario antLab = new ExamenComplementario();

            InicializarMensaje(mensaje);

            return View(antLab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamenComplementario ExamenComplementario)
        {

            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
            ExamenesComplementariosViewModel alvm = new ExamenesComplementariosViewModel();


            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido);
                


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

                    
                    return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = ExamenComplementario.IdFichaMedica });
                }
                
                //return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = response.Message+"", idFicha = ExamenComplementario.IdFichaMedica });

                InicializarMensaje(Mensaje.ModeloInvalido);

                return View(ExamenComplementario);
            }
            catch (Exception ex)
            {
                InicializarMensaje(Mensaje.Error);
                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.Excepcion, idFicha = ExamenComplementario.IdFichaMedica });
            }


        }


        /*
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
        }*/



        public async Task<IActionResult> Edit(string id)
        {
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ExamenesComplementarios");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ExamenComplementario>(respuesta.Resultado.ToString());

                    
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

                        return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = ExamenComplementario.IdFichaMedica });
                    }

                    return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = response.Message, idFicha = ExamenComplementario.IdFichaMedica });


                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest();

            }
        }

        

        public async Task<IActionResult> Index(string mensaje, int idFicha, int idPersona)
        {

            var alvm = new ExamenesComplementariosViewModel();

            var lista = new List<ExamenComplementario>();

            var fichaMedica = new FichaMedica();
            fichaMedica.IdPersona = idPersona;
            fichaMedica.IdFichaMedica = idFicha;


            Response response = new Response();

            try
            {

                if (idPersona < 1)
                {
                    response = await apiServicio.InsertarAsync<Response>(fichaMedica.IdFichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerIdPersonaPorFicha");

                    if (response.IsSuccess)
                    {
                        fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

                    }

                }
                else
                {
                    response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerUltimaFichaMedica");


                    if (response.IsSuccess)
                    {
                        fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());
                    }

                }


                response = await apiServicio.InsertarAsync<Response>(fichaMedica.IdFichaMedica,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/ExamenesComplementarios/ListarExamenesComplementariosPorFicha");

                if (response.IsSuccess)
                {
                    lista = JsonConvert.DeserializeObject<List<ExamenComplementario>>(response.Resultado.ToString());
                }



                InicializarMensaje(mensaje);


                alvm.ListaExamenesComplementarios = lista;
                alvm.fichaMedica = fichaMedica;

                return View(alvm);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }



        public async Task<IActionResult> Delete(string id, int idFM)
        {

            FichaMedica fm = new FichaMedica();
            fm.IdFichaMedica = idFM;

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/ExamenesComplementarios");
                if (response.IsSuccess)
                {

                    return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.BorradoSatisfactorio, idFicha = idFM });

                }

                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = response.Message, idFicha = idFM });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

    }
}