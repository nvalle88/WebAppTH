using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using Newtonsoft.Json;
using bd.webappth.entidades.Utils;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.Servicios;
using System.Security.Claims;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class ConsultasVacacionesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ConsultasVacacionesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }



        public async Task<ActionResult> Index()
        {

            try
            {
                return RedirectToAction("AgregarPiePagina", "GenerarFirmas", new { NombreReporteConParametros = "RepVacacionesNoGozadas" });

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }




    }
}