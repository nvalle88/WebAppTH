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
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class InstitucionesFinancierasController : Controller
    {
        private readonly IApiServicio apiServicio;


        public InstitucionesFinancierasController(IApiServicio apiServicio)
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
        public IActionResult Create()
        {
            InicializarMensaje(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstitucionFinanciera InstitucionFinanciera)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(InstitucionFinanciera);

            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(InstitucionFinanciera,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/InstitucionesFinancieras/InsertarInstitucionFinanciera");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                    "InstitucionesFinancieras",
                    "Index",
                    $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                return View(InstitucionFinanciera);

            }
            catch (Exception ex)
            {
               
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
                                                                  "api/InstitucionesFinancieras");


                    respuesta.Resultado = JsonConvert.DeserializeObject<InstitucionFinanciera>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, InstitucionFinanciera InstitucionFinanciera)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, InstitucionFinanciera, new Uri(WebApp.BaseAddress),
                                                                 "api/InstitucionesFinancieras");

                    if (response.IsSuccess)
                    {
                        return this.RedireccionarMensajeTime(
                    "InstitucionesFinancieras",
                    "Index",
                    $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                    return View(InstitucionFinanciera);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<InstitucionFinanciera>();
            try
            {
                lista = await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress)
                                                                    , "api/InstitucionesFinancieras/ListarInstitucionesFinancieras");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/InstitucionesFinancieras");
                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                    "InstitucionesFinancieras",
                    "Index",
                    $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
               
                }
                return this.RedireccionarMensajeTime(
                    "InstitucionesFinancieras",
                    "Index",
                    $"{Mensaje.Error}|{response.Message}|{"7000"}"
                    );
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}