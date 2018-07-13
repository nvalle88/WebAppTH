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
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class TitulosController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TitulosController(IApiServicio apiServicio)
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

        public async Task<IActionResult> Create(string mensaje)
        {
            ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
            ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");

            InicializarMensaje(mensaje);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Titulo titulo)
        {
            ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
            ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                return View(titulo);
            }

                Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(titulo,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Titulos/InsertarTitulo");
                if (response.IsSuccess)
                {
                    
                    return this.Redireccionar(
                             "Titulos",
                             "Index",
                             $"{Mensaje.Success}|{response.Message}"
                          );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");
                return View(titulo);

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
                                                                  "api/Titulos");


                    respuesta.Resultado = JsonConvert.DeserializeObject<Titulo>(respuesta.Resultado.ToString());
                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                    ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");
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
        public async Task<IActionResult> Edit(string id, Titulo titulo)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, titulo, new Uri(WebApp.BaseAddress),
                                                                 "api/Titulos");

                    if (response.IsSuccess)
                    {
                        return this.Redireccionar(
                             "Titulos",
                             "Index",
                             $"{Mensaje.Success}|{response.Message}"
                          );
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";


                    ViewData["IdEstudio"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                    ViewData["IdAreaConocimiento"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<AreaConocimiento>(new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientos"), "IdAreaConocimiento", "Descripcion");
                    return View(titulo);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un título",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            var lista = new List<Titulo>();
            try
            {
                lista = await apiServicio.Listar<Titulo>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Titulos/ListarTitulos");
                InicializarMensaje(mensaje);

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
                                                               , "api/Titulos");
                if (response.IsSuccess)
                {

                    return this.Redireccionar(
                             "Titulos",
                             "Index",
                             $"{Mensaje.Success}|{response.Message}"
                          );
                }
                
                return this.RedireccionarMensajeTime(
                             "Titulos",
                             "Index",
                             $"{Mensaje.Error}|{response.Message}|{"10000"}"
                          );

            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }
    }
}