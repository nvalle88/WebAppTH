using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using bd.webappth.entidades.Utils.Seguridad;

namespace bd.webappth.web.Controllers.MVC
{
    public class HomesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public HomesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
           
        }

       [Authorize(Policy = PoliticasSeguridad.TienePermiso)]
        public async Task<IActionResult> Index()
        {

            var lista = new List<Noticia>();
            try
            {
                lista = await apiServicio.Listar<Noticia>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Homes/ListarNoticias");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando noticias",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

    }
}