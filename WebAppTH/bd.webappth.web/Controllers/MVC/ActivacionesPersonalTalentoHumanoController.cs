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
    public class ActivacionesPersonalTalentoHumanoController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public ActivacionesPersonalTalentoHumanoController(IApiServicio apiServicio)
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
            InicializarMensaje("");

            List<ActivarPersonalTalentoHumanoViewModel> lista = new List<ActivarPersonalTalentoHumanoViewModel>();


            try
            {
                var listaDependencias = await apiServicio.Listar<Dependencia>(new Uri(WebApp.BaseAddress)
                                                                  , "api/ActivacionesPersonalTalentoHumano/ListarDependencias");

                var lista2 = listaDependencias.Select(x => new ActivarPersonalTalentoHumanoViewModel
                {
                    IdDependencia = x.IdDependencia,
                    Fecha = DateTime.Now,
                    Estado = false,
                    Nombre = x.Nombre
                }

                ).ToList();

                lista = lista2;

                return View(lista);


            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;

                return View(lista);

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<ActivarPersonalTalentoHumanoViewModel> model)
        {

            ListaActivarPersonalTalentoHumanoViewModel listaViewModel = new ListaActivarPersonalTalentoHumanoViewModel();
            listaViewModel.listaAPTHVM = model;


            Response response = await apiServicio.InsertarAsync(listaViewModel,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/ActivacionesPersonalTalentoHumano/InsertarActivacionesPersonalTalentoHumano");
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "ActivacionesPersonalTalentoHumano", new { mensaje = response.Message });
                
            }



            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }




    }
}