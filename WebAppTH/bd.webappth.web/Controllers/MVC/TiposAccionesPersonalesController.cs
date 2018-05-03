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
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class TiposAccionesPersonalesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TiposAccionesPersonalesController(IApiServicio apiServicio)
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


        public async Task<IActionResult> Create()
        {

            var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
            {

                MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },
                TipoAccionPersonal = new TipoAccionPersonal {
                    NDiasMaximo = 0,
                    NDiasMinimo = 0,
                    NHorasMaximo = 0,
                    NHorasMinimo = 0
              
                }
            };

            ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");
            ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");


            return View(tipoAccionPersonalViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoAccionPersonal TipoAccionPersonal)
        {

            

            Response response = new Response();
            try
            {

                var model = new TipoAccionPersonalViewModel
                {

                    MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },
                    TipoAccionPersonal = new TipoAccionPersonal
                    {
                        NDiasMaximo = 0,
                        NDiasMinimo = 0,
                        NHorasMaximo = 0,
                        NHorasMinimo = 0

                    }
                };

                if (!ModelState.IsValid)
                {

                    ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");
                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(model.MatrizLista, "Id", "Nombre");

                    return View(model);
                }



                response = await apiServicio.InsertarAsync(TipoAccionPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TiposAccionesPersonales/InsertarTipoAccionPersonal");

                var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
                {

                    MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}                            },

                };



                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = response.Message});
                }

                ViewData["Error"] = response.Message;
                ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");
                ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");

                return View(tipoAccionPersonalViewmodel);

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
                                                                  "api/TiposAccionesPersonales");


                    respuesta.Resultado = JsonConvert.DeserializeObject<TipoAccionPersonal>(respuesta.Resultado.ToString());

                    var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
                    {

                        MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },


                        TipoAccionPersonal = (TipoAccionPersonal)respuesta.Resultado
                    };

                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");
                    ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");


                    if (respuesta.IsSuccess)
                    {
                        return View(tipoAccionPersonalViewmodel);
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
        public async Task<IActionResult> Edit(string id, TipoAccionPersonal TipoAccionPersonal)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    
                    var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
                    {

                        MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },
                        
                    };

                    response = await apiServicio.EditarAsync(id, TipoAccionPersonal, new Uri(WebApp.BaseAddress),
                                                                 "api/TiposAccionesPersonales");
                    

                    if (response.IsSuccess)
                    {

                        return RedirectToAction("Index", new { mensaje = response.Message});
                    }
                    ViewData["Error"] = response.Message;


                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");
                    ViewData["IdEstadoTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<EstadoTipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/EstadosTiposAccionPersonal/ListarEstadosTiposAccionPersonal"), "IdEstadoTipoAccionPersonal", "Nombre");


                    return View(tipoAccionPersonalViewmodel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);

            var lista = new List<TipoAccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress)
                                                                    , "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");
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
                                                               , "api/TiposAccionesPersonales");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = response.Message});
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
    }
}