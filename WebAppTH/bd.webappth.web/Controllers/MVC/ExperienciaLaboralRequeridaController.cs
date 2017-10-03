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

namespace bd.webappth.web.Controllers.MVC
{
    public class ExperienciaLaboralRequeridaController : Controller
    {

        private readonly IApiServicio apiServicio;

        public ExperienciaLaboralRequeridaController(IApiServicio apiServicio)
        {

            this.apiServicio = apiServicio;

        }



        public IActionResult Index()
        {
            return View();
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
                                                               , "/api/ExperienciaLaboralRequeridas/EliminarIncideOcupacionalExperienciaLaboralRequeridas");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1} {2} {3}", "Experiencia laboral requerida ",
                                                                                        experienciaLaboralRequerida.IdExperienciaLaboralRequerida, "Índice Ocupacional", experienciaLaboralRequerida.IdIndiceOcupacional),
                        Message = Mensaje.Satisfactorio,
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Detalles", "IndicesOcupacionales", new { id = experienciaLaboralRequerida.IdIndiceOcupacional });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Area de Conocimiento",
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