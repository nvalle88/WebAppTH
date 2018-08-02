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
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class ParroquiaController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ParroquiaController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<JsonResult> ListarProvinciaPorPais(string pais)
        {
            var Pais = new Pais
            {
                IdPais = Convert.ToInt32(pais),
            };
            var listaProvincias = await apiServicio.Listar<Provincia>(Pais, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            return Json(listaProvincias);
        }

        public async Task<JsonResult> ListarCiudadPorProvincia(string provincia)
        {
            var Provincia = new Provincia
            {
                IdProvincia = Convert.ToInt32(provincia),
            };
            var listaCiudades = await apiServicio.Listar<Ciudad>(Provincia, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorProvincia");
            return Json(listaCiudades);
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
            ViewData["IdPais"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdProvincia"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Parroquia parroquia)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                ViewData["IdPais"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                ViewData["IdProvincia"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
                ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"10000"}";
                return View(parroquia);
            }
            else
            {
                ViewData["IdPais"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                ViewData["IdProvincia"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
                ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            }
            
                
            

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(parroquia,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Parroquia/InsertarParroquia");
                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                           "Parroquia",
                           "Index",
                           $"{Mensaje.Success}|{response.Message}|{"7000"}"
                        );
                }


                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                return View(parroquia);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {

            ViewData["IdPais"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdProvincia"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/Parroquia");


                    var Resultado = JsonConvert.DeserializeObject<Parroquia>(respuesta.Resultado.ToString());

                    if (respuesta.IsSuccess)
                    {
                        var p = new Parroquia {Nombre = Resultado.Nombre, IdCiudad = Resultado.IdCiudad,IdProvincia = Resultado.Ciudad.Provincia.IdProvincia, IdPais = Resultado.Ciudad.Provincia.Pais.IdPais };
                        
                        return View(p);
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
        public async Task<IActionResult> Edit(string id, Parroquia parroquia)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, parroquia, new Uri(WebApp.BaseAddress),
                                                                 "api/Parroquia");
                    
                   

                    if (response.IsSuccess)
                    {

                        return this.RedireccionarMensajeTime(
                            "Parroquia",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                    }


                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                    ViewData["IdPais"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
                    ViewData["IdProvincia"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
                    ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
                    
                    return View(parroquia);

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

            var lista = new List<Parroquia>();
            try
            {
                lista = await apiServicio.Listar<Parroquia>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Parroquia/ListarParroquia");
                
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
                var response = await apiServicio.EliminarAsync(
                    id, new Uri(WebApp.BaseAddress), "api/Parroquia");


                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "Parroquia",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }

                return this.RedireccionarMensajeTime(
                            "Parroquia",
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