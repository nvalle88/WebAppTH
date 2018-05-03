using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{
    public class ConjuntoNominaController: Controller
    {
       
        private readonly IApiServicio apiServicio;

        public ConjuntoNominaController(IApiServicio apiServicio)
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
            InicializarMensaje(mensaje);
            await CargarCombox();
            return View();
        }

        public async Task CargarCombox()
        {
            ViewData["IdTipoConjunto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoConjuntoNomina>(new Uri(WebApp.BaseAddress), "api/TipoConjuntoNomina/ListarTipoConjuntoNomina"), "IdTipoConjunto", "Descripcion");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConjuntoNomina ConjuntoNomina)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ConjuntoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ConjuntoNomina/InsertarConjuntoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }

                ViewData["Error"] = response.Message;
                await CargarCombox();
                return View(ConjuntoNomina);

            }
            catch (Exception ex)
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
                    var ConjuntoNomina = new ConjuntoNomina { IdConjunto = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(ConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/ConjuntoNomina/ObtenerConjuntoNomina");
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        var vista = JsonConvert.DeserializeObject<ConjuntoNomina>(respuesta.Resultado.ToString());
                        await CargarCombox();
                        return View(vista);
                    }
                }

                 return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConjuntoNomina ConjuntoNomina)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                if (ConjuntoNomina.IdConjunto > 0)
                {
                    response = await apiServicio.EditarAsync<Response>(ConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ConjuntoNomina/EditarConjuntoNomina");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    ViewData["Error"] = response.Message;
                    await CargarCombox();
                    return View(ConjuntoNomina);
                }
                return BadRequest();
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
                var lista = await apiServicio.Listar<ConjuntoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/ConjuntoNomina/ListarConjuntoNomina");
                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.ErrorListado}");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index");
                }
                var tipoConjuntoEliminar = new ConjuntoNomina { IdConjunto = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/ConjuntoNomina/EliminarConjuntoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception ex)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

    }
}
