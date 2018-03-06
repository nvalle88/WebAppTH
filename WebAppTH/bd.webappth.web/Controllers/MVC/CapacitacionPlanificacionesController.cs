using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.ViewModels;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class CapacitacionPlanificacionesController : Controller
    {

        private readonly IApiServicio apiServicio;


        public CapacitacionPlanificacionesController(IApiServicio apiServicio)
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

            //ViewData["ListaCapacitacionTemario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<CapacitacionPlanificacionViewModel>(new Uri(WebApp.BaseAddress), "api/CapacitacionesTemarios/ListarCapacitacionesTemarios"), "IdCapacitacionTemario", "Tema");

            //return View();

            var lista = new List<CapacitacionPlanificacionViewModel>();
            try
            {
                lista = await apiServicio.Listar<CapacitacionPlanificacionViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/CapacitacionPlanificaciones/ListarCapacitacionPlanificaciones");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando tipos de preguntas de capacitación",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }


       // [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(string mensaje)
        {
                InicializarMensaje(mensaje);
                return View();
           
        }
    }
}