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
using System.Security.Claims;

namespace bd.webappth.web.Controllers.MVC
{
    public class SituacionActualController : Controller
    {

        private readonly IApiServicio apiServicio;


        public SituacionActualController(IApiServicio apiServicio)
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



        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);

            var lista = new List<DistributivoViewModel>();

            try
            {
                // Obtención de datos para generar pantalla
                lista = await apiServicio.Listar<DistributivoViewModel>(new Uri(WebApp.BaseAddress)
                    , "api/ActivacionesPersonalTalentoHumano/ObtenerSituacionActual");

                return View(lista);


            }
            catch (Exception ex)
            {
                

                InicializarMensaje(mensaje);

                return View(lista);

            }
        }
        

    }
}