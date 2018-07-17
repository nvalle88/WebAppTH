using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Servicios;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Interfaces;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class ExperienciaLaboralRequeridasController : Controller
    {

        private readonly IApiServicio apiServicio;

        public ExperienciaLaboralRequeridasController(IApiServicio apiServicio)
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

        public async Task<IActionResult> EliminarIndiceOcupacionalExperienciaLaboralRequerida(int idExperienciaLaboralRequerida, int idIndiceOcupacional)
        {

            try
            {

                var experienciaLaboralRequerida = new IndiceOcupacionalExperienciaLaboralRequerida
                {
                    IdExperienciaLaboralRequerida = idExperienciaLaboralRequerida,
                    IdIndiceOcupacional = idIndiceOcupacional
                };
                var response = await apiServicio.EliminarAsync(experienciaLaboralRequerida, new Uri(WebApp.BaseAddress)
                                                               , "api/ExperienciaLaboralRequeridas/EliminarIncideOcupacionalExperienciaLaboralRequeridas");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1} {2} {3}", "Experiencia laboral requerida ",
                        experienciaLaboralRequerida.IdExperienciaLaboralRequerida, "experiencia laboral requerida", experienciaLaboralRequerida.IdIndiceOcupacional),
                        Message = Mensaje.Satisfactorio,
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("ExperienciaLaboralRequerida", "IndicesOcupacionales", new { id = experienciaLaboralRequerida.IdIndiceOcupacional });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar experiencia laboral requerida",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }


        public async Task<IActionResult> Create()
        {
            InicializarMensaje(null);
            ViewData["IdEspecificidadExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EspecificidadExperiencia>(new Uri(WebApp.BaseAddress), "api/EspecificidadesExperiencia/ListarEspecificidadesExperiencia"), "IdEspecificidadExperiencia", "Descripcion");
            //ViewData["IdAnoExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AnoExperiencia>(new Uri(WebApp.BaseAddress), "api/AnosExperiencia/ListarAnosExperiencia"), "IdAnoExperiencia", "Descripcion");
            ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExperienciaLaboralRequerida ExperienciaLaboralRequerida)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ExperienciaLaboralRequerida,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ExperienciaLaboralRequeridas/InsertarExperienciaLaboralRequerida");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una experiencia laboral requerida",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Experiencia Laboral Requerida:", ExperienciaLaboralRequerida.IdExperienciaLaboralRequerida),
                    });

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                ViewData["IdEspecificidadExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EspecificidadExperiencia>(new Uri(WebApp.BaseAddress), "api/EspecificidadesExperiencia/ListarEspecificidadesExperiencia"), "IdEspecificidadExperiencia", "Descripcion");
                //ViewData["IdAnoExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AnoExperiencia>(new Uri(WebApp.BaseAddress), "api/AnosExperiencia/ListarAnosExperiencia"), "IdAnoExperiencia", "Descripcion");
                ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

                return View(ExperienciaLaboralRequerida);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando una experiencia laboral requerida",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

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
                                                                  "api/ExperienciaLaboralRequeridas");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ExperienciaLaboralRequerida>(respuesta.Resultado.ToString());

                    ViewData["IdEspecificidadExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EspecificidadExperiencia>(new Uri(WebApp.BaseAddress), "api/EspecificidadesExperiencia/ListarEspecificidadesExperiencia"), "IdEspecificidadExperiencia", "Descripcion");
                    //ViewData["IdAnoExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AnoExperiencia>(new Uri(WebApp.BaseAddress), "api/AnosExperiencia/ListarAnosExperiencia"), "IdAnoExperiencia", "Descripcion");
                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

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
        public async Task<IActionResult> Edit(string id, ExperienciaLaboralRequerida ExperienciaLaboralRequerida)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, ExperienciaLaboralRequerida, new Uri(WebApp.BaseAddress),
                                                                 "api/ExperienciaLaboralRequeridas");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Experiencia Laboral Requerida:", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una experiencia laboral requerida",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;

                    ViewData["IdEspecificidadExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EspecificidadExperiencia>(new Uri(WebApp.BaseAddress), "api/EspecificidadesExperiencia/ListarEspecificidadesExperiencia"), "IdEspecificidadExperiencia", "Descripcion");
                   // ViewData["IdAnoExperiencia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AnoExperiencia>(new Uri(WebApp.BaseAddress), "api/AnosExperiencia/ListarAnosExperiencia"), "IdAnoExperiencia", "Descripcion");
                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

                    return View(ExperienciaLaboralRequerida);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una experiencia laboral requerida",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<ExperienciaLaboralRequerida>();
            try
            {
                lista = await apiServicio.Listar<ExperienciaLaboralRequerida>(new Uri(WebApp.BaseAddress)
                                                                    , "api/ExperienciaLaboralRequeridas/ListarExperienciaLaboralRequeridas");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando una experiencia laboral requerida",
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
                                                               , "api/ExperienciaLaboralRequeridas");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de esperiencia laboral requerida",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = response.Message });
                //return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar una experiencia laboral requerida",
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