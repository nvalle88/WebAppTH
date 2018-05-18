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
using EnviarCorreo;
using Microsoft.AspNetCore.Authorization;

namespace bd.webappth.web.Controllers.MVC
{
    
    public class PresupuestoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public PresupuestoController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create(Presupuesto presupuesto)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(presupuesto);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(presupuesto,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Presupuesto/InsertarPresupuesto");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;
                return View(presupuesto);

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
                                                                  "api/Presupuesto");


                    respuesta.Resultado = JsonConvert.DeserializeObject<Presupuesto>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(string id, Presupuesto presupuesto)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, presupuesto, new Uri(WebApp.BaseAddress),
                                                                 "api/Presupuesto");

                    if (response.IsSuccess)
                    {
                        
                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(presupuesto);

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

            var lista = new List<Presupuesto>();
            try
            {
                lista = await apiServicio.Listar<Presupuesto>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Presupuesto/ListarPresupuesto");
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
                                                               , "api/Presupuesto");
                if (response.IsSuccess)
                {
                  
                    return RedirectToAction("Index");
                }
                //return RedirectToAction("Index");
                return RedirectToAction("Index", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {
               

                return BadRequest();
            }
        }
    }
}