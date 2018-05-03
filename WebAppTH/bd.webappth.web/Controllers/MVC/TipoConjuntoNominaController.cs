using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class TipoConjuntoNominaController : Controller
    {

        private readonly IApiServicio apiServicio;

        public TipoConjuntoNominaController(IApiServicio apiServicio)
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

         
        public IActionResult Create(string mensaje)
        {
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoConjuntoNomina TipoConjuntoNomina)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(TipoConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(TipoConjuntoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TipoConjuntoNomina/InsertarTipoConjuntoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                return View(TipoConjuntoNomina);

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCrear}");
            }
        }
         
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var TipoConjuntoNomina = new TipoConjuntoNomina { IdTipoConjunto = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(TipoConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/TipoConjuntoNomina/ObtenerTipoConjuntoNomina");
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        var vista = JsonConvert.DeserializeObject<TipoConjuntoNomina>(respuesta.Resultado.ToString());
                        return View(vista);
                    }
                }

                return RedirectToAction("Index",new {mensaje=Mensaje.RegistroNoEncontrado});
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TipoConjuntoNomina TipoConjuntoNomina)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(TipoConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                if (TipoConjuntoNomina.IdTipoConjunto>0)
                {
                    response = await apiServicio.EditarAsync<Response>(TipoConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/TipoConjuntoNomina/EditarTipoConjuntoNomina");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    return View(TipoConjuntoNomina);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }
        }
         
        public async Task<IActionResult> Index(string mensaje)
        {

            try
            {
                InicializarMensaje(mensaje);
               var lista = await apiServicio.Listar<TipoConjuntoNomina>(new Uri(WebApp.BaseAddress)
                                                                    , "api/TipoConjuntoNomina/ListarTipoConjuntoNomina");
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
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
                }
                var tipoConjuntoEliminar = new TipoConjuntoNomina { IdTipoConjunto = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/TipoConjuntoNomina/EliminarTipoConjuntoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }
    }
}